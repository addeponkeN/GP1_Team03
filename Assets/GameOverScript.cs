using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScript : MonoBehaviour
{


    public TMP_Text pointsText; 
    
    
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " FOLLOWERS";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void MenuButton()
    {

    }
}
