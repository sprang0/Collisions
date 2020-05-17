using static Collisions.GameMath;

namespace Collisions
{
    public struct Range
    {
        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public Range(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ToString()
        {
            return $"{{{Minimum}, {Maximum}}}";
        }

        public void Sort()
        {
            if (this.Minimum <= this.Maximum) return;

            var min = this.Minimum;
            this.Minimum = this.Maximum;
            this.Maximum = min;
        }

        public Range Hull(Range range) => new Range((this.Minimum < range.Minimum ? this.Minimum : range.Minimum),
            (this.Maximum > range.Maximum ? this.Maximum : range.Maximum));

        public bool Overlaps(Range range)
        {
            return OverlappingOnAxis(this.Minimum, this.Maximum, range.Minimum, range.Maximum);
        }
    }
}