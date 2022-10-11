using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Jellybeans.Levels
{
    [CreateAssetMenu(fileName = "Level Manager",
     menuName = "Managers/Levels", order = -2)]
    public class LevelManager : ScriptableObject
    {
        [SF] private LevelInfo _menuLevel = null;
        [SF] private LevelInfo _menuBackdrop = null;
        [SF] private LevelInfo[] _levels = null;

        private int _selected = 0;

// PROPERTIES

        public LevelInfo MenuLevel => _menuLevel;
        public LevelInfo MenuBackdrop => _menuBackdrop;
        public LevelInfo SelectedLevel => _levels[_selected];

// INITIALISATION AND FINALISATION

        /// <summary>
        /// Initialises the level manager
        /// </summary>
        public void Initialise(){
            
        }

        /// <summary>
        /// Finalises the level manager
        /// </summary>
        public void OnDestroy(){
            _selected = 0;
        }

// LEVEL MANAGEMENT

        /// <summary>
        /// Changes the currently selected level
        /// </summary>
        /// <param name="index">Level array index</param>
        public void SetSelectedLevel(int index){
            if (index < 0 || 
                index >= _levels.Length) 
                return;
            
            _selected = index;
        }
    }
}