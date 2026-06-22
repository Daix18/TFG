// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using UnityEngine;

public class BaseLineSystem : MonoBehaviour
{
    [SerializeField]  LightEvent _lightEvent;
    [SerializeField]  DataLogger _dataLogger;

    bool _triggered = false;

    private void Start()
    {
        var controller = FindAnyObjectByType<TechniqueManager>();

        if (controller.SelectedTechnique != Technique.Baseline)
        {
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;

        if (other.CompareTag("Player"))
        {
            _triggered = true;

            Invoke(nameof(TriggerEvent), 1.5f);
        }
    }

    void TriggerEvent()
    {
        _lightEvent.TriggerLightEvent(0.2f);
        _dataLogger.RegisterEvent("LIGHT_EVENT_BASELINE");
    }
}
