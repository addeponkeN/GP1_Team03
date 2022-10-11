using SF = UnityEngine.SerializeField;
using Action = System.Action;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Jellybeans.Updates;

namespace Jellybeans.Scenes
{
    [CreateAssetMenu(fileName = "Scenes Manager",
     menuName = "Jellybeans/Managers/Scenes", order = -2)]
    public sealed class ScenesManager : ScriptableObject
	{
        [SF] private UpdateManager _update = null;

        private float _progress = 0f;
        private Subscription<Action> _onLoaded = null;
        private Subscription<Action> _onUnloaded = null;
        private Subscription<Action> _onLoading = null;
        private Subscription<Action> _onUnloading = null;

// PROPERTIES

        public float Progress => _progress;

// INITIALISATION AND FINALISATION

        /// <summary>
        /// Initialises the scenes manager
        /// </summary>
		public void Initialise(){
            _onLoaded    = new Subscription<Action>();
            _onUnloaded  = new Subscription<Action>();
            _onLoading   = new Subscription<Action>();
            _onUnloading = new Subscription<Action>();
            
            _progress = 0f;
        }

        /// <summary>
        /// Finalises the scenes manager
        /// </summary>
        public void OnDestroy(){
            _onLoaded.ClearSubscribers();
            _onUnloaded.ClearSubscribers();
            _onLoading.ClearSubscribers();
            _onUnloading.ClearSubscribers();
        }

// SCENE DATA

        /// <summary>
        /// Returns true if the scene has been loaded
        /// </summary>
        /// <param name="index">Scene build index</param>
        public bool IsLoaded(int index){
            var scene = SceneManager.GetSceneByBuildIndex(index);
            return scene.IsValid();
        }

        /// <summary>
        /// Returns true if the scene is the active level
        /// </summary>
        /// <param name="index">Scene build index</param>
        public bool IsActive(int index){
            var active = SceneManager.GetActiveScene();
            return active.buildIndex == index;
        }

// SCENE MANAGEMENT

        /// <summary>
        /// Changes the active scene
        /// </summary>
        /// <param name="index">Scene build index</param>
        public void SetActive(int index){
            var scene = SceneManager.GetSceneByBuildIndex(index);
            if (!scene.IsValid()) return;

            SceneManager.SetActiveScene(scene);
        }

// SCENE LOADING

        /// <summary>
        /// Loads the scene asyncronesly in additive mode
        /// </summary>
        /// <param name="index">Scene build index</param>
        public void LoadScene(int index, bool activate = false){
            _update.StartCoroutine(
                OnLoadingScene(index, activate)
            );
        }

        /// <summary>
        /// Loads the scene asyncronesly in additive mode
        /// </summary>
        private IEnumerator OnLoadingScene(int scene, bool activate){
            var async = SceneManager.LoadSceneAsync(
                scene, LoadSceneMode.Additive
            );
  
            while (!async.isDone){
                if (async.progress != _progress){
                    _progress = async.progress / 0.9f;
                    _onLoading.NotifySubscribers();
                }
                yield return null;
            }

            yield return null;
            if (activate) SetActive(scene);

            _onLoaded.NotifySubscribers();
        }

// SCENE UNLOADING

        /// <summary>
        /// Unloads the scene asyncronesly
        /// </summary>
        /// <param name="index">Scene build index</param>
        public void UnloadScene(int index){
            _update.StartCoroutine(
                OnUnloadingScene(index)
            );
        }

        /// <summary>
        /// Unloads the scene asyncronesly
        /// </summary>
        private IEnumerator OnUnloadingScene(int index){
            var async = SceneManager.UnloadSceneAsync(index);

            while (!async.isDone){
                _progress = async.progress;
                _onUnloading.NotifySubscribers();
                yield return null;
            }

            _onUnloaded.NotifySubscribers();
        }

// SUBSCRIPTIONS

        /// <summary>
        /// Subscribes to the on scene loaded event
        /// </summary>
        public void SubscribeOnLoaded(Action subscriber) 
            => _onLoaded.AddSubscriber(subscriber);

        /// <summary>
        /// Unsubscribes from the on scene loaded event
        /// </summary>
        public void UnsubscribeOnLoaded(Action subscriber) 
            => _onLoaded.RemoveSubscriber(subscriber);

        /// <summary>
        /// Subscribes to the on scene loading event
        /// </summary>
        public void SubscribeOnLoading(Action subscriber) 
            => _onLoading.AddSubscriber(subscriber);

        /// <summary>
        /// Unsubscribes from the on scene loading event
        /// </summary>
        public void UnsubscribeOnLoading(Action subscriber) 
            => _onLoading.RemoveSubscriber(subscriber);


        /// <summary>
        /// Subscribes to the on scene unloaded event
        /// </summary>
        public void SubscribeOnUnloaded(Action subscriber)
            => _onUnloaded.AddSubscriber(subscriber);

        /// <summary>
        /// Unsubscribes from the on scene unloaded event
        /// </summary>
        public void UnsubscribeOnUnloaded(Action subscriber)
            => _onUnloaded.RemoveSubscriber(subscriber);

        /// <summary>
        /// Subscribes to the on scene unloading event
        /// </summary>
        public void SubscribeOnUnloading(Action subscriber) 
            => _onUnloading.AddSubscriber(subscriber);

        /// <summary>
        /// Unsubscribes from the on scene unloading event
        /// </summary>
        public void UnsubscribeOnUnloading(Action subscriber) 
            => _onUnloading.RemoveSubscriber(subscriber);
    }
}