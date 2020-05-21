namespace Collisions
{
    public class OrientedRectangle
    {
        #region Oriented Rectangle

        public Vector Center { get; set; }
        public Vector HalfExtent { get; set; }
        public float Rotation { get; set; }

        public OrientedRectangle() { }
        public OrientedRectangle(Vector center, Vector halfExtent, float rotation) : this()
        {
            Center = center;
            HalfExtent = halfExtent;
            Rotation = rotation;
        }

        public override string ToString() => $"Oriented Rectangle {{{Center}, {HalfExtent}, {Rotation}}}";

        #endregion

        #region Operations

        public LineSegment Edge(int number)
        {
            var a = this.HalfExtent;
            var b = this.HalfExtent;

            // edge number not specified
            // only promise is sequential sides are adjacent edges (0-1, 1-2, 2-3, 3-0).
            switch (number % 4)
            {
                case 0:
                    a.X = -a.X;
                    break;
                case 1:
                    b.Y = -b.Y;
                    break;
                case 2:
                    a.Y = -a.Y;
                    b = b.Negated;
                    break;
                case 3:
                    b.X = -b.X;
                    a = a.Negated;
                    break;
            }

            a = a.Rotate(this.Rotation);
            a = a.Add(this.Center);
            b = b.Rotate(this.Rotation);
            b = b.Add(this.Center);

            return new LineSegment(a, b);
        }

        public Vector Corner(int number)
        {
            var c = this.HalfExtent;

            switch (number)
            {
                case 0:
                    c.X = -c.X;
                    break;
                case 1:
                    break; // c = this.HalfExtent
                case 2:
                    c.Y = -c.Y;
                    break;
                case 3:
                    c = c.Negated;
                    break;
            }

            c = c.Rotate(this.Rotation);

            return c.Add(this.Center);
        }

        public Rectangle GetRectangleHull()
        {
            var h = new Rectangle(this.Center, new Vector(0, 0));
            for (var i = 0; i < 4; i++)
            {
                h = h.EnlargeBy(Corner(i));
            }
            return h;
        }

        public Circle GetCircleHull() => new Circle(this.Center, this.HalfExtent.Length);

        public bool IsSeparatingAxis(LineSegment axis)
        {
            var rEdge0 = this.Edge(0);
            var rEdge2 = this.Edge(2);
            var n = axis.Point1.Subtract(axis.Point2);

            var axisRange = axis.Project(n);
            var r0Range = rEdge0.Project(n);
            var r2Range = rEdge2.Project(n);
            var rProjection = r0Range.Hull(r2Range);

            return !axisRange.Overlaps(rProjection);
        }

        #endregion

        #region Collisions

        public bool CollidesWith(Vector point)
        {
            var r = new Rectangle(new Vector(0, 0), this.HalfExtent.MultiplyBy(2));
            var p = point.Subtract(this.Center);
            p = p.Rotate(-this.Rotation);
            p = p.Add(this.HalfExtent);

            return r.CollidesWith(p);
        }

        public bool CollidesWith(LineSegment lineSegment)
        {
            var r = new Rectangle(new Vector(0, 0), this.HalfExtent.MultiplyBy(2));
            var s = new LineSegment();
            s.Point1 = lineSegment.Point1.Subtract(this.Center);
            s.Point1 = s.Point1.Rotate(-this.Rotation);
            s.Point1 = s.Point1.Add(this.HalfExtent);
            s.Point2 = lineSegment.Point2.Subtract(this.Center);
            s.Point2 = s.Point2.Rotate(-this.Rotation);
            s.Point2 = s.Point2.Add(this.HalfExtent);

            return r.CollidesWith(s);
        }

        public bool CollidesWith(Line line)
        {
            var r = new Rectangle(new Vector(0, 0), this.HalfExtent.MultiplyBy(2));
            var l = new Line();
            l.Base = line.Base.Subtract(this.Center);
            l.Base = l.Base.Rotate(-this.Rotation);
            l.Base = l.Base.Add(this.HalfExtent);
            l.Direction = line.Direction.Rotate(-this.Rotation);

            return r.CollidesWith(l);
        }

        public bool CollidesWith(Circle circle) => circle.CollidesWith(this);

        public bool CollidesWith(Rectangle rectangle) => rectangle.CollidesWith(this);

        public bool CollidesWith(OrientedRectangle orientedRectangle)
        {
            var edge = this.Edge(0);
            if (orientedRectangle.IsSeparatingAxis(edge)) return false;

            edge = this.Edge(1);
            if (orientedRectangle.IsSeparatingAxis(edge)) return false;

            edge = orientedRectangle.Edge(0);
            if (this.IsSeparatingAxis(edge)) return false;

            edge = orientedRectangle.Edge(1);
            if (this.IsSeparatingAxis(edge)) return false;

            return true;
        }

        #endregion
    }
}