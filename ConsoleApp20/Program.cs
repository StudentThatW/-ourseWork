using System;
using System.Collections.Generic;

namespace ConsoleApp20
{
     class Program
    {
        static void Main(string[] args)
        {
            var vo1 = new VirtualObject(null, 1, 2);
            var vo2 = new VirtualObject(null, 1, 2);
            var vo3 = new VirtualObject(new List<int>(), 2, 3);
            Console.WriteLine(vo1.Equals(vo2)); // True
            Console.WriteLine(vo1.Equals(vo3)); // False
            Console.ReadLine();
        }
    }

}
