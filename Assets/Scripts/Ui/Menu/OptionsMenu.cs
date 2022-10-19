using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public EventSystem eventSys;
    public GameObject firstSelectedOpBack;

    private void OnEnable()
    {
        eventSys.SetSelectedGameObject(firstSelectedOpBack);
    }

}
