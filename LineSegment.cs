namespace Collisions
{
    public struct LineSegment
    {
        public Vector Point1 { get; set; }
        public Vector Point2 { get; set; }

        public LineSegment(Vector point1, Vector point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override string ToString()
        {
            return $"{{{Point1}, {Point2}}}";
        }

        public Range Project(Vector onto)
        {
            var ontoUnit = onto.UnitVector;
            var range = new Range(ontoUnit.DotProduct(this.Point1), ontoUnit.DotProduct(this.Point2));
            range.Sort();
            return range;
        }

        public bool CollidesWith(LineSegment segment)
        {
            var axisA = new Line(this.Point1, this.Point2.Subtract(this.Point1));;
            if (axisA.IsOnOneSide(segment)) return false;

            var axisB = new Line(segment.Point1, segment.Point2.Subtract(segment.Point1));
            if (axisB.IsOnOneSide(this)) return false;

            if (axisA.Direction.ParallelWith(axisB.Direction))
            {
                var rangeA = this.Project(axisA.Direction);
                var rangeB = segment.Project(axisA.Direction);
                return rangeA.Overlaps(rangeB);
            }

            return true;
        }

        public bool IsSeparatingAxis(OrientedRectangle orientedRectangle)
        {
            var rEdge0 = orientedRectangle.Edge(0);
            var rEdge2 = orientedRectangle.Edge(2);
            var n = this.Point1.Subtract(this.Point2);

            var axisRange = this.Project(n);
            var r0Range = rEdge0.Project(n);
            var r2Range = rEdge2.Project(n);
            var rProjection = r0Range.Hull(r2Range);

            return !axisRange.Overlaps(rProjection);
        }
    }
}