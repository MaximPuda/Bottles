using UnityEngine;

public class GamePlayService : Service
{
    [SerializeField] private LevelController _level;
    [SerializeField] private ScoreController _score;

    public LevelController LevelCTRL => _level;
    public ScoreController ScoreCTRL => _score;

    protected override void InitAllControllers()
    {
        _level.Initialize(this);
        _score.Initialize(this);

        _level.OnStart();
        _score.OnStart();
    }

    protected override void OnWinEnter()
    {
        base.OnWinEnter();

        ScoreCTRL.SendTotal();
        LevelCTRL.CurrentLevel.Grid.ClearGrid();
    }

    protected override void OnLoseEnter()
    {
        base.OnLoseEnter();

        //LevelCTRL.CurrentLevel.Grid.ClearGrid();
    }

    protected override void OnMenuEnter()
    {
        base.OnMenuEnter();

        LevelCTRL.CurrentLevel.Grid.ClearGrid();
    }
}