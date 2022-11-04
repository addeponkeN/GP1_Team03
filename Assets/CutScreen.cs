using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScreen : MonoBehaviour
{
    public void LetsGo()
    {
        SceneManager.LoadScene("MainLevel 1");
    }
}
