using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FaceMapper
{
    public static List<TriangleFace> mapTriangles(int[] triangles, Vector3[] vertices)
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

        return mappedTriangles;
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
        List<TriangleFace> extractedFaces = new List<TriangleFace>();

        TriangleFace face = faces[0];
        extractedFaces.Add(face);
        adjPoll.AddRange(face.adjacent);
        
        for (int i = 1; i < numToExtract; i++)
        {
            bool foundOne = false;
            foreach (TriangleFace adj in adjPoll)
            {
                if (!extractedFaces.Contains(adj))
                {
                    foundOne = true;
                    extractedFaces.Add(adj);
                    adjPoll.AddRange(adj.adjacent);
                    adjPoll.Remove(adj);
                    break;
                }
            }

            if (! foundOne)
            {
                throw new Exception("Wasn't able to find a connected");
            }

        }
        
        return extractedFaces;
    }
}
