using UnityEngine;

public class AffectiveLoopSystem : MonoBehaviour
{
    [Header("Arousal")]
    [SerializeField] float _arousalIncrease = 0.5f;
    [SerializeField] float _arousalDecay = 0.2f;
    [SerializeField] float _maxArousal = 1f;

    [Header("Events")]
    [SerializeField] float _baseProbability = 0.1f;
    [SerializeField] float _arousalMultiplier = 0.5f;

    [Header("Timing")]
    [SerializeField] float _evaluationInterval = 2f;
    [SerializeField] float _cooldownDuration = 5f;

    float _arousal;
    float _evaluationTimer;
    float _cooldownTimer;
    bool _isInCooldown;

    float _lastX;
    float _lastY;

    // References 
    Transform _playerCamera;
    DataLogger _dataLogger;

    void Start()
    {
        _playerCamera = Camera.main.transform;
        _dataLogger = FindAnyObjectByType<DataLogger>();
    }

    void Update()
    {
        UpdateArousal();
        HandleCooldown();
        EvaluateEvents();
    }

    void UpdateArousal()
    {
        float currentX = _playerCamera.eulerAngles.x;
        float currentY = _playerCamera.eulerAngles.y;

        float deltaX = currentX - _lastX;
        float deltaY = currentY - _lastY;

        float movement = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

        movement = Mathf.Clamp(movement, 0f, 10f); // Prevent extreme values

        _arousal += movement * Time.deltaTime;
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

        if (_evaluationTimer >= _evaluationInterval)
        {
            _evaluationTimer = 0f;

            if (_isInCooldown) return;

            float prob = _baseProbability + (_arousal * _arousalMultiplier);
            prob = Mathf.Clamp(prob, 0f, 1f);

            if (Random.value < prob)
            {
                TriggerEvent();
                _isInCooldown = true;
            }
        }
    }

    void TriggerEvent()
    {
        // luz / sonido / jumpscare
        // + DataLogger
    }
}
