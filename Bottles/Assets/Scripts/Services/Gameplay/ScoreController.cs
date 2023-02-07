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
    private PlayerData _data;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _currentWagon = ((GamePlayService)CurrentService).WagonCTRL;
        _currentWagon.BoxCloseEvent += CheckCombo;

        ServiceManager.TryGetService<PlayerService>(out PlayerService playerService);
        _data = playerService.PlayerDataCTRL;
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

    public void SendTotal()
    {
        _data.Coins += Points;
    }
}
