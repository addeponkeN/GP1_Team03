namespace Util
{
    public struct IndexLooper
    {
        public static implicit operator IndexLooper(int length)
            => new(length);

        public static implicit operator int(IndexLooper looper)
            => looper.Current;

        public int Current;
        public int Length;

        public IndexLooper(int length)
        {
            Length = length;
            Current = 0;
        }
        
        public int Next()
            => Current = (Current + 1) % Length;

        public int Previous()
            => --Current < 0 ? Current = Length - 1 : Current;
    }
}