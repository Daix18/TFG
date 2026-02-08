using StarterAssets;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    float gameTime;
    FirstPersonController playerController;
    public Transform playerCameraRotationX;
    public Transform playerCameraRotationY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        float CameraRotationX = playerCameraRotationX.transform.eulerAngles.y;
        float  CameraRotationY = playerCameraRotationY.transform.eulerAngles.x;
        Debug.Log("Time: " + gameTime + " || " + "Camera Rotation Y: " + CameraRotationY + " || " + "Camera Rotation X: " + CameraRotationX);
    }
}
