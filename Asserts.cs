using static System.Diagnostics.Debug;

namespace BadKittyGames.Collisions
{
    public static class Asserts
    {
        public static void AssertEqual(Vector a, Vector b)
        {
            AssertEqual(a.X, b.X);
            AssertEqual(a.Y, b.Y);
            WriteLine($"Assert passed {a}=~{b}");
        }

        public static void AssertEqual(float a, float b)
        {
            Assert(a.Equal(b));
            WriteLine($"Assert passed {a}=~{b}");
        }
    }
}