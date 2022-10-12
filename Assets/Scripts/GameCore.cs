using UnityEngine;

public class GameCore : MonoBehaviour
{
    public static GameCore Get { get; private set; }

    public Player Player;

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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if(Player == null)
        {
            Player = FindObjectOfType<Player>();
            Debug.LogWarning("Player has automatically been assigned in 'GameCore' (temporarily) \n" +
                             $"Please assign 'Player' to the 'GameCore' script (in the '{gameObject.name}' gameobject)");
        }
    }
}