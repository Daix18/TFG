// Autor: Daniel Izaguirre Montalvo
// TFG - Generación Dinámica de Horror en Unity
// Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales - UDIT 2025/2026

using UnityEngine;

public enum Technique
{
    Baseline,
    DHG,
    AffectiveLoop
}

public class TechniqueManager : MonoBehaviour
{
    [SerializeField] private Technique  _selectedTechinique;
    public Technique SelectedTechnique => _selectedTechinique;

    private void Awake()
    {
        //int random = Random.Range(0, 3);
        //_selectedTechinique = (Technique)random;
        Debug.Log("Selected Technique: " + _selectedTechinique);
    }
}
