using System;
using static System.Diagnostics.Debug;

namespace Collisions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TestCollisions();
        }

        private static void TestCollisions()
        {
            TestVectors();

            TestShapes();
        }

        private static void TestShapes()
        {
            // shapes
            var p = new Vector(3, 3);
            var d1 = new Vector(7, -2);
            var d2 = new Vector(3, 5);
            var l1 = new Line(p, d1);
            var l2 = new Line(p, d2);
            System.Console.WriteLine($"Line1: {l1}  Line2: {l2}");

            var p1 = new Vector(3, 4);
            var p2 = new Vector(11, 1);
            var p3 = new Vector(8, 4);
            var p4 = new Vector(11, 7);
            var s1 = new LineSegment(p1, p2);
            var s2 = new LineSegment(p3, p4);
            System.Console.WriteLine($"Seg1: {s1}  Seg2: {s2}");

            var c = new Vector(6, 4);
            float r = 4;
            var c1 = new Circle(c, r);
            System.Console.WriteLine($"Circle: {c1}");
        }

        private static void TestVectors()
        {
            // vectors - atoms of geometry
            var v = new Vector(10, 4);

            var a = new Vector(3, 5);
            var b = new Vector(8, 2);
            var c = GameMath.Add(a, b);
            GameMath.AssertEqual(c, new Vector(11, 7));

            a = new Vector(7, 4);
            b = new Vector(3, -3);
            c = GameMath.Subtract(a, b);
            GameMath.AssertEqual(c, new Vector(4, 7));

            c = GameMath.Add(a, GameMath.Negate(b));
            GameMath.AssertEqual(c, GameMath.Subtract(a, b));

            a = new Vector(6, 3);
            b = GameMath.Multiply(a, 2);
            GameMath.AssertEqual(b, new Vector(12, 6));

            a = new Vector(8, 4);
            b = GameMath.Divide(a, 2);
            GameMath.AssertEqual(b, new Vector(4, 2));

            float divisor = 2;
            b = GameMath.Divide(a, divisor);
            c = GameMath.Multiply(a, 1 / divisor);
            GameMath.AssertEqual(b, c);

            v = new Vector(10, 5);
            var f = GameMath.Length(v);
            GameMath.AssertEqual(11.18033f, f);

            a = new Vector(10, 5);
            Assert(1 < GameMath.Length(a));
            var u = GameMath.UnitVector(a);
            GameMath.AssertEqual(1, GameMath.Length(u));

            a = new Vector(12, 3);
            b = GameMath.Rotate(a, 50);
            System.Console.WriteLine(b);

            float degrees = 19;
            var v1 = GameMath.Rotate(a, degrees);
            var v2 = GameMath.Rotate(a, degrees + 360);
            GameMath.AssertEqual(v1, v2);

            a = new Vector(8, 2);
            b = new Vector(-2, 8);
            c = new Vector(-5, 5);
            GameMath.AssertEqual(0, GameMath.DotProduct(a, b));
            Assert(GameMath.DotProduct(a, c) < 0);
            Assert(GameMath.DotProduct(b, c) > 0);

            a = new Vector(8, 2);
            b = new Vector(-2, 8);
            GameMath.AssertEqual(90, GameMath.EnclosedAngle(a, b));
            GameMath.AssertEqual(0, GameMath.DotProduct(a, b));

            a = new Vector(12, 5);
            b = new Vector(5, 6);
            var p = GameMath.Project(b, a);
            GameMath.AssertEqual(new Vector(6.3905325f, 2.6627219f), p);
        }
    }
}