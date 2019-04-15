using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMapperEngine : MonoBehaviour
{
    public MeshFilter objectToMap;

    [ContextMenu("Map")]
    void FaceMap()
    {
        Mesh mesh = objectToMap.sharedMesh;

        Stopwatch sw = Stopwatch.StartNew();
        FaceMapper.mapTriangles(mesh.triangles, mesh.vertices);
        sw.Stop();

        UnityEngine.Debug.Log("Time taken: " +  sw.Elapsed.TotalMilliseconds + "ms");
    }
}
