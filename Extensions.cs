using System;
using static Collisions.GameMath;

namespace Collisions
{
    public static class Extensions
    {
        const float FloatEqualityThreshold = 1f / 8192f;

        public static bool Equal(this float a, float b)
        {
            return Math.Abs(a - b) < FloatEqualityThreshold;
        }

        public static float OrGreater(this float x, float f)
        {
            return f > x ? f : x;
        }

        public static float OrLesser(this float x, float f)
        {
            return f < x ? f : x;
        }

        public static float ToRadians(this float degrees)
        {
            return PI * degrees / 180;
        }

        public static float ToDegrees(this float radians)
        {
            return radians * 180 / PI;
        }

        public static float ClampTo(this float x, Range range)
        {
            return x.ClampTo(range.Minimum, range.Maximum);
        }

        public static float ClampTo(this float x, float rangeMin, float rangeMax)
        {
            if (x < rangeMin) return rangeMin;
            if (rangeMax < x) return rangeMax;
            return x;
        }
    }
}