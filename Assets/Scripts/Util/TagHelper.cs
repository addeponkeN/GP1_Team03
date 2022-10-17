namespace Util
{
    public static class TagHelper
    {
        private const string TAG_PLAYER = "Player";
        
        public static bool IsTagPlayer(string tag)
        {
            return TAG_PLAYER.Equals(tag);
        }
    }
}