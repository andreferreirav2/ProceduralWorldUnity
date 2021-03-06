﻿using System;
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

            (mappedTriangles, _) = FaceMapper.mapTriangles(triangles, vertices);
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
    }
}
