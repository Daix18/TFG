using UnityEngine;
using UnityEngine.InputSystem;


public class CalibrationManager : MonoBehaviour
{
    public bool _calibracionActive = false;
    public float _baseSensitivity = 1.0f;
    public float _movement;
    public Transform _playerCamera;

    float _calibrationTimer = 5.0f;
    float _sumMovement;
    float _numofSamples;
    float _lastY;
    float _lastX;


    private void Update()
    {
        if(!_calibracionActive) return;

        float currentX = _playerCamera.eulerAngles.x;
        float currentY = _playerCamera.eulerAngles.y;

        float deltaX = Mathf.DeltaAngle(_lastX, currentX);
        float deltaY = Mathf.DeltaAngle(_lastY, currentY);

        float rawMovement = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY) / Time.deltaTime;

        _movement = rawMovement * 0.01f;
        _movement = Mathf.Clamp01(_movement);

        _lastX = currentX;
        _lastY = currentY;

        _sumMovement += _movement;
        _numofSamples++;
        _calibrationTimer -= Time.deltaTime;

        if (_calibrationTimer <= 0f)
        {
            if (_sumMovement < 0.5f)
            {
                StartCalibration();
                Debug.Log("Calibration restarted due to insufficient movement.");
            }
            else
            {
                _calibracionActive = false;
                _calibrationTimer = 5.0f;
                _baseSensitivity = _sumMovement / _numofSamples;
                GameManager.THIS.ShowInstructions();
                GameManager.THIS.CalibrationEnded();
            }
        }
    }

    public void StartCalibration()
    {
        _calibracionActive = true;
        _calibrationTimer = 5.0f;
        _sumMovement = 0f;
        _numofSamples = 0f;
    }
}
