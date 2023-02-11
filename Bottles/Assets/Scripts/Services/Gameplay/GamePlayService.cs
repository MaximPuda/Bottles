using UnityEngine;

public class GamePlayService : Service
{
    [SerializeField] private LevelController _level;
    [SerializeField] private GridController _grid; 
    [SerializeField] private ScoreController _score;
    [SerializeField] private WagonController _wagon;

    public LevelController LevelCTRL => _level;
    public GridController GridCTRL => _grid;
    public ScoreController ScoreCTRL => _score;
    public WagonController WagonCTRL => _wagon;

    protected override void InitAllControllers()
    {
        _level.Initialize(this);
        _grid.Initialize(this);
        _score.Initialize(this);
        _wagon.Initialize(this);
        _wagon.WagonInEvent += OnWagonIn;

        _level.OnStart();
        _grid.OnStart();
        _score.OnStart();
        _wagon.OnStart();
    }
    protected override void OnWinEnter()
    {
        base.OnWinEnter();

        _score.SendTotal();
    }

    protected override void OnLoseEnter()
    {
        base.OnLoseEnter();
    }

    protected override void OnMenuEnter()
    {
        base.OnMenuEnter();

        _grid.ClearGrid();
    }

    private void OnWagonIn()
    {
        GridCTRL.ShowItems();
    }
}