using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string NAME;
    public string TYPE;
    public string RANK;
    public int HP;
    public int ATK;
    public int DEF;
    public int ATK_RANGE;
    public double ATK_SPD;
    public double MOVE_SPD;

    public int HP_CURRENT;
    public bool ATTACHED;
    public List<GameObject> TARGET_HOLDER;

    // Start 메서드에서 원을 생성
    void Start()
    {
        CreateAttackRangeIndicator();
    }

    // ATK_RANGE에 기반한 보이지 않는 원을 생성하는 메서드
    void CreateAttackRangeIndicator()
    {
        GameObject rangeIndicator = new GameObject("AttackRange");
        rangeIndicator.transform.parent = this.transform;
        rangeIndicator.transform.localPosition = Vector3.zero;

        // 원형 콜라이더 추가
        SphereCollider rangeCollider = rangeIndicator.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true; // 트리거로 설정
        rangeCollider.radius = ATK_RANGE;

        // 콜라이더가 보이지 않도록 합니다.
        MeshRenderer renderer = rangeIndicator.AddComponent<MeshRenderer>();
        renderer.enabled = false;
    }
}
