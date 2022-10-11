namespace Util
{
    public static class OmegaMathf
    {
        public static float Normalize(float value)
            => value == 0f ? 0f : value > 0f ? 1f : -1f;
    }
}