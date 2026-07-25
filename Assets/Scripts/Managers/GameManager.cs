// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using TMPro;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class GameManager : MonoBehaviour
{
    public static GameManager THIS;

    [Header("Timer Settings")]
    [SerializeField] float duration = 180f; // 3 minutos

    float currentTime;
    bool isRunning = true;
    bool hasStarted = false;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI instructionsText;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject endPanel;
    [SerializeField] Button startButton;

    DataLogger _dataLogger;
    CalibrationManager _calibrationManager;
    FirstPersonController _firstPersonController;

    [SerializeField] StarterAssetsInputs _input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = duration;
        isRunning = false;
        _dataLogger = FindAnyObjectByType<DataLogger>();
        _calibrationManager = FindAnyObjectByType<CalibrationManager>();
        _firstPersonController = FindAnyObjectByType<FirstPersonController>();
        Time.timeScale = 0f; // pausa el juego al inicio
        _input.cursorLocked = false;
        _input.cursorInputForLook = false; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startPanel.SetActive(true);
        startButton.onClick.AddListener(StartCalibration);
        endPanel.SetActive(false);
    }

    private void Awake()
    {
        THIS = this;
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
        _dataLogger.isLogging = true;

        _firstPersonController.MoveSpeed = 4f;
        _firstPersonController.SprintSpeed = 6f;
        _firstPersonController.enabled = true;

        StartTimer();
    }

    public void StartCalibration()
    {
        startPanel.SetActive(false);
        timerText.gameObject.SetActive(true);
        Time.timeScale = 1f;
        _input.cursorLocked = true;
        _input.cursorInputForLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _firstPersonController.MoveSpeed = 0f;
        _firstPersonController.SprintSpeed = 0f;
        _calibrationManager.StartCalibration();
    }

    public void CalibrationEnded()
    {
        startPanel.SetActive(true);
        timerText.gameObject.SetActive(false);
        Time.timeScale = 0f;
        _input.cursorLocked = false;
        _input.cursorInputForLook = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        endPanel.SetActive(false);
        _firstPersonController.MoveSpeed = 0f;
        _firstPersonController.SprintSpeed = 0f;
        _firstPersonController.enabled = false;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ShowInstructions()
    {
        instructionsText.SetText("Muevete libremente por el mapa.\n No te quedes quieto.\nEsta sesión dura 3 minutos.");
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(StartGame);
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
