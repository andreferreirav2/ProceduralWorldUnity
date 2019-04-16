using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FaceExtracterTests
    {
        Vector3[] vertices;
        int[] triangles;
        List<TriangleFace> mappedTriangles;

        [SetUp]
        public void Setup()
        {
            vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0),
                                       new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 1, 1), new Vector3(1, 1, 1) };
            triangles = new int[] { 0, 1, 2,
                                    1, 2, 3,

                                    0, 1, 4,
                                    1, 4, 5,

                                    0, 2, 4,
                                    2, 4, 6,

                                    2, 3, 6,
                                    3, 6, 7,

                                    1, 3, 5,
                                    3, 5, 7,

                                    4, 6, 5,
                                    5, 6, 7 };

            (mappedTriangles, _) = FaceMapper.mapTriangles(triangles, vertices);
        }

        [Test]
        public void MapperExtractsInvalidConnectedFaces()
        {
            Assert.Throws<Exception>(delegate { FaceMapper.extractFaces(null, 0); });
            Assert.Throws<Exception>(delegate { FaceMapper.extractFaces(mappedTriangles, 100); });
            Assert.Throws<Exception>(delegate { FaceMapper.extractFaces(mappedTriangles, -1); });
            Assert.Throws<Exception>(delegate { FaceMapper.extractFaces(mappedTriangles, 0); });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(12)]
        public void MapperExtractsFaces(int numFaces)
        {
            var connectedFaces = FaceMapper.extractFaces(mappedTriangles, numFaces);
            Assert.AreEqual(numFaces, connectedFaces.Count);
            Assert.AreEqual(12 - numFaces, mappedTriangles.Count);
            
            foreach (var extractedFace in connectedFaces)
            {
                Assert.False(mappedTriangles.Contains(extractedFace));
            }
        }
    }
}
