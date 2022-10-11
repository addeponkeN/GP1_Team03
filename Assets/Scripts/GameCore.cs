using UnityEngine;

public class GameCore : MonoBehaviour
{
    public static GameCore Get { get; private set; }

    private void Awake()
    {
        if(Get != null)
        {
            Destroy(this);
            return;
        }

        Get = this;
        DontDestroyOnLoad(gameObject);
    }
}