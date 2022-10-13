using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameRules _defaultRules;

    public GameRules Rules;

    private Player _player;
    private PlayerTimer _playerTimer;

    private void Awake()
    {
        _defaultRules = ScriptableObject.CreateInstance<GameRules>();
        if(Rules == null)
            Rules = _defaultRules;
    }

    private void Start()
    {
        _player = GameCore.Get.Player;
        if(_player == null) return;
    }

    public void OnFollowerAdded()
    {
        if(_player == null) return;
        CheckWinCondition();
    }

    public void OnFollowerLost()
    {
        if(_player == null) return;
        var followers = _player.GetComponent<PlayerFollowers>();
    }

    private bool CheckLoseCondition()
    {
        return false;
    }
    
    private bool CheckWinCondition()
    {
        var followers = _player.GetComponent<PlayerFollowers>();
        return followers.Count >= Rules.FollowerWinCount;
    }
}