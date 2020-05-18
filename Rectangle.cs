using static Collisions.GameMath;

namespace Collisions
{
    public struct Rectangle
    {
        public Vector Origin { get; set; }
        public Vector Size { get; set; }

        public Rectangle(Vector origin, Vector size)
        {
            Origin = origin;
            Size = size;
        }

        public override string ToString() => $"{{{Origin}, {Size}}}";

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

        public Rectangle Enlarge(Vector point)
        {
            var enlarged = new Rectangle(
                new Vector(this.Origin.X.OrLesser(point.X),
                    this.Origin.Y.OrLesser(point.Y)),
                new Vector(point.X.OrGreater(this.Origin.X + this.Size.X),
                    (point.Y.OrGreater(this.Origin.Y + this.Size.Y))));
            enlarged.Size = enlarged.Size.Subtract(enlarged.Origin);
            return enlarged;
        }

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

            var dp1 = n.DotProduct(c1);
            var dp2 = n.DotProduct(c2);
            var dp3 = n.DotProduct(c3);
            var dp4 = n.DotProduct(c4);

            return dp1 * dp2 <= 0 || dp2 * dp3 <= 0 || dp3 * dp4 <= 0;
        }

        public bool CollidesWith(LineSegment segment)
        {
            var sLine = new Line(segment.Point1, segment.Point2.Subtract(segment.Point1));
            if (!CollidesWith(sLine)) return false;

            var rRange = new Range(this.Origin.X, this.Origin.X + this.Size.X);
            var sRange = new Range(segment.Point1.X, segment.Point2.X);
            sRange.Sort();
            if (!rRange.Overlaps(sRange)) return false;

            rRange.Minimum = this.Origin.Y;
            rRange.Maximum = this.Origin.Y + this.Size.Y;
            sRange.Minimum = segment.Point1.Y;
            sRange.Maximum = segment.Point2.Y;
            sRange.Sort();
            if (!rRange.Overlaps(sRange)) return false;

            return true;
        }

        public bool CollidesWith(OrientedRectangle orientedRectangle)
        {
            var orHull = orientedRectangle.Hull;
            if (!orHull.CollidesWith(this)) return false;

            var edge = orientedRectangle.Edge(0);
            if (this.IsSeparatingAxis(edge)) return false;

            edge = orientedRectangle.Edge(1);
            if (this.IsSeparatingAxis(edge)) return false;

            return true;
        }
    }
}