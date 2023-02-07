using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    private Button _button;
    private TextMeshProUGUI _label;
    private int _index;

    public event UnityAction<int> BtnClickEvent;

    public void Initialize(int index)
    {
        _button = GetComponent<Button>();
        _label = _button.GetComponentInChildren<TextMeshProUGUI>();

        _index = index;
        _label.text = (index + 1).ToString();

       
    }

    public void OnButtonClick()
    {
        BtnClickEvent?.Invoke(_index);
    }    
}