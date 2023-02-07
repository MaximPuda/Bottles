using UnityEngine;

[CreateAssetMenu]
public class ItemColor : ScriptableObject
{
    [SerializeField] private ColorNames _name;
    [SerializeField] private Color _color;
    [SerializeField] [Range(0,100)] private int _chance = 100;
 
    public ColorNames Name => _name;
    public Color Color => _color;
    public int Chance => _chance;
    public int Weight { get; set; }

}
