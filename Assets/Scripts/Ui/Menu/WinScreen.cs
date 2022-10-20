using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    
    public static int score;

    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        scoreText.text = $"YOUR SCORE IS : {score} FOLLOWERS";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainLevel");

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("START");
    }
}
