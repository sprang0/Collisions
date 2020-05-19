using static Collisions.GameMath;

namespace Collisions
{
    public struct Range
    {
        #region Range

        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public Range(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ToString() => $"{{{Minimum}, {Maximum}}}";

        #endregion

        #region Publics 

        public void Sort()
        {
            if (this.Minimum <= this.Maximum) return;

            var min = this.Minimum;
            this.Minimum = this.Maximum;
            this.Maximum = min;
        }

        public Range Hull(Range range) =>
            new Range(this.Minimum.OrLesser(range.Minimum), this.Maximum.OrGreater(range.Maximum));

        public bool Overlaps(Range range)
        {
            return Overlapping(this.Minimum, this.Maximum, range.Minimum, range.Maximum);
        }

        public static bool Overlapping(float minA, float maxA, float minB, float maxB)
        {
            return minB <= maxA && minA <= maxB;
        }

        #endregion
    }
}