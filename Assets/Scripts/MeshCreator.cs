using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

// Static class that creates primitives
public static class MeshCreator
{
    public static Mesh CreateQuad(Vector3 origin, float width, float height, float rotation = 0)
    {
        var normal = Vector3.up;
        var mesh = new Mesh()
        {
            vertices = new Vector3[] { origin, origin + new Vector3(0, 0, height), origin + new Vector3(width, 0, height), origin + new Vector3(width, 0, 0) },
            normals = new Vector3[] { normal, normal, normal, normal },
            uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
            triangles = new int[] { 0, 1, 2, 0, 2, 3 }
        };
        return mesh;
    }
}
