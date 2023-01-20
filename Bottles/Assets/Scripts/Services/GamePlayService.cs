using UnityEngine;

public class GamePlayService : Service
{
    [SerializeField] private LevelController _level;
    [SerializeField] private GridController _grid; 
    [SerializeField] private ScoreController _score;
    [SerializeField] private WagonController _wagon;
    [SerializeField] private TransporterController _transporter;

    public LevelController LevelCTRL => _level;
    public ScoreController ScoreCTRL => _score;
    public WagonController WagonCTRL => _wagon;
    public TransporterController TransporterCTRL => _transporter;

    protected override void InitAllControllers()
    {
        _level.Initialize(this);
        _grid.Initialize(this);
        _score.Initialize(this);
        _wagon.Initialize(this);
        _transporter.Initialize(this);

        _level.OnStart();
        _grid.OnStart();
        _score.OnStart();
        _wagon.OnStart();
        _transporter.OnStart();
    }

    protected override void OnLoseEnter()
    {
        base.OnLoseEnter();
        _grid.ClearGrid();
    }
}