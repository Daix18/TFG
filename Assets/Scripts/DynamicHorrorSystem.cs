using UnityEngine;

public class DynamicHorrorSystem : MonoBehaviour
{
    //Design parameters
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
    [SerializeField] private LightEvent _lightEvent;

    //DataLogger reference
    private DataLogger _dataLogger;

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

        if (_evaluationTimer >= _evaluationInterval)
        {
            Debug.Log("Evaluating horror event trigger...");
            _evaluationTimer = 0f;
            if (!_isInCooldown)
            {
                float prob = _baseProbability + (_timeSinceLastEvent * _growthFactor);
                float randomValue = Random.value;
                prob = Mathf.Clamp(prob, 0f, _maxProbability);

                if (randomValue < prob)
                {
                    _lightEvent.TriggerLightEvent();
                   _dataLogger.RegisterEvent("LIGHT_EVENT_DHG");
                    _timeSinceLastEvent = 0f;
                    _isInCooldown = true;
                }
            }
            else
            {
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _cooldownDuration)
                {
                    _isInCooldown = false;
                    _cooldownTimer = 0f;
                }
            }
        }
    }
}
