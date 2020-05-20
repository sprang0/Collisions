using System;
using static Collisions.GameMath;

namespace Collisions
{
    public struct Vector
    {
        #region Vector
        public float X { get; set; }
        public float Y { get; set; }

        public Vector(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString() => $"Vector {{{X}, {Y}}}";

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Vector v)
                return this.X == v.X && this.Y == v.Y;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int) this.X ^ (int) this.Y;
        }

        #endregion

        #region Operations

        public Vector Add(Vector vector) => new Vector(this.X + vector.X, this.Y + vector.Y);

        public Vector Subtract(Vector vector) => new Vector(this.X - vector.X, this.Y - vector.Y);

        public Vector MultiplyBy(float scalar) => new Vector(this.X * scalar, this.Y * scalar);

        public Vector DividedBy(float divisor) => new Vector(this.X / divisor, this.Y / divisor);

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

        public Vector Rotate(float degrees)
        {
            var radians = degrees.ToRadians();
            var sin = Sin(radians);
            var cos = Cos(radians);
            return new Vector(this.X * cos - this.Y * sin, this.X * sin + this.Y * cos);
        }

        public Vector Rotated90 => new Vector(-this.Y, this.X);

        public float EnclosedAngle(Vector vector)
        {
            var ua = this.UnitVector;
            var ub = vector.UnitVector;
            var dp = ua.DotProductWith(ub);
            return (Acos(dp)).ToDegrees();
        }

        public float DotProductWith(Vector vector) => this.X * vector.X + this.Y * vector.Y;

        public Vector Project(Vector onto)
        {
            var d = onto.DotProductWith(onto);
            if (d <= 0) return onto;

            var dp = this.DotProductWith(onto);
            return onto.MultiplyBy(dp / d);
        }

        public Vector ClampTo(Rectangle rectangle) =>
            new Vector(this.X.Clamp(rectangle.Origin.X, rectangle.Origin.X + rectangle.Size.X),
                this.Y.Clamp(rectangle.Origin.Y, rectangle.Origin.Y + rectangle.Size.Y));

        public bool CollidesWith(Vector point) => this.X.Equals(point.X) && this.Y.Equals(point.Y);

        public bool ParallelWith(Vector line) => this.Rotated90.DotProductWith(line).Equals(0);

        public bool EqualTo(Vector vector) => (this.X - vector.X).Equals(0) && (this.Y - vector.Y).Equals(0);

        #endregion
    }
}