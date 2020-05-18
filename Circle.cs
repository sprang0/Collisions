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

        public override string ToString() => $"{{{Center}, {Radius}}}";

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
            var c = this.Center.Subtract(line.Base);
            var p = c.Project(line.Direction);
            var nearest = line.Base.Add(p);
            return CollidesWith(nearest);
        }

        public bool CollidesWith(LineSegment segment)
        {
            if (this.CollidesWith(segment.Point1)) return true;
            if (this.CollidesWith(segment.Point2)) return true;

            var d = segment.Point2.Subtract(segment.Point1);
            var c = this.Center.Subtract(segment.Point1);
            var p = c.Project(d);
            var nearest = segment.Point1.Add(p);

            return this.CollidesWith(nearest) &&
                p.Length <= d.Length &&
                p.DotProduct(d) >= 0;
        }

        public bool CollidesWith(Rectangle rectangle)
        {
            var clamped = this.Center.Clamp(rectangle);
            return this.CollidesWith(clamped);
        }

        public bool CollidesWith(OrientedRectangle orientedRectangle)
        {
            var r = new Rectangle(new Vector(0, 0), orientedRectangle.HalfExtent.Multiply(2));
            var c = new Circle(new Vector(0, 0), this.Radius);
            var distance = this.Center.Subtract(orientedRectangle.Center);
            c.Center = distance.Add(orientedRectangle.HalfExtent);

            return c.CollidesWith(r);
        }
    }
}