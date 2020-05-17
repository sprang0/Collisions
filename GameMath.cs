using System;
using static System.Diagnostics.Debug;

namespace Collisions
{
    public static class GameMath
    {
        public static Vector Add(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector Subtract(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector Multiply(Vector v, float scalar)
        {
            return new Vector(v.X * scalar, v.Y * scalar);
        }

        public static Vector Divide(Vector v, float divisor)
        {
            return new Vector(v.X / divisor, v.Y / divisor);
        }

        public static Vector Negate(Vector v)
        {
            return new Vector(-v.X, -v.Y);
        }

        public static float Length(Vector v)
        {
            return (float) Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static Vector UnitVector(Vector v)
        {
            var length = Length(v);
            return (length > 0) ? Divide(v, length) : v;
        }

        public static Vector Rotate(Vector v, float degrees)
        {
            var rads = DegreesToRadians(degrees);
            var sin = Sin(rads);
            var cos = Cos(rads);
            return new Vector(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }

        public static float EnclosedAngle(Vector a, Vector b)
        {
            var ua = UnitVector(a);
            var ub = UnitVector(b);
            var dp = DotProduct(ua, ub);
            return RadiansToDegrees(Acos(dp));
        }

        public static Vector Project(Vector project, Vector onto)
        {
            var d = DotProduct(onto, onto);
            if (d > 0)
            {
                var dp = DotProduct(project, onto);
                return Multiply(onto, dp / d);
            }
            return onto;
        }

        public static float DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float DegreesToRadians(float degrees)
        {
            return PI * degrees / 180;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * 180 / PI;
        }

        #region Helpers

        static float PI => (float) Math.PI;
        static float Sin(float x) => (float) Math.Sin(x);
        static float Cos(float x) => (float) Math.Cos(x);
        static float Acos(float x) => (float) Math.Acos(x);

        #endregion

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
            Assert(Math.Abs(a - b) < FloatEqualityThreshold);
            WriteLine($"Assert passed {a}=~{b}");
        }

        #endregion
    }
}