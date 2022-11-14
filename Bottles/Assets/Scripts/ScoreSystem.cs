using UnityEngine;
using UnityEngine.Events;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private int _oneComboPoints;
    [SerializeField] private int _doubleComboPoints;
    [SerializeField] private int _penalty;

    public int Points { get; private set; }

    public event UnityAction<int> OnPonintsChanged;

    private void OnEnable()
    {
        GlobalEvents.OnBottleCombo += CheckCombo;
    }

    private void OnDisable()
    {
        GlobalEvents.OnBottleCombo -= CheckCombo;
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
                ApllyPenalty(_penalty);
                break;
        }   

        OnPonintsChanged?.Invoke(Points);
    }

    private void AddPoints(int points) => Points += points;
    private void ApllyPenalty(int penalty) => Points -= penalty;
}
