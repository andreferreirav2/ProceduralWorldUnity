using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMapperEngine : MonoBehaviour
{
    public MeshFilter[] objectsToMap;
    
    public MeshFilter[] objectsToExtract;
    public int numberToExtract = 12;
    public Material extracedMaterial;

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

            clearAllChildren(meshFilter.gameObject);

            GameObject sub = buildMeshFromTriangles(extractedFaces);

            sub.name = meshFilter.gameObject.name + "_1";
            sub.transform.position = meshFilter.gameObject.transform.position;
            sub.transform.localScale = Vector3.one * 1.0001f;
            sub.transform.parent = meshFilter.gameObject.transform;
            
            sw.Stop();

            UnityEngine.Debug.Log("Time taken for " + meshFilter.gameObject.name + " -> " + sw.Elapsed.TotalMilliseconds + "ms");
        }
    }
    
    private void clearAllChildren(GameObject go) {
        foreach (Transform child in go.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
    
    private GameObject buildMeshFromTriangles(List<TriangleFace> meshFaces) {
        var (rawTriangles, rawVertices) = FaceMapper.getRawFromTriangles(meshFaces);

        GameObject sub = new GameObject();
        sub.AddComponent<MeshRenderer>().material = extracedMaterial;

        MeshFilter subMeshFilter = sub.AddComponent<MeshFilter>();
        Mesh subMesh = new Mesh();
        subMeshFilter.sharedMesh = subMesh;
        subMesh.Clear();
        subMesh.vertices = rawVertices;
        subMesh.triangles = rawTriangles;
        subMesh.RecalculateNormals();
        
        return sub;
    }
}
