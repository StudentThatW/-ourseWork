using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20
{
    public class VirtualObject : IEquatable<VirtualObject>
    {
        private static readonly IEqualityComparer<VirtualObject> Comparer = AutoEquality<VirtualObject>.Create();
        public List<int> ListSomething { get; set; }
        [EqualityProperty]
        public int Length { get; set; }
        [EqualityProperty]
        public int Width { get; set; }

        public VirtualObject(List<int> h, int l, int w)
        {
            this.ListSomething = h;
            this.Length = l;
            this.Width = w;
        }

        public bool Equals(VirtualObject other) => Comparer.Equals(this, other);

        public int GetHashCode(VirtualObject obj) => Comparer.GetHashCode(obj);

    }
}
