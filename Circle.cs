namespace Collisions
{
    public class Circle
    {
        public Vector Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public override string ToString()
        {
            return $"{{{Center}, {Radius}}}";
        }
    }
}