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

        public override string ToString() => $"{{{X}, {Y}}}";

        #endregion

        #region Publics

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
            var dp = ua.DotProduct(ub);
            return (Acos(dp)).ToDegrees();
        }

        public float DotProduct(Vector vector) => this.X * vector.X + this.Y * vector.Y;

        public Vector Project(Vector onto)
        {
            var d = onto.DotProduct(onto);
            if (d <= 0) return onto;

            var dp = this.DotProduct(onto);
            return onto.MultiplyBy(dp / d);
        }

        public Vector ClampTo(Rectangle rectangle) => new Vector(this.X.Clamp(rectangle.Origin.X, rectangle.Origin.X + rectangle.Size.X),
            this.Y.Clamp(rectangle.Origin.Y, rectangle.Origin.Y + rectangle.Size.Y));

        public bool CollidesWith(Vector point) => GameMath.AreEqual(this.X, point.X) &&
            GameMath.AreEqual(this.Y, point.Y);

        public bool ParallelWith(Vector line) => GameMath.AreEqual(0, this.Rotated90.DotProduct(line));

        public bool EqualTo(Vector vector) => GameMath.AreEqual(0, this.X - vector.X) &&
            GameMath.AreEqual(0, this.Y - vector.Y);

        #endregion
    }
}