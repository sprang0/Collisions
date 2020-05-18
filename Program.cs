using System;
using static System.Diagnostics.Debug;

namespace Collisions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Test();
        }

        private static void Test()
        {
            TestVectors();

            TestShapes();

            TestCollisions();
        }

        private static void TestCollisions()
        {
            var ra = new Rectangle(new Vector(1, 1), new Vector(4, 4));
            var rb = new Rectangle(new Vector(2, 2), new Vector(5, 5));
            var rc = new Rectangle(new Vector(6, 4), new Vector(4, 2));
            Assert(ra.CollidesWith(rb));
            Assert(rb.CollidesWith(rc));
            Assert(!ra.CollidesWith(rc));
            System.Console.WriteLine("Rectangles colliding with rectangles");

            var ca = new Circle(new Vector(4, 4), 2);
            var cb = new Circle(new Vector(7, 4), 2);
            var cc = new Circle(new Vector(10, 4), 2);
            Assert(ca.CollidesWith(cb));
            Assert(cb.CollidesWith(cc));
            Assert(!ca.CollidesWith(cc));
            System.Console.WriteLine("Circles colliding with circles");

            var va = new Vector(2, 3);
            var vb = new Vector(2, 3);
            var vc = new Vector(3, 4);
            Assert(va.CollidesWith(vb));
            Assert(!va.CollidesWith(vc));
            Assert(!vb.CollidesWith(vc));
            System.Console.WriteLine("Vectors/Points colliding with Vectors/Points");

            va = new Vector(3, 5);
            vb = new Vector(3, 2);
            vc = new Vector(8, 4);
            var down = new Vector(5, -1);
            var up = new Vector(5, 2);
            var la = new Line(va, down);
            var lb = new Line(va, up);
            var lc = new Line(vb, up);
            var ld = new Line(vc, down);

            Assert(la.CollidesWith(lb));
            Assert(la.CollidesWith(lc));
            Assert(!lb.CollidesWith(lc));
            Assert(la.CollidesWith(ld));
            System.Console.WriteLine("Lines colliding with lines");

            va = new Vector(3, 4);
            vb = new Vector(11, 1);
            vc = new Vector(8, 4);
            var vd = new Vector(11, 7);
            var sa = new LineSegment(va, vb);
            var sb = new LineSegment(vc, vd);
            Assert(!(sa.CollidesWith(sb)));
            vc = new Vector(5, 1);
            sb = new LineSegment(vc, vd);
            Assert((sa.CollidesWith(sb)));
            System.Console.WriteLine("Line Segments colliding with line segments");

            var oa = new OrientedRectangle(new Vector(3, 5), new Vector(1, 3), 15);
            var ob = new OrientedRectangle(new Vector(10, 5), new Vector(2, 2), -15);
            Assert(!oa.CollidesWith(ob));
            oa = new OrientedRectangle(new Vector(7, 5), new Vector(1, 3), 15);
            Assert(oa.CollidesWith(ob));
            System.Console.WriteLine("Oriented rectangles colliding with oriented rectangles");

            ca = new Circle(new Vector(6, 4), 3);
            var pa = new Vector(8, 3);
            var pb = new Vector(11, 7);
            Assert(ca.CollidesWith(pa));
            Assert(!ca.CollidesWith(pb));
            System.Console.WriteLine("Circles colliding with points");

            ca = new Circle(new Vector(6, 3), 2);
            la = new Line(new Vector(4, 7), new Vector(5, -1));
            Assert(!ca.CollidesWith(la));
            la = new Line(new Vector(4, 7), new Vector(1, -1));
            Assert(ca.CollidesWith(la));
            la = new Line(new Vector(4, 5), new Vector(5, -1));
            Assert(ca.CollidesWith(la));
            System.Console.WriteLine("Circles colliding with lines");

            ca = new Circle(new Vector(4, 4), 3);
            sa = new LineSegment(new Vector(8, 6), new Vector(13, 6));
            Assert(!(ca.CollidesWith(sa)));
            sa = new LineSegment(new Vector(6, 6), new Vector(13, 6));
            Assert(ca.CollidesWith(sa));
            System.Console.WriteLine("Circles colliding with line segments");

            ra = new Rectangle(new Vector(3, 2), new Vector(6, 4));
            ca = new Circle(new Vector(5, 4), 1);
            cb = new Circle(new Vector(7, 8), 1);
            Assert(ca.CollidesWith(ra));
            Assert(!cb.CollidesWith(ra));
            System.Console.WriteLine("Circles colliding with rectangles");

            oa = new OrientedRectangle(new Vector(5, 4), new Vector(3, 2), 30);
            ca = new Circle(new Vector(5, 7), 2);
            Assert(ca.CollidesWith(oa));
            ca = new Circle(new Vector(0, 7), 2);
            Assert(!ca.CollidesWith(oa));
            System.Console.WriteLine("Circles colliding with oriented rectangles");

            ra = new Rectangle(new Vector(3, 2), new Vector(6, 4));
            pa = new Vector(4, 5);
            pb = new Vector(11, 4);
            Assert(ra.CollidesWith(pa));
            Assert(!ra.CollidesWith(pb));
            System.Console.WriteLine("Rectangles colliding with points");

            la = new Line(new Vector(6, 8), new Vector(2, -3));
            ra = new Rectangle(new Vector(3, 2), new Vector(6, 4));
            Assert(ra.CollidesWith(la));
            la = new Line(new Vector(6, 8), new Vector(2, -1));
            Assert(!ra.CollidesWith(la));
            System.Console.WriteLine("Rectangles colliding with lines");

            ra = new Rectangle(new Vector(3, 2), new Vector(6, 4));
            sa = new LineSegment(new Vector(6, 8), new Vector(10, 2));
            Assert(ra.CollidesWith(sa));
            sa = new LineSegment(new Vector(6, 8), new Vector(7, 7));
            Assert(!ra.CollidesWith(sa));
            System.Console.WriteLine("Rectangles colliding with line segments");

            ra = new Rectangle(new Vector(1, 5), new Vector(3, 3));
            oa = new OrientedRectangle(new Vector(10, 4), new Vector(4, 2), 25);
            Assert(!ra.CollidesWith(oa));
            ra = new Rectangle(new Vector(1, 5), new Vector(7, 3));
            Assert(ra.CollidesWith(oa));
            System.Console.WriteLine("Rectangles colliding with line oriented rectangles");
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

            var rec = new Rectangle(new Vector(2, 3), new Vector(6, 4));
            System.Console.WriteLine(rec);

            var orrec = new OrientedRectangle(new Vector(6, 4), new Vector(3, 2), 30);
        }

        private static void TestVectors()
        {
            // vectors - atoms of geometry
            var a = new Vector(3, 5);
            var b = new Vector(8, 2);
            var c = a.Add(b);
            GameMath.AssertEqual(c, new Vector(11, 7));

            a = new Vector(7, 4);
            b = new Vector(3, -3);
            c = a.Subtract(b);
            GameMath.AssertEqual(c, new Vector(4, 7));

            c = a.Add(b.Negated);
            GameMath.AssertEqual(c, a.Subtract(b));

            a = new Vector(6, 3);
            b = a.Multiply(2);
            GameMath.AssertEqual(b, new Vector(12, 6));

            a = new Vector(8, 4);
            b = a.Divide(2);
            GameMath.AssertEqual(b, new Vector(4, 2));

            float divisor = 2;
            b = a.Divide(divisor);
            c = a.Multiply(1 / divisor);
            GameMath.AssertEqual(b, c);

            var v = new Vector(10, 5);
            var f = v.Length;
            GameMath.AssertEqual(11.18033f, f);

            a = new Vector(10, 5);
            Assert(1 < a.Length);
            var u = a.UnitVector;
            GameMath.AssertEqual(1, u.Length);

            a = new Vector(12, 3);
            b = a.Rotate(50);
            System.Console.WriteLine(b);

            float degrees = 19;
            var v1 = a.Rotate(degrees);
            var v2 = a.Rotate(degrees + 360);
            GameMath.AssertEqual(v1, v2);

            a = new Vector(8, 2);
            b = new Vector(-2, 8);
            c = new Vector(-5, 5);
            GameMath.AssertEqual(0, a.DotProduct(b));
            Assert(a.DotProduct(c) < 0);
            Assert(b.DotProduct(c) > 0);

            a = new Vector(8, 2);
            b = new Vector(-2, 8);
            GameMath.AssertEqual(90, a.EnclosedAngle(b));
            GameMath.AssertEqual(0, a.DotProduct(b));

            a = new Vector(12, 5);
            b = new Vector(5, 6);
            var p = b.Project(a);
            GameMath.AssertEqual(new Vector(6.3905325f, 2.6627219f), p);
        }
    }
}