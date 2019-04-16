using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FaceMapper
{
    public static (List<TriangleFace>, Dictionary<Vector3, List<TriangleFace>>) mapTriangles(int[] triangles, Vector3[] vertices)
    {
        var count = 0;
        var (mappedTriangles, mappedVertices) = createTriangles(triangles, vertices);
        
        foreach (var vertex in mappedVertices.Keys)
        {
            for (var i = 0; i < mappedVertices[vertex].Count - 1; i++)
            {
                for (var j = i + 1; j < mappedVertices[vertex].Count; j++)
                {
                    mappedVertices[vertex][i].addAdjacent(mappedVertices[vertex][j]); count++;
                }
            }
        }

        return (mappedTriangles, mappedVertices);
    }

    public static (List<TriangleFace>, Dictionary<Vector3, List<TriangleFace>>) createTriangles(int[] triangles, Vector3[] vertices)
    {
        var mappedTriangles = new List<TriangleFace>();
        var vertexToTriangles = new Dictionary<Vector3, List<TriangleFace>>();
        
        for (int i = 0; i < triangles.Length; i += 3)
        {
            var v1 = vertices[triangles[i]];
            var v2 = vertices[triangles[i + 1]];
            var v3 = vertices[triangles[i + 2]];
            var triangle = new TriangleFace(v1, v2, v3);
            
            mappedTriangles.Add(triangle);
            foreach (Vector3 v in new ArrayList(){v1, v2, v3})
            {
                List<TriangleFace> trianglesOfVertex;
                if (!vertexToTriangles.ContainsKey(v))
                {
                    trianglesOfVertex = new List<TriangleFace>();
                    vertexToTriangles.Add(v, trianglesOfVertex);
                } else {
                    trianglesOfVertex = vertexToTriangles[v];
                }
                trianglesOfVertex.Add(triangle);
            }
        }

        return (mappedTriangles, vertexToTriangles);
    }

    public static List<TriangleFace> extractFaces(List<TriangleFace> faces, int numToExtract)
    {
        if (faces == null || numToExtract > faces.Count || numToExtract <= 0)
        {
             throw new Exception("The list of faces doesn't have enough faces to extract");
        }

        List<TriangleFace> adjPoll = new List<TriangleFace>();
        HashSet<TriangleFace> extractedFaces = new HashSet<TriangleFace>();

        TriangleFace face = faces[UnityEngine.Random.Range(0, faces.Count)];
        extractOneFace(faces, extractedFaces, adjPoll, face);
        
        for (int i = 1; i < numToExtract; i++)
        {
            bool foundOne = false;
            foreach (TriangleFace adj in adjPoll)
            {
                if (!extractedFaces.Contains(adj))
                {
                    foundOne = true;
                    extractOneFace(faces, extractedFaces, adjPoll, adj);
                    Shuffle(adjPoll);
                    break;
                }
            }

            if (! foundOne)
            {
                throw new Exception("Wasn't able to find a connected");
            }

        }
        
        return new List<TriangleFace>(extractedFaces);
    }

    public static (int[], Vector3[]) getRawFromTriangles(List<TriangleFace> faces) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        foreach (var face in faces)
        {
            foreach (var vertex in face.vertices)
            {
                if (!vertices.Contains(vertex))
                {
                    vertices.Add(vertex);
                }
                
                triangles.Add(vertices.IndexOf(vertex));
            }
        }
        
        return (triangles.ToArray(), vertices.ToArray());
    }

    static void extractOneFace(List<TriangleFace> allFaces, HashSet<TriangleFace> extractedFaces, List<TriangleFace> adjPoll, TriangleFace face) {
        extractedFaces.Add(face);
        allFaces.Remove(face);
        adjPoll.Remove(face);
        adjPoll.AddRange(face.adjacent);
    }

    private static void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
