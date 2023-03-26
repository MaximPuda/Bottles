using UnityEngine;

public class LevelPrefs : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private string _name;
    [SerializeField] private int _moves;

    private WagonController _wagon;
    private GridController _grid;
    private Tutorial _tutor;

    public string LevelName => _name;
    public int Moves => _moves;
    public WagonController Wagon => _wagon;
    public GridController Grid => _grid;
    public Tutorial Tutorial => _tutor;

    public void Intialize(ParticleSystemForceField coinForceField)
    {
        _wagon = GetComponentInChildren<WagonController>();
        _wagon?.Initialize(coinForceField);
        
        _grid = GetComponentInChildren<GridController>();
        _grid?.Initialize();

        _tutor = GetComponentInChildren<Tutorial>();
        _tutor?.Initialize();
    }
}
