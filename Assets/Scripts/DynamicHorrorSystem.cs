
using UnityEngine;

public class DynamicHorrorSystem : MonoBehaviour
{
    //Design parameters
    [Header("DHG Parameters")]
    [SerializeField] private float _evaluationInterval = 2f;
    [SerializeField] private float _cooldownDuration = 6f;
    [SerializeField] private float _baseProbability = 0.1f;
    [SerializeField] private float _growthFactor = 0.05f;
    [SerializeField] private float _maxProbability = 0.6f;

    //Internal state variables
    float _timeSinceLastEvent;
    float _evaluationTimer;
    float _cooldownTimer;
    bool _isInCooldown;

    //Horror events
    [Header("Horror Event References")]
    [SerializeField] private LightEvent _lightEvent;

    //DataLogger reference
    private DataLogger _dataLogger;

    [Header("Event Weights")]
    [SerializeField] private float lightWeight = 0.3f;
    [SerializeField] private float soundWeight = 0.7f;

    private void Start()
    {
        _dataLogger = FindAnyObjectByType<DataLogger>();

        var controller = FindAnyObjectByType<TechniqueManager>();

        if (controller.SelectedTechnique != Technique.DHG)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastEvent += Time.deltaTime;
        _evaluationTimer += Time.deltaTime;

        if (_isInCooldown)
        {
            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= _cooldownDuration)
            {
                _isInCooldown = false;
                _cooldownTimer = 0f;
            }
        }

        if (_evaluationTimer >= _evaluationInterval)
        {
            Debug.Log("Evaluating horror event trigger...");
            _evaluationTimer = 0f;
            if (!_isInCooldown)
            {
                float prob = _baseProbability + (_timeSinceLastEvent * _growthFactor);
                prob = Mathf.Clamp(prob, 0f, _maxProbability);

                if (Random.value < prob)
                {
                    float randomEvent = Random.value;
                    if (randomEvent < lightWeight)
                    {
                        _lightEvent.TriggerLightEvent();
                        _dataLogger.RegisterEvent("LIGHT_EVENT_DHG");
                    }
                    else 
                    {
                        string soundName = AudioManager.THIS.PlayRandomSound();
                        _dataLogger.RegisterEvent("SOUND_" + soundName);
                    }
                    _isInCooldown = true;
                    _timeSinceLastEvent = 0f;
                }
            }
        }
    }
}
