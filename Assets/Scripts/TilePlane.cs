using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TilePlane : MonoBehaviour, IShape
{
    public Vector2 TileSize = new Vector2(2, 1);
    public Vector2 PlaneSize = new Vector2(4, 8);

    private MeshFilter meshFilter;
    public float Seam { get; set; }
    public float Angle { get; set; }
    public float Offset { get; set; }

    private void Awake()
    {
        // Caching component
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        // Create mesh with default parameters
        Create();
    }

    // Mesh creation
    public void Create()
    {
        // List of meshes to combine together
        List<CombineInstance> combine = new List<CombineInstance>();

        // We need additional (Sin(Angle) * height) tiles, +1 tile for possible offset
        int additionalX = Mathf.CeilToInt(Mathf.Abs(Mathf.Sin((float)Angle * Mathf.Deg2Rad) ) * PlaneSize.y / TileSize.x) + 1;

        // Calculate columns and rows
        int height = Mathf.CeilToInt(PlaneSize.y / TileSize.y) + 1;
        int width = Mathf.CeilToInt(PlaneSize.x / TileSize.x) + 1;
        for (int y = 0; y < height; y++)
        {
            for (int x = -additionalX; x < width; x++)
            {
                // Offset between [-tilesize, 0]
                float offsetX = (Offset * y) % TileSize.x - TileSize.x;
                Mesh mesh = MeshCreator.CreateQuad(new Vector3(x * (TileSize.x + Seam) + offsetX, 0, y * (TileSize.y + Seam)), TileSize.x, TileSize.y, Angle);

                // Init new mesh for combining
                CombineInstance combineInstance = new CombineInstance()
                {
                    mesh = mesh,
                    transform = Matrix4x4.Rotate(Quaternion.AngleAxis(Angle, Vector3.up))
                };
                combine.Add(combineInstance);
            }
        }

        // Create new empty mesh and combine all meshes inside it
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.subMeshCount = 1;
        meshFilter.mesh.CombineMeshes(combine.ToArray(), true, true);
        
        // Slicing mesh to fit bounds. We need 1e-5f, because of a bug in zero point
        meshFilter.mesh = SliceBounds(new Rect(1e-5f, 1e-5f, PlaneSize.x, PlaneSize.y), meshFilter.mesh);
    }


    public Mesh SliceBounds(Rect bounds, Mesh mesh)
    {
        // Left bound
        SlicedHull m = Slicer.Slice(mesh, new EzySlice.Plane(new Vector3(bounds.x, 0, bounds.y), Vector3.right),
            new TextureRegion(0, 0, 1, 1), 0);
        if (m != null)
            mesh = m.upperHull;

        // Right bound
        m = Slicer.Slice(mesh, new EzySlice.Plane(new Vector3(bounds.xMax, 0, bounds.yMax), Vector3.left),
            new TextureRegion(0, 0, 1, 1), 0);
        if (m != null)
            mesh = m.upperHull;

        // Bottom bound
        m = Slicer.Slice(mesh, new EzySlice.Plane(new Vector3(bounds.x, 0, bounds.y), Vector3.forward),
            new TextureRegion(0, 0, 1, 1), 0);
        if (m != null)
            mesh = m.upperHull;

        // Top bound
        m = Slicer.Slice(mesh, new EzySlice.Plane(new Vector3(bounds.xMax, 0, bounds.yMax), Vector3.back),
            new TextureRegion(0, 0, 1, 1), 0);
        if (m != null)
            mesh = m.upperHull;

        return mesh;
    }

    // Calculate all tiles area
    public double GetArea()
    {
        return Math.Round(MeshUtils.GetArea(meshFilter.mesh), 2);
    }

}
