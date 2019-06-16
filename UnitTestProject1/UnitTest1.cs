using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp20;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var vo1 = new VirtualObject(new List<int>(), 1, 2);
            var vo2 = new VirtualObject(new List<int>(), 1, 2);
            Assert.AreEqual(vo1.Equals(vo2), true);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var p1 = new Box(1, 2, 3);
            var p2 = new Box(2, 3, 3);
            Assert.AreEqual(p1.Equals(p2), false);
        }

        
        [TestMethod]
        public void TestMethod3()
        {
            List<int> numbers = new List<int>() { 1, 2, 3, 45 };
            List<int> numbers2 = new List<int>() { 1, 2, 3, 45 };
            var numbers3 = numbers;
            var p1 = new VirtualObject(numbers, 2, 3);
            var p2 = new VirtualObject(numbers, 3, 2);
            var p3 = new VirtualObject(numbers3, 2, 3);
            Assert.AreEqual(p1.Equals(p2), false);
            Assert.AreEqual(p1.Equals(p3), true);
            Assert.AreEqual(numbers.Equals(numbers2), false);
        }
        
    }
}
