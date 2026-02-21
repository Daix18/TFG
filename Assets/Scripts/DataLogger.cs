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

    //Variable Logger
    List<LogEntry> logEntries = new List<LogEntry>();

    [SerializeField] private Technique _currentTechnique;


    private void Start()
    {
        var manager = FindFirstObjectByType<TechniqueManager>();
        _currentTechnique = manager.SelectedTechnique;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTime += Time.deltaTime;

        if (timer >= sampleInterval)
        {
            logEntries.Add(CreateLogEntry("NONE"));
            timer -= sampleInterval;
        }

    }

    void SaveToCSV()
    {
        string csv = "Time;CameraRotationX;CameraRotationY;EventType;Technique\n";
        string fileName = _currentTechnique.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";
        string filePath = Application.persistentDataPath + "/" + fileName;
        //Write to CSV string
        foreach (LogEntry outputentry in logEntries)
        {
            csv += outputentry.Time.ToString("F3") + ";" + 
                outputentry.CameraRotationX + ";" + 
                outputentry.CameraRotationY + 
                ";" + outputentry.EventType + 
                ";" + outputentry.Technique + "\n";
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
        entry.EventType = eventName;
        entry.Technique = _currentTechnique.ToString();

        return entry;
    }

    public void RegisterEvent(string eventName)
    {
        logEntries.Add(CreateLogEntry(eventName));
    }

    private void OnApplicationQuit()
    {
        SaveToCSV();
    }
}
