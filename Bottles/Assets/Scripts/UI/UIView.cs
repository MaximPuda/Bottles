using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsLabel;
    [SerializeField] private Animation _alarmAnimation;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Animator _animator;

    public void IntitializeHUD()
    {
        _pointsLabel.text = "0";
    }

    public void UpdatePoints(int points)
    {
        _pointsLabel.text = points.ToString();
    }

    public void DesableOneHP()
    {
        foreach (var heart in _hearts)
        {
            if (heart.enabled)
            {
                heart.enabled = false;
                return;
            }
        }
    }

    public void Alarm(bool active)
    {
        if (active)
            _alarmAnimation.Play();
        else _alarmAnimation.Stop();
    }

    public void ShowGameOverScreen(bool show)
    {
        _animator.SetBool("GameOver", show);
    }
}
