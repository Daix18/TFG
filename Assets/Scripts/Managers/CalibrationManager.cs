using UnityEngine;
using UnityEngine.InputSystem;


public class CalibrationManager : MonoBehaviour
{
    public bool _calibracionActive = false;
    public float _baseSensitivity = 1.0f;

    float _calibrationTimer = 5.0f;
    float _sumMovement;
    float _numofSamples;


    private void Update()
    {
        if(!_calibracionActive) return;

        float calibrationDeltaX = Mouse.current.delta.x.ReadValue();
        float calibrationDeltaY = Mouse.current.delta.y.ReadValue();
        float movimiento = new Vector2(calibrationDeltaX, calibrationDeltaY).magnitude;
        
        _sumMovement += movimiento;
        _numofSamples++;
        _calibrationTimer -= Time.deltaTime;

        if (_calibrationTimer <= 0f)
        {
            _calibracionActive = false;
            _baseSensitivity = _numofSamples > 0 ? _sumMovement / _numofSamples : 1f;
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
