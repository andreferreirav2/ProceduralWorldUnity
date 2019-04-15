using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleFace {
    private static float vectorDistanceThreshold = 0.0001f;
    
    public List<Vector3> vertices { get; }
    public HashSet<TriangleFace> adjacent { get; }
    
    public TriangleFace(Vector3 v1, Vector3 v2, Vector3 v3) {
        this.vertices = new List<Vector3> {v1, v2, v3};
        this.adjacent = new HashSet<TriangleFace>();
    }
    
    public void addAdjacent(TriangleFace adj) {
        if (this.isAdjacent(adj)) {
            this.adjacent.Add(adj);
            adj.adjacent.Add(this);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return this == obj || this.hasEqualVertices(((TriangleFace) obj).vertices, 3);
    }

    public bool isAdjacent(TriangleFace adj)
    {
        return adj != null && this.hasEqualVertices(adj.vertices, 2);
    }
    
    public bool hasEqualVertices(List<Vector3> otherVertices, int targetVertices) {
        int similar = 0;
        for (int i = 0; i < vertices.Count && similar != targetVertices; i++)
        {
            for (int j = 0; j < otherVertices.Count; j++)
            {
                if (areVerticesEqual(vertices[i], otherVertices[j]))
                {
                    similar++;
                    break;
                }
            }
            
            if(similar + targetVertices - 2 < i) {
                //Debug.Log("We're in cycle " + i + " with only " + similar + " similar and whishing to reach " + targetVertices);
                return false;
            }
        }
        return similar == targetVertices;
    }
    
    public static bool areVerticesEqual(Vector3 v1, Vector3 v2) {
        return Vector3.Distance(v1, v2) < vectorDistanceThreshold;
    }
}
