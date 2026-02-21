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
}
