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

            return OverlappingOnAxis(aLeft, aRight, bLeft, bRight) &&
                OverlappingOnAxis(aBottom, aTop, bBottom, bTop);
        }
    }
}