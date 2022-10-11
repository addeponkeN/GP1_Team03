using System.Collections.Generic;
using System.Linq;
using Util;

namespace PlayerControllers
{
    public class ControllerSet : BasePlayerController
    {
        /// <summary>
        /// Disables all controllers in the set
        /// </summary>
        public virtual bool ControllersEnabled
        {
            get => _controllersEnabled;
            set
            {
                _controllersEnabled = value;
                for(int i = 0; i < Controllers.Count; i++)
                    Controllers[i].SetEnabled(_controllersEnabled);
            }
        }

        public List<BasePlayerController> Controllers;

        private bool _controllersEnabled = false;

        public ControllerSet()
        {
            Controllers = new List<BasePlayerController>();
        }

        /// <summary>
        /// Adds a controller to the controller set
        /// </summary>
        /// <param name="controller"></param>
        public void AddController(BasePlayerController controller)
        {
            controller.IsAlive = true;
            controller.Manager = Manager;
            controller.Init();
            Controllers.Add(controller);
        }

        /// <summary>
        /// Get a controller from the set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetController<T>() where T : BasePlayerController
            => Controllers.FirstFast(x => x is T) as T;

        public override void Init()
        {
            for(int i = 0; i < Controllers.Count; i++)
            {
                Controllers[i].Init();
            }
        }

        public override void Update(float delta)
        {
            for(int i = 0; i < Controllers.Count; i++)
            {
                Controllers[i].Update(delta);
            }
        }

        public override void FixedUpdate()
        {
            for(int i = 0; i < Controllers.Count; i++)
            {
                Controllers[i].FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            for(int i = 0; i < Controllers.Count; i++)
            {
                if(!Controllers[i].IsAlive)
                    Controllers.RemoveAt(i--);
            }
        }
    }
}