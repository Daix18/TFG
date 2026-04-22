using System.Collections;
using UnityEngine;

public class LightEvent : MonoBehaviour
{
    public Light _light;
    bool hasTriggered;

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
        if (other.CompareTag("Player"))
        {
            if (hasTriggered) return;

            StartCoroutine(Flicker(0.5f));
        }
    }

    public void TriggerLightEvent(float intensity)
    {
        StartCoroutine(Flicker(intensity));
    }

    IEnumerator Flicker(float intensity)
    {
        hasTriggered = true;

        int flickers = Mathf.RoundToInt(Mathf.Lerp(2, 10, intensity));
        float speed = Mathf.Lerp(0.2f, 0.05f, intensity);

        for (int i = 0; i < flickers; i++)
        {
            _light.enabled = !_light.enabled;
            yield return new WaitForSeconds(speed);
        }

        _light.enabled = true;
        yield return new WaitForSeconds(1f);

        hasTriggered = false;
    }
}
