using System;
using static System.Diagnostics.Debug;

namespace Collisions
{
    public static class GameMath
    {
        public static float PI => (float) Math.PI;
        public static float Sin(float x) => (float) Math.Sin(x);
        public static float Cos(float x) => (float) Math.Cos(x);
        public static float Acos(float x) => (float) Math.Acos(x);

        #region Assertions 

        public static void AssertEqual(Vector a, Vector b)
        {
            AssertEqual(a.X, b.X);
            AssertEqual(a.Y, b.Y);
            WriteLine($"Assert passed {a}=~{b}");
        }

        const float FloatEqualityThreshold = 1f / 8192f;
        public static void AssertEqual(float a, float b)
        {
            Assert(AreEqual(a, b));
            WriteLine($"Assert passed {a}=~{b}");
        }

        public static bool AreEqual(float a, float b)
        {
            return Math.Abs(a - b) < FloatEqualityThreshold;
        }

        #endregion
    }
}