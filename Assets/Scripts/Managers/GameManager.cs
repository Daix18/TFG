// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using TMPro;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] float duration = 180f; // 3 minutos

    float currentTime;
    bool isRunning = true;
    bool hasStarted = false;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject endPanel;

    DataLogger _dataLogger;

    [SerializeField] StarterAssetsInputs _input;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = duration;
        isRunning = false;
        _dataLogger = FindAnyObjectByType<DataLogger>();
        Time.timeScale = 0f; // pausa el juego al inicio
        _input.cursorLocked = false;
        _input.cursorInputForLook = false; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        UpdateUI();

        if (currentTime <= 0f)
        {
            EndGame();
        }
    }

    void UpdateUI()
    {
        currentTime = Mathf.Max(0f, currentTime);
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        if (timerText != null)
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void StartGame()
    {
        if (hasStarted) return;

        hasStarted = true;

        startPanel.SetActive(false);
        timerText.gameObject.SetActive(true);
        Time.timeScale = 1f;
        _input.cursorLocked = true;
        _input.cursorInputForLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartTimer();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    void EndGame()
    {
        isRunning = false;

        Debug.Log("Game Finished");

        endPanel.SetActive(true);

        _input.cursorLocked = false;
        _input.cursorInputForLook = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Guardar datos
        if (_dataLogger != null)
            _dataLogger.SaveToCSV();

        // Opcional: parar el juego
        Time.timeScale = 0f;
    }


    void StartTimer()
    {
        isRunning = true;
        currentTime = duration;
    }

}
