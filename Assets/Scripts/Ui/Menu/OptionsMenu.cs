using UnityEngine.EventSystems;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public EventSystem eventSys;
    public GameObject firstSelectedOpBack;

    private void OnEnable(){
        eventSys.SetSelectedGameObject(firstSelectedOpBack);
        //Debug.Log("hello");
    }
}