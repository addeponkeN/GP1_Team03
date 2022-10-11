namespace PlayerControllers
{
    public abstract class BasePlayerController
    {
        public bool IsAlive { get; set; } = true;
        public PlayerControllerManager Manager { get; set; }

        public virtual void Init() { }
    
        public virtual void Update(float delta) { }

        public virtual void FixedUpdate() { }

        public virtual void OnEnabled(bool enabled) { }

        public virtual void Exit() { }
    }
}
