using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20
{
    public class Box : IEquatable<Box>
    {
        private static readonly IEqualityComparer<Box> Comparer = AutoEquality<Box>.Create();
        public Box(int h, int l, int w)
        {
            this.Height = h;
            this.Length = l;
            this.Width = w;
        }

        [EqualityProperty]
        public int Height { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }

        public override String ToString()
        {
            return String.Format("({0}, {1}, {2})", Height, Length, Width);
        }

        public bool Equals(Box other) => Comparer.Equals(this, other);

        public int GetHashCode(Box obj) => Comparer.GetHashCode(obj);
    }
}
