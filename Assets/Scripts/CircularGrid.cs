using UnityEngine;
using System.Collections.Generic;

public class CircularGrid : MonoBehaviour
{
    public int numberOfRings = 5;
    public int sectorsPerRing = 6;
    public float ringSpacing = 2f;

    private List<GameObject> gridCells = new List<GameObject>();

    public void GenerateGrid()
    {
        for (int ring = 0; ring < numberOfRings; ring++)
        {
            float innerRadius = ring * ringSpacing;
            float outerRadius = (ring + 1) * ringSpacing;
            int sectorCount = sectorsPerRing * (ring + 1);

            for (int sector = 0; sector < sectorCount; sector++)
            {
                float angleStart = 360f / sectorCount * sector;
                float angleEnd = 360f / sectorCount * (sector + 1);
                GameObject gridCell = CreateGridCell(innerRadius, outerRadius, angleStart, angleEnd);
                gridCells.Add(gridCell);
            }
        }
    }

    GameObject CreateGridCell(float innerRadius, float outerRadius, float angleStart, float angleEnd)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        // Convert angles to radians
        float radStart = Mathf.Deg2Rad * angleStart;
        float radEnd = Mathf.Deg2Rad * angleEnd;

        // Create vertices
        vertices.Add(new Vector3(innerRadius * Mathf.Cos(radStart), 0, innerRadius * Mathf.Sin(radStart))); // inner start
        vertices.Add(new Vector3(innerRadius * Mathf.Cos(radEnd), 0, innerRadius * Mathf.Sin(radEnd)));     // inner end
        vertices.Add(new Vector3(outerRadius * Mathf.Cos(radStart), 0, outerRadius * Mathf.Sin(radStart))); // outer start
        vertices.Add(new Vector3(outerRadius * Mathf.Cos(radEnd), 0, outerRadius * Mathf.Sin(radEnd)));     // outer end

        // Define triangles
        triangles.Add(0); // inner start
        triangles.Add(2); // outer start
        triangles.Add(1); // inner end

        triangles.Add(1); // inner end
        triangles.Add(2); // outer start
        triangles.Add(3); // outer end

        // UVs (optional, for texture mapping)
        uv.Add(new Vector2(0, 0));
        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(0, 1));
        uv.Add(new Vector2(1, 1));

        // Assign vertices, triangles, and uv to mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();

        // Create a new GameObject to hold the mesh
        GameObject gridCell = new GameObject("GridCell");
        gridCell.transform.parent = this.transform;

        // Assign mesh to the MeshFilter and MeshRenderer components
        MeshFilter meshFilter = gridCell.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gridCell.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;

        // Add a basic material (you can customize this as needed)
        meshRenderer.material = new Material(Shader.Find("Standard"));

        return gridCell;
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        GameObject closestCell = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject cell in gridCells)
        {
            float distance = Vector3.Distance(position, cell.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCell = cell;
            }
        }

        return closestCell != null ? closestCell.transform.position : position;
    }
}
