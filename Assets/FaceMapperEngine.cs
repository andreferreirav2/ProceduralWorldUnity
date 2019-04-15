using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMapperEngine : MonoBehaviour
{
    public MeshFilter[] objectsToMap;

    [ContextMenu("Map")]
    void FaceMap()
    {
        foreach (var meshFilter in objectsToMap)
        {
            Mesh mesh = meshFilter.sharedMesh;

            Stopwatch sw = Stopwatch.StartNew();
            FaceMapper.mapTriangles(mesh.triangles, mesh.vertices);
            sw.Stop();

            UnityEngine.Debug.Log("Time taken for " + meshFilter.gameObject.name + " -> " + sw.Elapsed.TotalMilliseconds + "ms");
        }
    }
}
