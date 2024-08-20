using UnityEngine;
using System.Collections.Generic;

public class GridCell : MonoBehaviour
{
    public float innerRadius;
    public float outerRadius;
    public float angleStart;
    public float angleEnd;

    void Start()
    {
        CreateCellMesh();
    }

    void CreateCellMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        // 각도를 라디안으로 변환
        float radStart = Mathf.Deg2Rad * angleStart;
        float radEnd = Mathf.Deg2Rad * angleEnd;

        // 꼭짓점 생성
        vertices.Add(new Vector3(innerRadius * Mathf.Cos(radStart), 0, innerRadius * Mathf.Sin(radStart))); // inner start
        vertices.Add(new Vector3(innerRadius * Mathf.Cos(radEnd), 0, innerRadius * Mathf.Sin(radEnd)));     // inner end
        vertices.Add(new Vector3(outerRadius * Mathf.Cos(radStart), 0, outerRadius * Mathf.Sin(radStart))); // outer start
        vertices.Add(new Vector3(outerRadius * Mathf.Cos(radEnd), 0, outerRadius * Mathf.Sin(radEnd)));     // outer end

        // 삼각형 정의
        triangles.Add(0); // inner start
        triangles.Add(2); // outer start
        triangles.Add(1); // inner end

        triangles.Add(1); // inner end
        triangles.Add(2); // outer start
        triangles.Add(3); // outer end

        // UVs (옵션, 텍스처 맵핑에 사용)
        uv.Add(new Vector2(0, 0));
        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(0, 1));
        uv.Add(new Vector2(1, 1));

        // 메쉬에 꼭짓점, 삼각형 및 uv 할당
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();

        // 메쉬 필터와 렌더러에 메쉬 할당
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;

        // 쉐이더를 찾고, 머티리얼을 안전하게 생성
        Shader standardShader = Shader.Find("Standard");
        if (standardShader == null)
        {
            Debug.LogError("Standard Shader를 찾을 수 없습니다!");
            return;
        }

        Material whiteMaterial = new Material(standardShader);
        if (whiteMaterial == null)
        {
            Debug.LogError("Material을 생성할 수 없습니다!");
            return;
        }

        whiteMaterial.color = Color.white;
        meshRenderer.material = whiteMaterial;
    }

}
