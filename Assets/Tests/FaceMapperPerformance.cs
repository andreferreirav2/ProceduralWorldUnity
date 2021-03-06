﻿using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FaceMapperPerformance
    {
        Mesh mesh;

        [SetUp]
        public void Setup()
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mesh = temp.GetComponent<MeshFilter>().sharedMesh;
        }

        [Test]
        public void CreatesInUnder5Milis()
        {
            int numberIterations = 1;

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < numberIterations; i++)
            {
                FaceMapper.createTriangles(mesh.triangles, mesh.vertices);
            }
            sw.Stop();

            Assert.Less(sw.Elapsed.TotalMilliseconds / numberIterations, 5);
        }

        [Test]
        public void MapsInUnder20Milis()
        {
            int numberIterations = 3;
            
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < numberIterations; i++)
            {
                FaceMapper.mapTriangles(mesh.triangles, mesh.vertices);
            }
            sw.Stop();
            Assert.Less(sw.Elapsed.TotalMilliseconds / numberIterations, 20);
        }
    }
}
