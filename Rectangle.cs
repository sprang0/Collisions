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

        public override string ToString()
        {
            return $"{{{Origin}, {Size}}}";
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

            return Overlapping(aLeft, aRight, bLeft, bRight) &&
                Overlapping(aBottom, aTop, bBottom, bTop);
        }

        bool Overlapping(float minA, float maxA, float minB, float maxB)
        {
            return minB <= maxA && minA <= maxB;
        }

    }
}