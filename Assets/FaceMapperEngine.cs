using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMapperEngine : MonoBehaviour
{
    public MeshFilter[] objectsToExtract;
    
    public Dictionary<string, int> continents = new Dictionary<string, int>() {
        {"europe", 200},
        {"africa", 700},
        {"america", 600},
        {"oceania", 50},
    };
    
    public float percentageOfSea = 0.5f;
    public Material landMaterial;
    public Material seaMaterial;
    
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
            int totalAmountOfLand = 0;
            foreach (var size in continents.Values)
            {
                totalAmountOfLand += size;
            }
            foreach (var continent in continents)
            {
                int sizeOfThisContinent = facesForContinents * continent.Value / totalAmountOfLand;
                var extractedFaces = FaceMapper.extractFaces(triangles, sizeOfThisContinent);

                createSubMesh(meshFilter.gameObject, continent.Key, extractedFaces, landMaterial);
            }
            
            createSubMesh(meshFilter.gameObject, "sea", triangles, seaMaterial);
            
            sw.Stop();
            UnityEngine.Debug.Log("Time taken for " + meshFilter.gameObject.name + " -> " + sw.Elapsed.TotalMilliseconds + "ms");
        }
    }
    
    private void createSubMesh(GameObject parent, string name, List<TriangleFace> faces, Material material) {
        GameObject sub = buildMeshFromTriangles(faces, material);

        sub.name = name;
        sub.transform.position = parent.transform.position + Vector3.up * 2.2f;
        sub.transform.parent = parent.transform;

        UnityEngine.Debug.Log(sub.name + " got " + faces.Count);
    }
    
    private void clearAllChildren(GameObject go) {
        while (go.transform.childCount != 0)
        {
            DestroyImmediate(go.transform.GetChild(0).gameObject);
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
