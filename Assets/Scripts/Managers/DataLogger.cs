// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using StarterAssets;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public struct LogEntry
{
    public float Time;
    public float CameraRotationX;
    public float CameraRotationY;
    public float DeltaCameraX;
    public float DeltaCameraY;
    public float Arousal;
    public string EventType;
    public string Technique;
}

public class DataLogger : MonoBehaviour
{
    //Timers
    [SerializeField] private float sampleInterval = 0.2f;
    float timer = 0f;

    //Data variables
    float gameTime;
    public Transform playerCameraRotationX;
    public Transform playerCameraRotationY;

    float previousCameraRotationX;
    float previousCameraRotationY;

    AffectiveLoopSystem _affectiveLoopSystem;

    //Variable Logger
    List<LogEntry> logEntries = new List<LogEntry>();

    [SerializeField] private Technique _currentTechnique;


    private void Start()
    {
        var manager = FindFirstObjectByType<TechniqueManager>();
        _currentTechnique = manager.SelectedTechnique;
        _affectiveLoopSystem = FindFirstObjectByType<AffectiveLoopSystem>();
        previousCameraRotationX = playerCameraRotationX.transform.eulerAngles.y;
        previousCameraRotationY = playerCameraRotationX.transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTime += Time.deltaTime;

        if (timer >= sampleInterval)
        {
            logEntries.Add(CreateLogEntry("STATE"));
            timer -= sampleInterval;
        }

    }

    public void SaveToCSV()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string csv = "Time;CameraRotationX;CameraRotationY;DeltaCameraX;DeltaCameraY;Arousal;EventType;Technique\n";
        string fileName = _currentTechnique.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";
        string filePath = desktopPath + "/" + fileName;
        //Write to CSV string
        foreach (LogEntry outputentry in logEntries)
        {
            csv += outputentry.Time.ToString("F3") + ";" + 
                outputentry.CameraRotationX + ";" + 
                outputentry.CameraRotationY + ";" +
                outputentry.DeltaCameraX + ";" +
                outputentry.DeltaCameraY + ";" +
                outputentry.Arousal + ";" +
                outputentry.EventType + ";" +
                outputentry.Technique + "\n";
        }
        File.WriteAllText(filePath, csv);
        Debug.Log("Data saved to: " + filePath);
    }
    public LogEntry CreateLogEntry(string eventName)
    {
        LogEntry entry = new LogEntry();
        float CameraRotationX = playerCameraRotationX.transform.eulerAngles.y;
        float CameraRotationY = playerCameraRotationY.transform.eulerAngles.x;


        entry.Time = gameTime;
        entry.CameraRotationX = CameraRotationX;
        entry.CameraRotationY = CameraRotationY;
        entry.DeltaCameraX = Mathf.DeltaAngle(previousCameraRotationX, CameraRotationX);
        entry.DeltaCameraY = Mathf.DeltaAngle(previousCameraRotationY, CameraRotationY);
        entry.Arousal = _affectiveLoopSystem._arousal;
        entry.EventType = eventName;
        entry.Technique = _currentTechnique.ToString();

        previousCameraRotationX = CameraRotationX;
        previousCameraRotationY = CameraRotationY;

        return entry;
    }

    public void RegisterEvent(string eventName)
    {
        logEntries.Add(CreateLogEntry(eventName));
    }

    private void OnDisable()
    {
        SaveToCSV();
    }
}
