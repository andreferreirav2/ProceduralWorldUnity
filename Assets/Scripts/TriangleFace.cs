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
        
        return this == obj || this.numberOfEqualVertices(((TriangleFace) obj).vertices) == 3;
    }

    public bool isAdjacent(TriangleFace adj)
    {
        return adj != null && !adj.Equals(null) && !this.Equals(adj) &&
              this.numberOfEqualVertices(adj.vertices) == 2;
    }
    
    public int numberOfEqualVertices(List<Vector3> otherVertices) {
        int similar = 0;
        foreach (Vector3 v in vertices)
        {
            foreach (Vector3 otherV in otherVertices)
            {
                if(areVerticesEqual(v, otherV)) {
                    similar++;
                    break;
                }
            }
        }
        return similar;
    }
    
    public static bool areVerticesEqual(Vector3 v1, Vector3 v2) {
        return Vector3.Distance(v1, v2) < vectorDistanceThreshold;
    }
}
