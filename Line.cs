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

        public override string ToString()
        {
            return $"{{{Base}, {Direction}}}";
        }
    }
}