namespace PlayerControllers
{
    public class PlayerControllerManager : ControllerSet
    {
        /// <summary>
        /// The player root GameObject
        /// </summary>
        public Player Player; 

        public PlayerControllerManager(Player pl)
        {
            Player = pl;
            Manager = this;
        }

    }
}