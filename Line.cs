namespace Collisions
{
    public class Line
    {
        #region Line

        public Vector Base { get; set; }
        public Vector Direction { get; set; }

        public Line() { }
        public Line(Vector @base, Vector direction) : this()
        {
            Base = @base;
            Direction = direction;
        }

        public override string ToString() => $"Line {{{Base}, {Direction}}}";

        #endregion

        #region Operations 

        public bool IsEquivalentTo(Line line)
        {
            if (!this.Direction.ParallelWith(line.Direction)) return false;

            var d = this.Base.Subtract(line.Base);
            return d.ParallelWith(this.Direction);
        }

        public bool IsOnSameSideOf(LineSegment segment)
        {
            var d1 = segment.Point1.Subtract(this.Base);
            var d2 = segment.Point2.Subtract(this.Base);
            var n = this.Direction.Rotated90;
            return n.DotProduct(d1) * n.DotProduct(d2) > 0;
        }

        #endregion

        #region Collisions

        public bool CollidesWith(Vector point)
        {
            if (point.CollidesWith(this.Base)) return true;
            var lp = point.Subtract(this.Base);
            return lp.ParallelWith(this.Direction);
        }

        public bool CollidesWith(LineSegment lineSegment)
        {
            return !this.IsOnSameSideOf(lineSegment);
        }

        public bool CollidesWith(Line line)
        {
            if (this.Direction.ParallelWith(line.Direction))
                return this.IsEquivalentTo(line);
            else return true;
        }

        public bool CollidesWith(Circle circle) => circle.CollidesWith(this);

        public bool CollidesWith(Rectangle rectangle) => rectangle.CollidesWith(this);

        public bool CollidesWith(OrientedRectangle orientedRectangle) => orientedRectangle.CollidesWith(this);

        #endregion
    }
}