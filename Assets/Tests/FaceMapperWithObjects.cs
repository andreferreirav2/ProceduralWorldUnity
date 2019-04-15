using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FaceMapperTestsWithObjects
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

            mappedTriangles = FaceMapper.mapTriangles(triangles, vertices);
        }

        [Test]
        public void MapperReturns12Triangles()
        {
            Assert.IsNotEmpty(mappedTriangles);
            Assert.AreEqual(mappedTriangles.Count, 12);
        }

        [Test]
        public void MapperReturnsAdjacentTriangles()
        {
            Assert.AreEqual(3, mappedTriangles[0].adjacent.Count);
            Assert.True(mappedTriangles[0].adjacent.Contains(mappedTriangles[1]));
            Assert.True(mappedTriangles[0].adjacent.Contains(mappedTriangles[2]));
            Assert.True(mappedTriangles[0].adjacent.Contains(mappedTriangles[4]));
        }

        [Test]
        public void MapperReturnsAllTrianglesHave3Adjacent()
        {
            foreach (TriangleFace triangle in mappedTriangles)
            {
                Assert.AreEqual(3, triangle.adjacent.Count);
            }
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
        public void MapperExtracts1ConnectedFaces()
        {
            Assert.AreEqual(1, FaceMapper.extractFaces(mappedTriangles, 1).Count);
        }

        [Test]
        public void MapperExtracts2ConnectedFaces()
        {
            Assert.AreEqual(2, FaceMapper.extractFaces(mappedTriangles, 2).Count);
        }

        [Test]
        public void MapperExtracts6ConnectedFaces()
        {
            List<TriangleFace> sixConnectedFaces = FaceMapper.extractFaces(mappedTriangles, 6);
            Assert.AreEqual(6, sixConnectedFaces.Count);
        }

        [Test]
        public void MapperExtracts12ConnectedFaces()
        {
            List<TriangleFace> twelveConnectedFaces = FaceMapper.extractFaces(mappedTriangles, 12);
            Assert.AreEqual(12, twelveConnectedFaces.Count);
        }
    }
}
