using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TriangleFaceTests
    {
        TriangleFace t1, t2, t3;
        
        [SetUp]
        public void Setup() {
            t1 = new TriangleFace(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0));
            t2 = new TriangleFace(new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(0, 1, 0));
            t3 = new TriangleFace(new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0));
        }

        [Test]
        public void FacesAreEqual()
        {
            Assert.True(t1.Equals(new TriangleFace(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0))));
            Assert.True(t1.Equals(t1));
        }

        [Test]
        public void FacesNotAreEqual()
        {
            Assert.False(t1.Equals(new TriangleFace(new Vector3(0.001f, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0))));
            Assert.False(t1.Equals(t2));
        }

        [Test]
        public void VerticesAreSimilar()
        {
            Assert.True(TriangleFace.areVerticesEqual(new Vector3(1, 1, 1), new Vector3(1, 1, 1)));
        }

        [Test]
        public void VerticesAreNotSimilar()
        {
            Assert.False(TriangleFace.areVerticesEqual(new Vector3(2, 1, 1), new Vector3(1, 1, 1)));
        }

        [Test]
        public void SameTriangleHas3SimilarVertices()
        {
            Assert.True(t1.hasEqualVertices(t1.vertices, 3));
        }

        [Test]
        public void OtherTriangleHas3SimilarVertices()
        {
            Assert.True(t1.hasEqualVertices(t2.vertices, 2));
        }

        [Test]
        public void DiffTriangleHasOneSimilarVertices()
        {
            Assert.True(t1.hasEqualVertices(t3.vertices, 1));
        }

        [Test]
        public void IsAdjacent()
        {
            Assert.True(t1.isAdjacent(t2));
            Assert.True(t2.isAdjacent(t3));
            Assert.False(t1.isAdjacent(t3));
        }

        [Test]
        public void AddAdjacentIsEmpty()
        {
            Assert.IsEmpty(t1.adjacent);
            Assert.IsEmpty(t2.adjacent);
        }

        [Test]
        public void AddAdjacent()
        {
            t1.addAdjacent(t2);

            Assert.AreEqual(1, t1.adjacent.Count);
            Assert.AreEqual(1, t2.adjacent.Count);
            Assert.True(t1.adjacent.Contains(t2));
            Assert.True(t2.adjacent.Contains(t1));
        }

        [Test]
        public void AddAdjacentTwice()
        {
            t1.addAdjacent(t2);
            t1.addAdjacent(t2);

            Assert.AreEqual(1, t1.adjacent.Count);
            Assert.AreEqual(1, t2.adjacent.Count);
            Assert.True(t1.adjacent.Contains(t2));
            Assert.True(t2.adjacent.Contains(t1));
        }

        [Test]
        public void AddAdjacentTwiceReversed()
        {
            t1.addAdjacent(t2);
            t2.addAdjacent(t1);

            Assert.AreEqual(1, t1.adjacent.Count);
            Assert.AreEqual(1, t2.adjacent.Count);
            Assert.True(t1.adjacent.Contains(t2));
            Assert.True(t2.adjacent.Contains(t1));
        }
    }
}
