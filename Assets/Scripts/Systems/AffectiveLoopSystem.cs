// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using UnityEngine;
using UnityEngine.Rendering;

public class AffectiveLoopSystem : MonoBehaviour
{
    [Header("Arousal")]
    [SerializeField] float _arousalIncrease = 0.8f;
    [SerializeField] float _arousalDecay = 0.5f;
    [SerializeField] float _maxArousal = 1f;

    [Header("Events")]
    [SerializeField] float _baseProbability = 0.1f;
    [SerializeField] float _arousalMultiplier = 0.5f;

    [Header("Timing")]
    [SerializeField] float _evaluationInterval = 2f;
    [SerializeField] float _cooldownDuration = 5f;
    [SerializeField] float _warmupDuration = 2f;

    [HideInInspector] public float _arousal;
    float _evaluationTimer;
    float _cooldownTimer;
    float warmupTimer;
    bool _isInCooldown;

    float _lastX;
    float _lastY;
    float _playerSpeed;

    // References 
    DataLogger _dataLogger;
    CharacterController _characterController;
    CalibrationManager _calibrationManager;

    //Jumpscare variables
    [SerializeField] JumpScare _jumpscareEvent;

    void Start()
    {
        _dataLogger = FindAnyObjectByType<DataLogger>();
        _characterController = FindAnyObjectByType<CharacterController>();
        _calibrationManager = FindAnyObjectByType<CalibrationManager>();

        var controller = FindAnyObjectByType<TechniqueManager>();

        if (controller.SelectedTechnique != Technique.AffectiveLoop)
        {
            enabled = false;
        }
    }

    void Update()
    {
        UpdateArousal();
        HandleCooldown();
        EvaluateEvents();
    }

    void UpdateArousal()
    {
        Vector3 velocity = _characterController.velocity;
        velocity.y = 0f;
        _playerSpeed = velocity.magnitude;

        float currentX = transform.eulerAngles.x;
        float currentY = transform.eulerAngles.y;

        float deltaX = Mathf.DeltaAngle(_lastX, currentX);
        float deltaY = Mathf.DeltaAngle(_lastY, currentY);

        float rawMovement = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY) / Time.deltaTime;

        float movement = rawMovement * 0.01f;
        movement = Mathf.Clamp01(movement);

        if (movement < 0.05f)
            movement = 0f;

        if (_playerSpeed < 0.1f)
            _playerSpeed = 0f;

        float combined = movement + _playerSpeed * 0.1f;
        combined = Mathf.Clamp01(combined);
        combined = Mathf.Pow(combined, 1.5f);

        float normalizeMovement = movement / _calibrationManager._baseSensitivity;
        normalizeMovement = Mathf.Clamp01(normalizeMovement);

        float increase = normalizeMovement * _arousalIncrease * Time.deltaTime;
        increase = Mathf.Min(increase, 0.03f);

        _arousal += increase;
        _arousal -= _arousalDecay * Time.deltaTime;

        _arousal = Mathf.Clamp(_arousal, 0f, _maxArousal);

        _lastX = currentX;
        _lastY = currentY;
    }

    void HandleCooldown()
    {
        if (_isInCooldown)
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= _cooldownDuration)
            {
                _isInCooldown = false;
                _cooldownTimer = 0f;
            }
        }
    }

    void EvaluateEvents()
    {
        _evaluationTimer += Time.deltaTime;

        float prob = _baseProbability + (_arousal * _arousalMultiplier);
        prob = Mathf.Clamp(prob, 0f, 1f);

        if (_evaluationTimer >= _evaluationInterval)
        {
            _evaluationTimer = 0f;

            if (_isInCooldown) return;


            if (Random.value < prob)
            {
                TriggerEvent();
                _isInCooldown = true;
            }
        }
    }

    void TriggerEvent()
    {
        if (_arousal < 0.3f)
        {
            // sonido suave
            string sound = AudioManager.THIS.PlayRandomSound();
            _dataLogger.RegisterEvent("LOW_LIGHT_EVENT_AFFECTIVE");
            LightManager.THIS.TriggerClosestLight(0.2f);
            Debug.Log("Triggered Low Light Affective Event");
        }
        else if (_arousal < 0.7f)
        {
            // evento medio
            LightManager.THIS.TriggerClosestLight(0.5f);
            _dataLogger.RegisterEvent("MEDIUM_LIGHT_EVENT_AFFECTIVE");
            Debug.Log("Triggered Medium Light Affective Event");
        }
        else
        {
            if (Random.value < 0.7f)
            {
                AudioManager.THIS.PlayRandomSound();
            }

            if (_arousal > 0.8f && Random.value < 0.4f)
            {
                _jumpscareEvent.TriggerJumpScare();
                _dataLogger.RegisterEvent("JUMPSCARE_AFFECTIVE");
            }
            else
            {
                _dataLogger.RegisterEvent("STRONG_LIGHT_EVENT_AFFECTIVE");
                LightManager.THIS.TriggerClosestLight(1f);
                Debug.Log("Triggered Strong Light Affective Event");
            }
        }
    }
}
