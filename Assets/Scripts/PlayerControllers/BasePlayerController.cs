namespace PlayerControllers
{
    public abstract class BasePlayerController
    {
        public bool IsAlive { get; set; } = true;
        public bool Enabled { get; set; } = true;
        public PlayerControllerManager Manager { get; set; }

        /// <summary>
        /// This is called when a controller is added to a control set
        /// </summary>
        public virtual void Init() { }
    
        public virtual void Update(float delta) { }

        public virtual void FixedUpdate(float fixedDelta) { }

        public virtual void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// This is called when a controller is removed from a control set
        /// </summary>
        public virtual void Exit() { }
    }
}
