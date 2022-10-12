namespace PlayerControllers
{
    public class PlayerControllerManager : ControllerSet
    {
        /// <summary>
        /// The player root GameObject
        /// </summary>
        public Player Player;

        /// <summary>
        /// Getter of the player's InputContainer
        /// </summary>
        public InputContainer Input => Player.Input;

        public PlayerControllerManager(Player pl)
        {
            Player = pl;
            Manager = this;
        }

    }
}