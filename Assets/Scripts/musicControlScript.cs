using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicControlScript : MonoBehaviour
{
    public static musicControlScript instance;

    private void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }  
}
