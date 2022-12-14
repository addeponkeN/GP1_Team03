using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{

    public EventSystem eventSys;
    public GameObject firstSelectedOpBack;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Goodbye");
        Application.Quit();
    }

    private void OnEnable()
    {
        eventSys.SetSelectedGameObject(firstSelectedOpBack);
    }

}
