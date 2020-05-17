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

        public bool CollidesWith(Circle circle)
        {
            var radiusSum = this.Radius + circle.Radius;
            var distance = this.Center.Subtract(circle.Center);
            return distance.Length <= radiusSum;
        }

        public bool CollidesWith(Vector point)
        {
            var distance = this.Center.Subtract(point);
            return distance.Length <= this.Radius;
        }

        public bool CollidesWith(Line line)
        {
            var lc = this.Center.Subtract(line.Base);
            var p = lc.Project(line.Direction);
            var nearest = line.Base.Add(p);
            return CollidesWith(nearest);
        }
    }
}