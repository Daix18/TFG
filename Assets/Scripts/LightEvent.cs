using System.Collections;
using UnityEngine;

public class LightEvent : MonoBehaviour
{
    public Light Light;
    bool hasTriggered;

    private void Start()
    {
        var controller = FindAnyObjectByType<TechniqueManager>();
        if (controller.SelectedTechnique != Technique.Baseline)
        {
            enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LightFlick());
        }
    }

    public void TriggerLightEvent()
    {
        StartCoroutine(LightFlick());
    }

    IEnumerator LightFlick()
    {
        Light.enabled = false;
        hasTriggered = true;
        FindAnyObjectByType<DataLogger>().RegisterEvent("LIGHT_EVENT");
        yield return new WaitForSeconds(1.5f);
        Light.enabled = true;
    }
}
