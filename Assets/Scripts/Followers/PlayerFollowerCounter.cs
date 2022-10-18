using SF = UnityEngine.SerializeField;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerFollowers))]
public class PlayerFollowerCounter : MonoBehaviour
{
    [SF] private GameRules _gameRules = null;
    [SF] private TMP_Text _hudText = null;

    private int _level = 0;
    private int _followerCount = 0;
    private PlayerFollowers _followers = null;

// PROPERTIES

    public int Level => _level;
    public int FollowerCount => _followerCount;

// INITIALISATION

    /// <summary>
    /// Initialises the follow counter
    /// </summary>
    private void Awake(){
        _followers = gameObject.GetComponent<PlayerFollowers>();
        _followerCount = _followers.Count;
        UpdateText();
    }

    /// <summary>
    /// Update the follow counter
    /// </summary>
    public void UpdatedFollowers(){
        if (_followers.Count >= _gameRules.LevelUpCount){
            _followers.Clear();
            _level++;
        }

        var min = (_level * _gameRules.LevelUpCount);
        var current = (_followers.Count - _followers.KeeptOnLevelUp);
        _followerCount = min + current;

        UpdateText();
    }

    /// <summary>
    /// Updates the hud follow counter
    /// </summary>
    private void UpdateText(){
        if (_hudText == null) return;
        _hudText.text = _followerCount.ToString();
    }
}