using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUIView : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _coinsLabel;
    [SerializeField] private TextMeshProUGUI _lifesLabel;

    public void IntitializeHUD()
    {
        _coinsLabel.text = "0";
    }

    public void UpdateCoins(int points)
    {
        _coinsLabel.text = points.ToString();
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
}
