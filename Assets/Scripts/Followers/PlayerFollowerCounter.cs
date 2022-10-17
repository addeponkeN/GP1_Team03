using SF = UnityEngine.SerializeField;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerFollowers))]
public class PlayerFollowerCounter : MonoBehaviour
{
    [SF] private int _levelUpCount = 20;
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
        UpdatedFollowers();
    }

    /// <summary>
    /// Update the follow counter
    /// </summary>
    public void UpdatedFollowers(){
        if (_followers.Count >= _levelUpCount){
            _followers.Clear();
            _level++;
        }

        var min = (_level * _levelUpCount);
        var current = (_followers.Count - _followers.KeeptOnLevelUp);
        _followerCount = min + current;

        _hudText.text = _followerCount.ToString();
    }
}