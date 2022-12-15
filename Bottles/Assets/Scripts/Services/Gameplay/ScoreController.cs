using UnityEngine;
using UnityEngine.Events;

public class ScoreController : Controller
{
    [SerializeField] private int _oneComboPoints;
    [SerializeField] private int _doubleComboPoints;
    [SerializeField] private int _penalty;

    public int Points { get; private set; }

    public event UnityAction<int> PonintsChangedEvent;

    private WagonController _currentWagon;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        CurrentService.TryGetController<WagonController>(out _currentWagon);
        _currentWagon.BoxCloseEvent += CheckCombo;
    }

    private void OnDisable()
    {
        _currentWagon.BoxCloseEvent -= CheckCombo;
    }

    private void CheckCombo(int combo)
    {
        switch (combo)
        {
            case (1):
                AddPoints(_oneComboPoints);
                break;

            case (2):
                AddPoints(_doubleComboPoints);
                break;

            default:
                break;
        }   

        PonintsChangedEvent?.Invoke(Points);
    }

    private void AddPoints(int points) => Points += points;
    private void ApllyPenalty(int penalty) => Points -= penalty;
}
