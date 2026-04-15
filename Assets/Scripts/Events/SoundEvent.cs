using UnityEngine;

public class SoundEvent : MonoBehaviour
{
    void Start()
    {
        var controller = FindAnyObjectByType<TechniqueManager>();

        if (controller.SelectedTechnique != Technique.Baseline)
            GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            AudioManager.THIS.PlayRandomSound();
    }
}
