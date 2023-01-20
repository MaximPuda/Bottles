using UnityEngine;

[System.Serializable]
public class ItemType 
{
    [SerializeField] private TypeNames _type;
    [SerializeField] [Range(0, 100)] private int _chance = 100;

    public TypeNames Type => _type;
    public int Chance => _chance;
    public int Weight { get; set; }
}
