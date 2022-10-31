using UnityEngine.EventSystems;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject firstSelectedOpBack;

    private void OnEnable(){
        var current = EventSystem.current;
        current.SetSelectedGameObject(null);
        current.SetSelectedGameObject(firstSelectedOpBack);
    }
}