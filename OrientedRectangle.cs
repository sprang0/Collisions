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
    }
}