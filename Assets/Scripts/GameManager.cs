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

    public Util.Timer GameTimer;
    
    public GameRules Rules;
    public FileLoader<LeaderboardFile> Leaderboard;
    public FileLoader<GameSettingsFile> GameSettings;

    [SerializeField] private UpdateManager _updateManager;
    [SerializeField] private PlayerManager _playerManager;

    [Header("Game Winning Triggers")] [SerializeField]
    private UnityEvent _onGameVictory;

    [Header("Game Losing Triggers")] [SerializeField]
    private UnityEvent _onGameLost;
    
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
        GameTimer = new(Rules.Time, 1f);
        _updateManager.Subscribe(GameTimer.Update, UpdateType.Update);
    }

    private void OnDestroy()
    {
        _updateManager.Unsubscribe(GameTimer.Update, UpdateType.Update);
    }
    
}