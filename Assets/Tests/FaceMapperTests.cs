using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Diagnostics;

namespace Tests
{
    public class FaceMapperTests
    {
        Vector3 downleft, downright, midright, midleft, upright, upleft;
        Vector3[] vertices;
        int[] triangles;
        
        [SetUp]
        public void Setup()
        {   
            downleft = new Vector3(0, 0, 0);
            downright = new Vector3(1, 0, 0);
            midright = new Vector3(1, 1, 0);
            midleft = new Vector3(0, 1, 0);
            upright = new Vector3(1, 2, 0);
            upleft = new Vector3(0, 2, 0);

            vertices = new Vector3[] { downleft, downright, midleft, midright, upleft, upright };
            triangles = new int[] { 0, 1, 2,
                                    1, 3, 2,
                                    2, 3, 4,
                                    3, 5, 4 };
        }

        [Test]
        public void MapperRetursFourTriangles()
        {
            List<TriangleFace> mappedTriangles = FaceMapper.createTriangles(triangles, vertices);

            Assert.IsNotEmpty(mappedTriangles);
            Assert.AreEqual(mappedTriangles.Count, 4);
        }

        [Test]
        public void MapperRetursFourTrianglesThatAreCorrect()
        {
            List<TriangleFace> mappedTriangles = FaceMapper.createTriangles(triangles, vertices);

            Assert.Contains(downleft, mappedTriangles[0].vertices);
            Assert.Contains(downright, mappedTriangles[0].vertices);
            Assert.Contains(midleft, mappedTriangles[0].vertices);

            Assert.Contains(downright, mappedTriangles[1].vertices);
            Assert.Contains(midright, mappedTriangles[1].vertices);
            Assert.Contains(midleft, mappedTriangles[1].vertices);
        }

        [Test]
        public void MapperRetursFourTrianglesThatAreAdjacent()
        {
            List<TriangleFace> mappedTriangles = FaceMapper.mapTriangles(triangles, vertices);

            Assert.True(mappedTriangles[0].isAdjacent(mappedTriangles[1]));
            Assert.True(mappedTriangles[1].isAdjacent(mappedTriangles[2]));
            Assert.True(mappedTriangles[2].isAdjacent(mappedTriangles[3]));
        }

        [Test]
        public void TriangleHasAdjacent()
        {
            List<TriangleFace> mappedTriangles = FaceMapper.mapTriangles(triangles, vertices);

            Assert.IsNotEmpty(mappedTriangles[0].adjacent);
            Assert.AreEqual(mappedTriangles[0].adjacent.Count, 1);

            Assert.IsNotEmpty(mappedTriangles[1].adjacent);
            Assert.AreEqual(mappedTriangles[1].adjacent.Count, 2);

            Assert.IsNotEmpty(mappedTriangles[2].adjacent);
            Assert.AreEqual(mappedTriangles[2].adjacent.Count, 2);

            Assert.IsNotEmpty(mappedTriangles[3].adjacent);
            Assert.AreEqual(mappedTriangles[3].adjacent.Count, 1);
        }

        [Test]
        public void TrianglesAreAdjacent()
        {
            List<TriangleFace> mappedTriangles = FaceMapper.mapTriangles(triangles, vertices);

            Assert.True(mappedTriangles[0].adjacent.Contains(mappedTriangles[1]));

            Assert.True(mappedTriangles[1].adjacent.Contains(mappedTriangles[0]));
            Assert.True(mappedTriangles[1].adjacent.Contains(mappedTriangles[2]));

            Assert.True(mappedTriangles[2].adjacent.Contains(mappedTriangles[1]));
            Assert.True(mappedTriangles[2].adjacent.Contains(mappedTriangles[3]));

            Assert.True(mappedTriangles[3].adjacent.Contains(mappedTriangles[2]));
        }
    }
}
