namespace BadKittyGames.Collisions
{
    public class Rectangle
    {
        #region Rectangle

        public Vector Origin { get; set; }
        public Vector Size { get; set; }

        public Rectangle() { }
        public Rectangle(Vector origin, Vector size) : this()
        {
            Origin = origin;
            Size = size;
        }

        public Rectangle(Rectangle rectangle) : this(rectangle.Origin, rectangle.Size) { }

        public bool Equals(Rectangle rectangle) =>
            this.Origin.Equals(rectangle.Origin) && this.Size.Equals(rectangle.Size);

        public Rectangle Clone => new Rectangle(this.Origin, this.Size);

        public override string ToString() => $"Rectangle {{{Origin}, {Size}}}";

        #endregion

        #region Operations

        public Vector GetCorner(int number)
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

        public Vector GetCenter()
        {
            var halfSize = this.Size.DividedBy(2);
            return this.Origin.Add(halfSize);
        }

        public bool IsSeparatingAxis(LineSegment axis)
        {
            var n = axis.Point1.Subtract(axis.Point2);
            var rEdgeA = new LineSegment(this.GetCorner(0), this.GetCorner(1));
            var rEdgeB = new LineSegment(this.GetCorner(2), this.GetCorner(3));
            var rEdgeARange = rEdgeA.Project(n);
            var rEdgeBRange = rEdgeB.Project(n);
            var rProjection = rEdgeARange.Hull(rEdgeBRange);

            var axisRange = axis.Project(n);

            return !axisRange.Overlaps(rProjection);
        }

        public Rectangle EnlargeBy(Vector point)
        {
            var enlarged = new Rectangle(new Vector(this.Origin.X.OrLesser(point.X), this.Origin.Y.OrLesser(point.Y)),
                new Vector(point.X.OrGreater(this.Origin.X + this.Size.X), (point.Y.OrGreater(this.Origin.Y + this.Size.Y))));

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
            var h = this.Clone;

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

            var dp1 = n.DotProduct(c1);
            var dp2 = n.DotProduct(c2);
            var dp3 = n.DotProduct(c3);
            var dp4 = n.DotProduct(c4);

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

        #region Moving Collisions

        const float Epsilon = 1f / 32f;

        public bool CollidesWith(Rectangle rectangle, Vector speed)
        {
            var envelope = this.Clone;
            envelope.Origin = envelope.Origin.Add(speed);
            envelope = envelope.EnlargeBy(this);

            if (envelope.CollidesWith(rectangle))
            {
                var min = this.Size.X.OrLesser(this.Size.Y) / 4;
                var minMoveDistance = min.OrGreater(Epsilon);
                var halfSpeed = speed.DividedBy(2);

                if (speed.Length < minMoveDistance)
                    return true;

                envelope.Origin = this.Origin.Add(halfSpeed);
                envelope.Size = this.Size;

                return this.CollidesWith(rectangle, halfSpeed) ||
                    envelope.CollidesWith(rectangle, halfSpeed);
            }
            else
                return false;
        }

        public bool CollidesWith(Circle circle, Vector speed)
        {
            return circle.CollidesWith(this, speed.Negated);
        }

        public bool CollidesWith(Rectangle rectangle, Vector speed, Vector targetSpeed)
        {
            return rectangle.CollidesWith(this, targetSpeed.Subtract(speed));
        }

        #endregion

    }
}