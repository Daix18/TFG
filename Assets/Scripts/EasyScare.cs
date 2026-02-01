using System.Collections;
using UnityEngine;

public class EasyScare : MonoBehaviour
{
    public Light Light;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LightFlick());
        }
    }

    IEnumerator LightFlick()
    {
        Light.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Light.enabled = true;
    }
}
