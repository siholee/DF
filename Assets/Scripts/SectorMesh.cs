using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SectorMesh : MonoBehaviour
{
    public float angle = 45f;  // 부채꼴 각도
    public float radius = 1f;  // 부채꼴 반지름
    public int segments = 10;  // 부채꼴 세그먼트 수

    // 윤곽선 색상과 채우기 색상 설정
    public Color outlineColor = Color.black;
    public Color fillColor = Color.white;

    void Start()
    {
        // 메쉬 생성
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];
        Vector2[] uvs = new Vector2[vertices.Length];

        vertices[0] = Vector3.zero;
        float angleStep = angle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = angleStep * i;
            float rad = Mathf.Deg2Rad * currentAngle;
            vertices[i + 1] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            uvs[i + 1] = new Vector2(vertices[i + 1].x / radius, vertices[i + 1].z / radius);  // UV 좌표 설정

            if (i < segments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        uvs[0] = new Vector2(0.5f, 0.5f);  // 중앙점의 UV 좌표 설정

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;  // UV 좌표 설정
        mesh.RecalculateNormals();

        // MeshFilter에 메쉬 설정
        GetComponent<MeshFilter>().mesh = mesh;

        // MeshRenderer에 새로운 Material 생성 및 색상 적용
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // 두 가지 색상의 Material을 생성하고 적용
        Material material = new Material(Shader.Find("Standard"));
        material.color = fillColor;  // 내부 색상 적용
        meshRenderer.material = material;

        // 윤곽선 그리기 (기본적인 방법)
        DrawOutline(meshRenderer);
    }

    void DrawOutline(MeshRenderer meshRenderer)
    {
        // 윤곽선 두께와 색상을 설정
        Material outlineMaterial = new Material(Shader.Find("Sprites/Default")); // 간단한 윤곽선 셰이더 사용
        outlineMaterial.color = outlineColor;

        // 윤곽선 오브젝트를 생성하여 원본 오브젝트 주변에 배치
        GameObject outline = new GameObject("Outline");
        outline.transform.parent = this.transform;
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localScale = Vector3.one * 1.05f; // 윤곽선 크기를 약간 키움

        MeshFilter outlineMeshFilter = outline.AddComponent<MeshFilter>();
        outlineMeshFilter.mesh = GetComponent<MeshFilter>().mesh;

        MeshRenderer outlineRenderer = outline.AddComponent<MeshRenderer>();
        outlineRenderer.material = outlineMaterial;
    }
}
