using SF = UnityEngine.SerializeField;
using UnityEngine;

[CreateAssetMenu(menuName = "BikeMania/Building Colours")]
public class BuildingColours : ScriptableObject
{
    [SF] private Color[] _primary   = null;
    [SF] private Color[] _secondary = null;
    [SF] private Color[] _tertiary  = null;

// PROPERTIES

    public Color[] Primary => _primary;
    public Color[] Secondary => _secondary;
    public Color[] Tertiary => _tertiary;
}