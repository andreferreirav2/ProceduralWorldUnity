using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FaceMapper
{
    public static List<TriangleFace> mapTriangles(int[] triangles, Vector3[] vertices)
    {
        List<TriangleFace> mappedTriangles = createTriangles(triangles, vertices);

        int count = 0;
        for (int i = 0; i < mappedTriangles.Count - 1; i++)
        {
            for (int j = i + 1; j < mappedTriangles.Count; j++)
            {
                mappedTriangles[i].addAdjacent(mappedTriangles[j]); count++;
            }
        }
        
        Debug.Log("Faces: " + mappedTriangles.Count + ", Cycles: " + count);

        return mappedTriangles;
    }
    
    public static List<TriangleFace> mapTrianglesOrig(int[] triangles, Vector3[] vertices)
    {
        List<TriangleFace> mappedTriangles = createTriangles(triangles, vertices);

        for (int i = 0; i < mappedTriangles.Count - 1; i++)
        {
            for (int j = i + 1; j < mappedTriangles.Count; j++)
            {
                mappedTriangles[i].addAdjacent(mappedTriangles[j]);
            }
        }

        return mappedTriangles;
    }

    public static List<TriangleFace> createTriangles(int[] triangles, Vector3[] vertices)
    {
        List<TriangleFace> mappedTriangles = new List<TriangleFace>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            mappedTriangles.Add(new TriangleFace(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]));
        }

        return mappedTriangles;
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
