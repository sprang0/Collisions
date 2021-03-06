using System;

namespace BadKittyGames.Collisions
{
    public struct Vector
    {
        #region Vector

        public float X { get; set; }
        public float Y { get; set; }

        public Vector(float x, float y) : this()
        {
            this.X = x;
            this.Y = y;
        }

        public Vector(Vector vector) : this(vector.X, vector.Y) { }

        public static Vector Zero => new Vector(0, 0);

        public bool Equals(Vector v) => this.X.Equal(v.X) && this.Y.Equal(v.Y);

        public override string ToString() => $"Vector {{{X}, {Y}}}";

        #endregion

        #region Operator Overloads

        public static Vector operator +(Vector vector1, Vector vector2) => vector1.Add(vector2);
        public static Vector operator -(Vector vector1, Vector vector2) => vector1.Subtract(vector2);

        #endregion

        #region Operations

        public Vector Add(Vector vector) => new Vector(this.X + vector.X, this.Y + vector.Y);

        public Vector Subtract(Vector vector) => new Vector(this.X - vector.X, this.Y - vector.Y);

        public Vector MultiplyBy(float scalar) => new Vector(this.X * scalar, this.Y * scalar);

        public Vector DividedBy(float divisor) => new Vector(this.X / divisor, this.Y / divisor);

        public float DotProduct(Vector vector) => this.X * vector.X + this.Y * vector.Y;

        public Vector Negated => new Vector(-this.X, -this.Y);

        public float Length => (float) Math.Sqrt(this.X * this.X + this.Y * this.Y);

        public Vector UnitVector
        {
            get
            {
                var length = Length;
                return (length > 0) ? this.DividedBy(length) : this;
            }
        }

        public Vector Rotated90 => new Vector(-this.Y, this.X);

        public Vector Rotated180 => this.Negated;

        public Vector Rotated270 => new Vector(this.Y, -this.X);

        public Vector Rotate(float degrees)
        {
            var radians = degrees.ToRadians();
            var sin = radians.Sin();
            var cos = radians.Cos();
            return new Vector(this.X * cos - this.Y * sin, this.X * sin + this.Y * cos);
        }

        public float EnclosedAngle(Vector vector)
        {
            var ua = this.UnitVector;
            var ub = vector.UnitVector;
            var dp = ua.DotProduct(ub);
            return dp.Acos().ToDegrees();
        }

        public Vector Project(Vector onto)
        {
            var d = onto.DotProduct(onto);
            if (d <= 0) return onto;

            var dp = this.DotProduct(onto);
            return onto.MultiplyBy(dp / d);
        }

        public Vector ClampTo(Rectangle rectangle) =>
            new Vector(this.X.ClampTo(rectangle.Origin.X, rectangle.Origin.X + rectangle.Size.X),
                this.Y.ClampTo(rectangle.Origin.Y, rectangle.Origin.Y + rectangle.Size.Y));

        public bool CollidesWith(Vector point) => this.X.Equals(point.X) && this.Y.Equals(point.Y);

        public bool ParallelWith(Vector line) => this.Rotated90.DotProduct(line).Equals(0);

        #endregion
    }
}