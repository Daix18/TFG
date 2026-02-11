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
}


public class DataLogger : MonoBehaviour
{
    //Timers
    float sampleInterval = 0.2f;
    float timer = 0f;

    //Data variables
    float gameTime;
    public Transform playerCameraRotationX;
    public Transform playerCameraRotationY;

    //Variable Logger
    List<LogEntry> logEntries = new List<LogEntry>();

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTime += Time.deltaTime;

        float CameraRotationX = playerCameraRotationX.transform.eulerAngles.y;
        float  CameraRotationY = playerCameraRotationY.transform.eulerAngles.x;

        if (timer >= sampleInterval)
        {
            LogEntry entry = new LogEntry();
            entry.Time = gameTime;
            entry.CameraRotationX = CameraRotationX;
            entry.CameraRotationY = CameraRotationY;

            logEntries.Add(entry);
            timer -= sampleInterval ;
        }
    }

    void SaveToCSV()
    {
        string csv = "Time;CameraRotationX;CameraRotationY\n";
        string fileName = "Baseline_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";
        string filePath = Application.persistentDataPath + "/" + fileName;
        //Write to CSV string
        foreach (LogEntry outputentry in logEntries)
        {
            csv += outputentry.Time.ToString("F3") + ";" + 
                outputentry.CameraRotationX + ";" + 
                outputentry.CameraRotationY + "\n";
        }
        File.WriteAllText(filePath, csv);
        Debug.Log("Data saved to: " + filePath);
    }

    private void OnApplicationQuit()
    {
        SaveToCSV();
    }
}
