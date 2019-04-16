using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMapperEngine : MonoBehaviour
{
    public MeshFilter[] objectsToMap;
    
    public MeshFilter[] objectsToExtract;
    public int numberToExtract = 12;

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

    [ContextMenu("Map+Extract")]
    void MapAndExtract()
    {
        foreach (var meshFilter in objectsToExtract)
        {
            Mesh mesh = meshFilter.sharedMesh;

            Stopwatch sw = Stopwatch.StartNew();
            var (triangles, vertices) = FaceMapper.mapTriangles(mesh.triangles, mesh.vertices);
            var extractedFaces = FaceMapper.extractFaces(triangles, Mathf.Min(numberToExtract, triangles.Count));
            
            var (rawTriangles, rawVertices) = FaceMapper.getRawFromTriangles(extractedFaces);

            foreach (Transform child in meshFilter.gameObject.transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }

            GameObject sub = new GameObject();
            sub.transform.position = meshFilter.gameObject.transform.position + Vector3.up * 2;
            sub.transform.parent = meshFilter.gameObject.transform;
            sub.AddComponent<MeshRenderer>().material = meshFilter.gameObject.GetComponent<MeshRenderer>().material;
            
            MeshFilter subMeshFilter = sub.AddComponent<MeshFilter>();
            Mesh subMesh = new Mesh();
            subMeshFilter.sharedMesh = subMesh;
            subMesh.Clear();
            subMesh.vertices = rawVertices;
            subMesh.triangles = rawTriangles;
            subMesh.RecalculateNormals();
            
            sw.Stop();

            UnityEngine.Debug.Log("Time taken for " + meshFilter.gameObject.name + " -> " + sw.Elapsed.TotalMilliseconds + "ms");
        }
    }
    
    private void buildMeshFromTriangles() {
        
    }
}
