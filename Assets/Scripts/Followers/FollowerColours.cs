using SF = UnityEngine.SerializeField;
using UnityEngine;

[CreateAssetMenu(menuName = "BikeMania/Follower Colours")]
public class FollowerColours : ScriptableObject
{
    [SF] private Color[] _skin = null;
    [SF] private Color[] _shirt = null;
    [SF] private Color[] _pants = null;
    [SF] private Color[] _bike = null;

// PROPERTIES

    public Color[] Skin => _skin;
    public Color[] Shirt => _shirt;
    public Color[] Pants => _pants;
    public Color[] Bike => _bike;
}