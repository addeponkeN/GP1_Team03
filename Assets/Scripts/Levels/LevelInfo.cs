using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Jellybeans.Levels
{
    [CreateAssetMenu(fileName = "Level Info",
     menuName = "Jellybeans/Settings/Levels", order = -1)]
    public class LevelInfo : ScriptableObject
    {
        [SF] private string _name = "Level 1";
        [SF] private Texture2D _thumbnail = null;
        [SF] private int _buildIndex = 0;
        [Space]
        [SF] private int _maxPlayerCount = 4;

// PROPERTIES

        public string Name => _name;
        public Texture2D Thumbnail => _thumbnail;
        public int BuildIndex => _buildIndex;

        public int MaxPlayerCount => _maxPlayerCount;
    }
}