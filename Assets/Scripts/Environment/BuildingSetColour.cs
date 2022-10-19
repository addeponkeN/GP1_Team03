using SF = UnityEngine.SerializeField;
using UnityEngine;

public class BuildingSetColour : MonoBehaviour
{
    [SF] private BuildingColours _colours = null;

    /// <summary>
    /// Assigns random colour to the building
    /// </summary>
    private void Awake(){
        var primaries = _colours.Primary;
        var secondaries = _colours.Secondary;
        var tertiaries = _colours.Tertiary;

        var primary = primaries[Random.Range(0, primaries.Length)];
        var secondary = secondaries[Random.Range(0, secondaries.Length)];
        var tertiary = tertiaries[Random.Range(0, tertiaries.Length)];

        var parts = GetComponentsInChildren<BuildingPart>();
        for (int i = 0; i < parts.Length; i++){
            parts[i].AssignColours(primary, secondary, tertiary);
        }
    }
}