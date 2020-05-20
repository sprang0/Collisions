using static Collisions.GameMath;

namespace Collisions
{
    public struct Rectangle
    {
        #region Rectangle

        public Vector Origin { get; set; }
        public Vector Size { get; set; }

        public Rectangle(Vector origin, Vector size)
        {
            Origin = origin;
            Size = size;
        }

        public override string ToString() => $"Rectangle {{{Origin}, {Size}}}";

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Rectangle r)
                return this.Origin.Equals(r.Origin) && this.Size.EqualTo(r.Size);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int) Origin.X ^ (int) Origin.Y ^ (int) Size.X ^ (int) Size.Y;
        }

        #endregion

        #region Operations

        public Vector Corner(int number)
        {
            var corner = this.Origin;
            switch (number)
            {
                case 0:
                    corner.X += this.Size.X;
                    break;
                case 1:
                    corner = corner.Add(this.Size);
                    break;
                case 2:
                    corner.Y += this.Size.Y;
                    break;
                default:
                    break; // corner = this.Origin
            }
            return corner;
        }

        public bool IsSeparatingAxis(LineSegment axis)
        {
            var n = axis.Point1.Subtract(axis.Point2);
            var rEdgeA = new LineSegment(this.Corner(0), this.Corner(1));
            var rEdgeB = new LineSegment(this.Corner(2), this.Corner(3));
            var rEdgeARange = rEdgeA.Project(n);
            var rEdgeBRange = rEdgeB.Project(n);
            var rProjection = rEdgeARange.Hull(rEdgeBRange);

            var axisRange = axis.Project(n);

            return !axisRange.Overlaps(rProjection);
        }

        public Rectangle EnlargeBy(Vector point)
        {
            var enlarged = new Rectangle(
                new Vector(this.Origin.X.OrLesser(point.X),
                    this.Origin.Y.OrLesser(point.Y)),
                new Vector(point.X.OrGreater(this.Origin.X + this.Size.X),
                    (point.Y.OrGreater(this.Origin.Y + this.Size.Y))));
            enlarged.Size = enlarged.Size.Subtract(enlarged.Origin);
            return enlarged;
        }

        public Rectangle EnlargeBy(Rectangle extent)
        {
            var maxCorner = extent.Origin.Add(extent.Size);
            var enlarged = this.EnlargeBy(maxCorner);
            return enlarged.EnlargeBy(extent.Origin);
        }

        public Rectangle GetHullWith(Rectangle[] otherRectangles)
        {
            var h = new Rectangle(this.Origin, this.Size);
            if (otherRectangles == null || otherRectangles.Length == 0) return h;

            for (int i = 0; i < otherRectangles.Length; i++)
                h = h.EnlargeBy(otherRectangles[i]);

            return h;
        }

        #endregion

        #region Collisions

        public bool CollidesWith(Vector point)
        {
            var left = this.Origin.X;
            var right = left + this.Size.X;
            var bottom = this.Origin.Y;
            var top = bottom + this.Size.Y;

            return left <= point.X && bottom <= point.Y && right >= point.X && top >= point.Y;
        }

        public bool CollidesWith(Line line)
        {
            var n = line.Direction.Rotated90;
            var c1 = this.Origin;
            var c2 = c1.Add(this.Size);
            var c3 = new Vector(c2.X, c1.Y);
            var c4 = new Vector(c1.X, c2.Y);

            c1 = c1.Subtract(line.Base);
            c2 = c2.Subtract(line.Base);
            c3 = c3.Subtract(line.Base);
            c4 = c4.Subtract(line.Base);

            var dp1 = n.DotProductWith(c1);
            var dp2 = n.DotProductWith(c2);
            var dp3 = n.DotProductWith(c3);
            var dp4 = n.DotProductWith(c4);

            return dp1 * dp2 <= 0 || dp2 * dp3 <= 0 || dp3 * dp4 <= 0;
        }

        public bool CollidesWith(LineSegment lineSegment)
        {
            var sLine = new Line(lineSegment.Point1, lineSegment.Point2.Subtract(lineSegment.Point1));
            if (!CollidesWith(sLine)) return false;

            var rRange = new Range(this.Origin.X, this.Origin.X + this.Size.X);
            var sRange = new Range(lineSegment.Point1.X, lineSegment.Point2.X);
            sRange.Sort();
            if (!rRange.Overlaps(sRange)) return false;

            rRange.Minimum = this.Origin.Y;
            rRange.Maximum = this.Origin.Y + this.Size.Y;
            sRange.Minimum = lineSegment.Point1.Y;
            sRange.Maximum = lineSegment.Point2.Y;
            sRange.Sort();
            if (!rRange.Overlaps(sRange)) return false;

            return true;
        }

        public bool CollidesWith(Circle circle) => circle.CollidesWith(this);

        public bool CollidesWith(Rectangle rectangle)
        {
            var aLeft = this.Origin.X;
            var aRight = aLeft + this.Size.X;
            var bLeft = rectangle.Origin.X;
            var bRight = bLeft + rectangle.Size.X;

            var aBottom = this.Origin.Y;
            var aTop = aBottom + this.Size.Y;
            var bBottom = rectangle.Origin.Y;
            var bTop = bBottom + rectangle.Size.Y;

            return Range.Overlapping(aLeft, aRight, bLeft, bRight) &&
                Range.Overlapping(aBottom, aTop, bBottom, bTop);
        }

        public bool CollidesWith(OrientedRectangle orientedRectangle)
        {
            var orHull = orientedRectangle.GetRectangleHull();
            if (!orHull.CollidesWith(this)) return false;

            var edge = orientedRectangle.Edge(0);
            if (this.IsSeparatingAxis(edge)) return false;

            edge = orientedRectangle.Edge(1);
            if (this.IsSeparatingAxis(edge)) return false;

            return true;
        }

        #endregion
    }
}