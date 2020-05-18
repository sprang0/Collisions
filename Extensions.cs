using static Collisions.GameMath;

namespace Collisions
{
    public static class Extensions
    {
        public static float OrGreater(this float x, float y)
        {
            return y > x ? y : x;
        }

        public static float OrLesser(this float x, float y)
        {
            return y < x ? y : x;
        }

        public static float ToRadians(this float degrees)
        {
            return PI * degrees / 180;
        }

        public static float ToDegrees(this float radians)
        {
            return radians * 180 / PI;
        }

        public static float Clamp(this float x, Range range)
        {
            return x.Clamp(range.Minimum, range.Maximum);
        }

        public static float Clamp(this float x, float rangeMin, float rangeMax)
        {
            if (x < rangeMin) return rangeMin;
            if (rangeMax < x) return rangeMax;
            return x;
        }
    }
}