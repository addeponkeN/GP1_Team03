using SF = UnityEngine.SerializeField;
using UnityEngine;
using Jellybeans.Levels;

namespace Jellybeans.Scenes
{
    [AddComponentMenu("Managers/Scenes Initialiser", 0)]
    public class ScenesInitialiser : MonoBehaviour
	{
		[SF] private ScenesManager _manager = null;
        [SF] private LevelManager _levels = null;
        private static ScenesInitialiser s_active = null;

        private void Awake(){
			if (s_active == null){
				_manager.Initialise();
				s_active = this;

                var level = _levels.SelectedLevel;
                if (!_manager.IsLoaded(level.BuildIndex))
                {
                    level = _levels.MenuBackdrop;
                    if (!_manager.IsLoaded(level.BuildIndex))
                        _manager.LoadScene(level.BuildIndex, true);
                }
            } else Destroy(this);
        }

		private void OnDestroy(){
			if (s_active != this) return;
            _manager.OnDestroy();
        }
	}
}