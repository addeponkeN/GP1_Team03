using System;
using Jellybeans.Updates;
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
    public FileLoader<LeaderboardFile> Leaderboard;
    public FileLoader<GameSettingsFile> GameSettings;

    [SerializeField] private UpdateManager _updateManager;

    [Header("Game Winning Triggers")] [SerializeField]
    private UnityEvent _onGameVictory;

    [Header("Game Losing Triggers")] [SerializeField]
    private UnityEvent _onGameLost;

    [SerializeField] private PlayerManager _playerManager;

    private Util.Timer _gameTimer;
    
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
        _gameTimer = new(Rules.Time);
        _updateManager.Subscribe(_gameTimer.Update, UpdateType.Update);
    }

    private void OnDestroy()
    {
        _updateManager.Unsubscribe(_gameTimer.Update, UpdateType.Update);
    }
    
}