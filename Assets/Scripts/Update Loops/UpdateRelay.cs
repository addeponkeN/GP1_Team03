using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Jellybeans.Updates 
{
    [AddComponentMenu("Jellybeans/Managers/Update Relay", 0), DefaultExecutionOrder(-1)]
    public sealed class UpdateRelay : MonoBehaviour
    {
        [SF] private UpdateManager _manager = null;
        private static UpdateRelay s_active = null;

        private void Awake(){
            if (s_active == null){
                _manager.Initialise(this);
                s_active = this;

            } else Destroy(this);
        }

        private void OnDestroy(){
            if (s_active != this) return;
            _manager.OnDestroy();
        }

        private void Update() 
            => _manager.OnUpdate(Time.deltaTime);

        private void FixedUpdate() 
            => _manager.OnFixedUpdate(Time.fixedDeltaTime);

        private void LateUpdate() 
            => _manager.OnLateUpdate(Time.deltaTime);
    }
}