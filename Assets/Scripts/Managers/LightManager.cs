// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager THIS;

    [SerializeField] private LightEvent[] _lightEvents;

    private void Awake()
    {
        THIS = this;
    }

    public void TriggerClosestLight(float intensity)
    {
        Transform player = Camera.main.transform;
        LightEvent closest = null;
        float minDistance = float.MaxValue;

        foreach (LightEvent le in _lightEvents)
        {
            float dist = Vector3.Distance(player.position, le.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = le;
            }
        }

        if (closest != null)
            closest.TriggerLightEvent(intensity);
    }
}
