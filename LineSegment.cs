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

        public override string ToString() => $"{{{Point1}, {Point2}}}";

        #endregion

        #region Publics

        public Range Project(Vector onto)
        {
            var ontoUnit = onto.UnitVector;
            var range = new Range(ontoUnit.DotProduct(this.Point1), ontoUnit.DotProduct(this.Point2));
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
            return p.EqualTo(pr) && pr.Length <= d.Length && pr.DotProduct(d) >= 0;
        }

        public bool CollidesWith(LineSegment segment)
        {
            var axisA = new Line(this.Point1, this.Point2.Subtract(this.Point1));;
            if (axisA.IsOnSameSideOf(segment)) return false;

            var axisB = new Line(segment.Point1, segment.Point2.Subtract(segment.Point1));
            if (axisB.IsOnSameSideOf(this)) return false;

            if (axisA.Direction.ParallelWith(axisB.Direction))
            {
                var rangeA = this.Project(axisA.Direction);
                var rangeB = segment.Project(axisA.Direction);
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