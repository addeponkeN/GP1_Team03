using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("MainLevel");

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("START");
    }
}
