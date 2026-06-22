using UnityEngine;

public class JumpScareEvent : MonoBehaviour
{
    [SerializeField] private JumpScare _jumpScare;
    [SerializeField] private DataLogger _dataLogger;
    [SerializeField] private float _cooldown = 30f;

    private float _lastTriggerTime = -999f;

    private void Start()
    {
        var controller = FindAnyObjectByType<TechniqueManager>();
        if (controller.SelectedTechnique != Technique.Baseline)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - _lastTriggerTime < _cooldown) return;

        _lastTriggerTime = Time.time;
        _jumpScare.TriggerJumpScare();
        _dataLogger.RegisterEvent("JUMPSCARE_BASELINE");
    }
}
