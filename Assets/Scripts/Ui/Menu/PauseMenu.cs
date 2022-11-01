using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

//Author Keziah Ferreira Dos Santos

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject GameHub;

    [SerializeField] InputActionReference back;



    // Update is called once per frame

    private void Awake()
    {
        back.action.Enable();
        back.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameHub.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameHub.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("START"); 
    }

    public void QuitGame()
    {
        Debug.Log("QUIT....");
        Application.Quit();
    }

}
