namespace Collisions
{
    public class Circle
    {
        #region Circle

        public Vector Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Equals(Circle circle) => this.Center.Equals(circle.Center) && this.Radius.Equal(circle.Radius);

        public override string ToString() => $"Circle {{{Center}, {Radius}}}";

        #endregion

        #region Operations

        public Circle GetCircleHullWith(Circle[] otherCircles)
        {
            var h = new Circle(this.Center, this.Radius);

            if (otherCircles == null || otherCircles.Length == 0) return h;

            var rh = this.GetRectangleHullWith(otherCircles);
            h.Center = rh.GetCenter();

            for (int i = 0; i < otherCircles.Length; i++)
            {
                var c = otherCircles[i];
                var d = c.Center.Subtract(h.Center);
                var extension = d.Length + c.Radius;
                h.Radius = extension.OrGreater(h.Radius);
            }

            return h;
        }

        public Rectangle GetRectangleHullWith(Circle[] otherCircles)
        {
            var h = GetRectangleHull();

            if (otherCircles == null || otherCircles.Length == 0) return h;

            Vector halfExtent = new Vector(), minP, maxP;
            for (int i = 0; i < otherCircles.Length; i++)
            {
                var c = otherCircles[i];
                halfExtent.X = c.Radius;
                halfExtent.Y = c.Radius;
                minP = c.Center.Subtract(halfExtent);
                maxP = c.Center.Add(halfExtent);
                h = h.EnlargeBy(minP).EnlargeBy(maxP);
            }

            return h;
        }

        public Rectangle GetRectangleHull()
        {
            var h = new Rectangle(this.Center, new Vector(0, 0));
            var halfExtent = new Vector(this.Radius, this.Radius);
            var minP = this.Center.Subtract(halfExtent);
            var maxP = this.Center.Add(halfExtent);
            return h.EnlargeBy(minP).EnlargeBy(maxP);
        }

        #endregion

        #region Collisions

        public bool CollidesWith(Vector point)
        {
            var distance = this.Center.Subtract(point);
            return distance.Length <= this.Radius;
        }

        public bool CollidesWith(LineSegment lineSegment)
        {
            if (this.CollidesWith(lineSegment.Point1)) return true;
            if (this.CollidesWith(lineSegment.Point2)) return true;

            var d = lineSegment.Point2.Subtract(lineSegment.Point1);
            var c = this.Center.Subtract(lineSegment.Point1);
            var p = c.Project(d);
            var nearest = lineSegment.Point1.Add(p);

            return this.CollidesWith(nearest) && p.Length <= d.Length && p.DotProductWith(d) >= 0;
        }

        public bool CollidesWith(Line line)
        {
            var c = this.Center.Subtract(line.Base);
            var p = c.Project(line.Direction);
            var nearest = line.Base.Add(p);
            return CollidesWith(nearest);
        }

        public bool CollidesWith(Circle circle)
        {
            var radiusSum = this.Radius + circle.Radius;
            var distance = this.Center.Subtract(circle.Center);
            return distance.Length <= radiusSum;
        }

        public bool CollidesWith(Rectangle rectangle)
        {
            var clamped = this.Center.ClampTo(rectangle);
            return this.CollidesWith(clamped);
        }

        public bool CollidesWith(OrientedRectangle orientedRectangle)
        {
            var r = new Rectangle(new Vector(0, 0), orientedRectangle.HalfExtent.MultiplyBy(2));
            var c = new Circle(new Vector(0, 0), this.Radius);
            var distance = this.Center.Subtract(orientedRectangle.Center);
            c.Center = distance.Add(orientedRectangle.HalfExtent);

            return c.CollidesWith(r);
        }

        #endregion

        #region Moving Collisions

        const float Epsilon = 1f / 32f;

        public bool CollidesWith(Rectangle rectangle, Vector speed)
        {
            var envelope = new Circle(this.Center, this.Radius);
            var halfSpeed = speed.DividedBy(2);
            var moveDistance = speed.Length;
            envelope.Center = this.Center.Add(halfSpeed);
            envelope.Radius = this.Radius + moveDistance / 2;

            if (envelope.CollidesWith(rectangle))
            {
                var minMoveDistance = (this.Radius / 4).OrGreater(Epsilon);
                if (moveDistance < minMoveDistance)
                    return true;

                envelope.Radius = this.Radius;

                return this.CollidesWith(rectangle, halfSpeed) ||
                    envelope.CollidesWith(rectangle, halfSpeed);
            }
            else
                return false;
        }

        public bool CollidesWith(Circle circle, Vector speed)
        {
            var absorbed = new Circle(circle.Center, this.Radius + circle.Radius);
            var travel = new LineSegment(this.Center, this.Center.Add(speed));
            return absorbed.CollidesWith(travel);
        }

        #endregion
    }
}