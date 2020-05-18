namespace Collisions
{
    public struct Line
    {
        public Vector Base { get; set; }
        public Vector Direction { get; set; }

        public Line(Vector @base, Vector direction)
        {
            Base = @base;
            Direction = direction;
        }

        public override string ToString() => $"{{{Base}, {Direction}}}";

        public bool CollidesWith(Line line)
        {
            if (this.Direction.ParallelWith(line.Direction))
                return this.EquivalentTo(line);
            else return true;
        }

        public bool EquivalentTo(Line line)
        {
            if (!this.Direction.ParallelWith(line.Direction)) return false;

            var d = this.Base.Subtract(line.Base);
            return d.ParallelWith(this.Direction);
        }

        public bool IsOnOneSide(LineSegment segment)
        {
            var d1 = segment.Point1.Subtract(this.Base);
            var d2 = segment.Point2.Subtract(this.Base);
            var n = this.Direction.Rotated90;
            return n.DotProduct(d1) * n.DotProduct(d2) > 0;
        }
    }
}