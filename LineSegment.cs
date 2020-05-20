namespace Collisions
{
    public struct LineSegment
    {
        #region Line Segment

        public Vector Point1 { get; set; }
        public Vector Point2 { get; set; }

        public LineSegment(Vector point1, Vector point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override string ToString() => $"Line Segment {{{Point1}, {Point2}}}";

        #endregion

        #region Operations

        public Range Project(Vector onto)
        {
            var ontoUnit = onto.UnitVector;
            var range = new Range(ontoUnit.DotProductWith(this.Point1), ontoUnit.DotProductWith(this.Point2));
            range.Sort();
            return range;
        }

        #endregion

        #region Collisions

        public bool CollidesWith(Vector point)
        {
            var d = this.Point2.Subtract(this.Point1);
            var p = point.Subtract(this.Point1);
            var pr = p.Project(d);
            return p.EqualTo(pr) && pr.Length <= d.Length && pr.DotProductWith(d) >= 0;
        }

        public bool CollidesWith(LineSegment lineSegment)
        {
            var axisA = new Line(this.Point1, this.Point2.Subtract(this.Point1));;
            if (axisA.IsOnSameSideOf(lineSegment)) return false;

            var axisB = new Line(lineSegment.Point1, lineSegment.Point2.Subtract(lineSegment.Point1));
            if (axisB.IsOnSameSideOf(this)) return false;

            if (axisA.Direction.ParallelWith(axisB.Direction))
            {
                var rangeA = this.Project(axisA.Direction);
                var rangeB = lineSegment.Project(axisA.Direction);
                return rangeA.Overlaps(rangeB);
            }

            return true;
        }

        public bool CollidesWith(Line line) => line.CollidesWith(this);

        public bool CollidesWith(Circle circle) => circle.CollidesWith(this);

        public bool CollidesWith(Rectangle rectangle) => rectangle.CollidesWith(this);

        public bool CollidesWith(OrientedRectangle orientedRectangle) => orientedRectangle.CollidesWith(this);

        #endregion
    }
}