using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMapperEngine : MonoBehaviour
{
    public MeshFilter[] objectsToExtract;
    public int numberOfContinents = 5;
    public float percentageOfSea = 0.5f;
    public Material extracedMaterial;
    
    [ContextMenu("Map+Extract")]
    void MapAndExtract()
    {
        foreach (var meshFilter in objectsToExtract)
        {
            Mesh mesh = meshFilter.sharedMesh;

            Stopwatch sw = Stopwatch.StartNew();
            var (triangles, vertices) = FaceMapper.mapTriangles(mesh.triangles, mesh.vertices);
            clearAllChildren(meshFilter.gameObject);

            int totalFaces = triangles.Count;
            int facesForContinents = (int)(totalFaces * (1f - percentageOfSea));
            int sizeOfEachContinent = (int)(facesForContinents / numberOfContinents);
            for (int i = 0; i < numberOfContinents; i++)
            {
                var extractedFaces = FaceMapper.extractFaces(triangles, sizeOfEachContinent);

                GameObject sub = buildMeshFromTriangles(extractedFaces, extracedMaterial);

                sub.name = meshFilter.gameObject.name + "_" + i;
                sub.transform.position = meshFilter.gameObject.transform.position + Vector3.up * 2;
                sub.transform.localScale = Vector3.one * 1.0001f;
                sub.transform.parent = meshFilter.gameObject.transform;
            }

            GameObject subSea = buildMeshFromTriangles(triangles, meshFilter.gameObject.GetComponent<MeshRenderer>().material);

            subSea.name = meshFilter.gameObject.name + "_sea";
            subSea.transform.position = meshFilter.gameObject.transform.position + Vector3.up * 2;
            subSea.transform.localScale = Vector3.one * 1.0001f;
            subSea.transform.parent = meshFilter.gameObject.transform;
            
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
    
    private GameObject buildMeshFromTriangles(List<TriangleFace> meshFaces, Material material) {
        var (rawTriangles, rawVertices) = FaceMapper.getRawFromTriangles(meshFaces);

        GameObject sub = new GameObject();
        sub.AddComponent<MeshRenderer>().material = material;

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
