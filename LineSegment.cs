namespace Collisions
{
    public class LineSegment
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
    }
}