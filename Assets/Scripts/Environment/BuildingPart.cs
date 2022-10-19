using SF = UnityEngine.SerializeField;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BuildingPart : MonoBehaviour
{
    [SF] private int _primaryIndex   = 0;
    [SF] private int _secondaryIndex = 1;
    [SF] private int _tertiaryIndex  = -1;

    /// <summary>
    /// Assigns the colours to the building part
    /// </summary>
    public void AssignColours(Color primary, Color secondary, Color tertiary){
        var renderer = GetComponent<MeshRenderer>();
        
        if (_primaryIndex > -1 && _primaryIndex < renderer.materials.Length)
            renderer.materials[_primaryIndex].color = primary;

        if (_secondaryIndex > -1 && _secondaryIndex < renderer.materials.Length)
            renderer.materials[_secondaryIndex].color = secondary;

        if (_tertiaryIndex > -1 && _tertiaryIndex < renderer.materials.Length)
            renderer.materials[_tertiaryIndex].color = tertiary;
    }
}