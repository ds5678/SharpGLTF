﻿using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SharpGLTF
{
    public class BinaryTableTests
    {
        [Test]
        public void TestBinaryConversion()
        {
            var bytes = BinaryTable.GetBytes(GetTestArray<byte>());
            Assert.That(bytes.Length, Is.EqualTo(BinaryTable.GetSize<byte>() * 2));

            bytes = BinaryTable.GetBytes(GetTestArray<int>());
            Assert.That(bytes.Length, Is.EqualTo(BinaryTable.GetSize<int>() * 2));
            bytes = BinaryTable.GetBytes(new List<string>() { "a", "b" });
            Assert.That(bytes.Length, Is.EqualTo(2));

            Assert.Throws<NotImplementedException>(() => BinaryTable.GetBytes(new List<bool>() { true, false }));
            var ints = new List<List<int>>();
            ints.Add(new List<int>() { 0, 1 });
            Assert.Throws<NotImplementedException>(() => BinaryTable.GetBytes(ints));
        }

        private List<T> GetTestArray<T>()
        {
            var l = new List<T>();
            l.Add((T)Convert.ChangeType(0, typeof(T)));
            l.Add((T)Convert.ChangeType(1, typeof(T)));
            return l;
        }

    }
}
