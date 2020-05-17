namespace Collisions
{
    public struct OrientedRectangle
    {
        public Vector Center { get; set; }
        public Vector HalfExtent { get; set; }
        public float Rotation { get; set; }

        public OrientedRectangle(Vector center, Vector halfExtent, float rotation)
        {
            Center = center;
            HalfExtent = halfExtent;
            Rotation = rotation;
        }

        public override string ToString()
        {
            return $"{{{Center}, {HalfExtent}, {Rotation}}}";
        }

        public LineSegment Edge(int number)
        {
            var a = this.HalfExtent;
            var b = this.HalfExtent;

            // edge number not specified
            // only promise is sequential sides are adjacent edges (0-1, 1-2, 2-3, 3-0).
            switch (number % 4)
            {
                case 0:
                    a.X = -a.X;
                    break;
                case 1:
                    b.Y = -b.Y;
                    break;
                case 2:
                    a.Y = -a.Y;
                    b = b.Negate;
                    break;
                case 3:
                    b.X = -b.X;
                    a = a.Negate;
                    break;
            }

            a = a.Rotate(this.Rotation);
            a = a.Add(this.Center);
            b = b.Rotate(this.Rotation);
            b = b.Add(this.Center);

            return new LineSegment(a, b);
        }

        public bool CollidesWith(OrientedRectangle orientedRectangle)
        {
            var edge = this.Edge(0);
            if (edge.IsSeparatingAxis(orientedRectangle)) return false;

            edge = this.Edge(1);
            if (edge.IsSeparatingAxis(orientedRectangle)) return false;

            edge = orientedRectangle.Edge(0);
            if (edge.IsSeparatingAxis(this)) return false;

            edge = orientedRectangle.Edge(1);
            if (edge.IsSeparatingAxis(this)) return false;

            return true;
        }
    }
}