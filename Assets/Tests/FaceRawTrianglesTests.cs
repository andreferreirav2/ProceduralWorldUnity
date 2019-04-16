using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FaceRawTrianglesTests
    {
        Vector3[] vertices;
        List<TriangleFace> mappedTriangles;

        [SetUp]
        public void Setup()
        {
            vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };

            mappedTriangles = new List<TriangleFace>() {
                                    new TriangleFace(vertices[0], vertices[1], vertices[2]),
                                    new TriangleFace(vertices[0], vertices[2], vertices[3]) };
        }

        [Test]
        public void MapperRawExtractsAllVertices()
        {
            var (_, rawVertices) = FaceMapper.getRawFromTriangles(mappedTriangles);

            Assert.IsNotEmpty(rawVertices);
            Assert.AreEqual(4, rawVertices.Length);
            foreach (var vertex in rawVertices)
            {
                Assert.Contains(vertex, vertices);
            }
        }

        [Test]
        public void MapperRawExtractsAllTriangles()
        {
            var (rawTriangles, _) = FaceMapper.getRawFromTriangles(mappedTriangles);

            Assert.IsNotEmpty(rawTriangles);
            Assert.AreEqual(2 * 3, rawTriangles.Length);

            Assert.AreEqual(0, rawTriangles[0]);
            Assert.AreEqual(1, rawTriangles[1]);
            Assert.AreEqual(2, rawTriangles[2]);

            Assert.AreEqual(0, rawTriangles[3]);
            Assert.AreEqual(2, rawTriangles[4]);
            Assert.AreEqual(3, rawTriangles[5]);
            
        }
    }
}
