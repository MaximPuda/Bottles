using UnityEngine;
using TMPro;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsLabel;
    [SerializeField] private Animation _alarmAnimation;

    public void IntitializeHUD()
    {
        _pointsLabel.text = "0";
    }

    public void UpdatePoints(int points)
    {
        _pointsLabel.text = points.ToString();
    }

    public void Alarm(bool active)
    {
        if (active)
            _alarmAnimation.Play();
        else _alarmAnimation.Stop();
    }
}
