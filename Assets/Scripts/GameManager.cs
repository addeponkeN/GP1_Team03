using Jellybeans.Updates;
using Settings;
using Settings.GameSettings;
using Settings.Leaderboard;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A default ruleset as backup
    /// </summary>
    private static GameRules _defaultRules;
    
    public GameRules Rules;

    [SerializeField] private UpdateManager _updateManager;
    
    [Header("Game Winning Triggers")] 
    [SerializeField] private UnityEvent _onGameVictory;

    [Header("Game Losing Triggers")] 
    [SerializeField] private UnityEvent _onGameLost;
    
    private Player _player;

    public FileLoader<LeaderboardFile> Leaderboard;
    public FileLoader<GameSettingsFile> GameSettings;

    private void Awake()
    {
        _defaultRules = ScriptableObject.CreateInstance<GameRules>();
        if(Rules == null)
            Rules = _defaultRules;

        Leaderboard = new(PathHelper.LeaderboardFilename, PathHelper.ExternalDataPath);
        GameSettings = new(PathHelper.GameSettingsFilename, PathHelper.ExternalDataPath);
    }

    private void Start()
    {
        _player = GameCore.Get.Player;
        if(_player == null) return;
    }

    public void OnFollowerAdded()
    {
        if(_player == null) return;
        if(CheckWinCondition())
        {
            _onGameVictory?.Invoke();
        }
    }

    public void OnFollowerLost()
    {
        if(_player == null) return;
        var followers = _player.GetComponent<PlayerFollowers>();

        if(CheckLoseCondition())
        {
            _onGameLost?.Invoke();
        }
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