using SF = UnityEngine.SerializeField;
using Action = System.Action<float>;
using UnityEngine;

using System.Collections;

namespace Jellybeans.Updates
{
    [CreateAssetMenu(fileName = "Update Manager", 
     menuName = "Managers/Update Loops", order = 0)]
    public sealed class UpdateManager : ScriptableObject
    {
        private bool _paused = false;
        private UpdateRelay _relay = null;
        private Subscription<Action> _update      = new();
        private Subscription<Action> _fixedUpdate = new();
        private Subscription<Action> _lateUpdate  = new();

// PROPERTIES

        public bool Paused => _paused;

// INITIALISATION AND FINALISATION

        /// <summary>
        /// Initialises the update manager
        /// </summary>
        public void Initialise(UpdateRelay relay){
            //_input.AddOnMenuToggleInput(OnMenuToggleEvent);
            _relay = relay;
        }

        /// <summary>
        /// Finalises the update manager
        /// </summary>
        public void OnDestroy(){
            //_input.RemoveOnMenuToggleInput(OnMenuToggleEvent);
            _update.ClearSubscribers();
            _fixedUpdate.ClearSubscribers();
            _lateUpdate.ClearSubscribers();
        }

// UPDATE MANAGEMENT

        /// <summary>
        /// Notifys subscribers on regular update
        /// </summary>
        public void OnUpdate(float deltaTime){
            if (!Paused) _update.NotifySubscribers(deltaTime);
        }

        /// <summary>
        /// Notifys subscribers on fixed update
        /// </summary>
        public void OnFixedUpdate(float fixedDeltaTime){
            if (!Paused) _fixedUpdate.NotifySubscribers(fixedDeltaTime);
        }

        /// <summary>
        /// Notifys subscribers on late update
        /// </summary>
        public void OnLateUpdate(float deltaTime){
            if (!Paused) _lateUpdate.NotifySubscribers(deltaTime);
        }

        /// <summary>
        /// Toggles pause on game input callback
        /// </summary>
        /// <param name="paused"></param>
        private void OnMenuToggleEvent(bool paused){
            _paused = paused;
        }

// COROUTINE MANAGEMENT

        /// <summary>
        /// Stops the specified coroutine
        /// </summary>
        public void StopCoroutine(Coroutine routine){
            if (routine == null) return;
            _relay.StopCoroutine(routine);
        }

        /// <summary>
        /// Starts a new coroutine
        /// </summary>
        public Coroutine StartCoroutine(IEnumerator routine){
            // Useful for running a coroutine from a scriptable object
            return _relay.StartCoroutine(routine);
        }

// SUBSCRIPTIONS

        /// <summary>
        /// Adds the subscriber to the update manager
        /// </summary>
        public void Subscribe(Action subscriber, UpdateType type){
            if (type == UpdateType.Update){
                _update.AddSubscriber(subscriber);

            } else if(type == UpdateType.FixedUpdate){
                _fixedUpdate.AddSubscriber(subscriber);

            } else if(type == UpdateType.LateUpdate){
                _lateUpdate.AddSubscriber(subscriber);

            } else MissingType(type);
        }

        /// <summary>
        /// Removes the subscriber from the update manager
        /// </summary>
        public void Unsubscribe(Action subscriber, UpdateType type){
            if (type == UpdateType.Update){
                _update.RemoveSubscriber(subscriber);

            } else if(type == UpdateType.FixedUpdate){
                _fixedUpdate.RemoveSubscriber(subscriber);

            } else if(type == UpdateType.LateUpdate){
                _lateUpdate.RemoveSubscriber(subscriber);

            } else MissingType(type);
        }

        /// <summary>
        /// Logs a missing update type error to the console
        /// </summary>
        private void MissingType(UpdateType type){
            Debug.LogError(new System.MissingFieldException(
                $"Missing implementation UpdateType.{type} in Update Manager"
            ));
        }
    }
}