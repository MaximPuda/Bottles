using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsLabel;
    [SerializeField] private TextMeshProUGUI _bottlesAmount;

    [SerializeField] private Animator _animator;

    public void IntitializeHUD()
    {
        _pointsLabel.text = "0";
    }

    public void UpdatePoints(int points)
    {
        _pointsLabel.text = points.ToString();
    }

    public void UpdateMoves(int amount)
    {
        _bottlesAmount.text = amount.ToString();
    }

    public void ShowGameOverScreen(bool show)
    {
        _animator.SetBool("GameOver", show);
    }
}
