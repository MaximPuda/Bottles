using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _pointsLabel;
    [SerializeField] private TextMeshProUGUI _movesLabel;
    [SerializeField] private TextMeshProUGUI _lifesLabel;

    [Header("WinScreen")]
    [SerializeField] private TextMeshProUGUI _resultPoints;

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
        _movesLabel.text = amount.ToString();
    }

    public void UpdateLifes(int amount, int max, int seconds)
    {
        if (seconds <= 0)
            _lifesLabel.text = amount.ToString() + "/" + max.ToString();
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            string left = time.ToString("mm':'ss");
            _lifesLabel.text = amount.ToString() + "/" + max.ToString() + "\n" + left;
        }
    }

    public void ShowLoseScreen(bool show)
    {
        _animator.SetBool("Lose", show);
    }

    public void ShowWinScreen(bool show, int score)
    {
        _animator.SetBool("Win", show);
        _resultPoints.text = "x" + score.ToString();
    }
}
