using UnityEngine;

public class CameraBreathingEffect : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float velocity;
    float time;

    private void LateUpdate()
    {
        time += Time.deltaTime;
        Breathing();
    }

    public void Breathing()
    {
        Vector3 rot = transform.localEulerAngles;
        float offset = Mathf.Sin(time * velocity) * intensity;
        rot.x =  offset;
        transform.localEulerAngles = rot;
    }
}
