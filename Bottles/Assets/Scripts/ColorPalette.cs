using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Add new color", order = 2)]
public class ColorPalette: ScriptableObject
{
    public ColorsName ColorName;
    public Color Color;
    public float Weight;
}
