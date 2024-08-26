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

    void CreateAttackRangeIndicator()
    {
        GameObject rangeIndicator = new GameObject("AttackRange");
        rangeIndicator.transform.parent = this.transform;
        rangeIndicator.transform.localPosition = Vector3.zero;

        SphereCollider rangeCollider = rangeIndicator.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true; 
        rangeCollider.radius = ATK_RANGE;

        MeshRenderer renderer = rangeIndicator.AddComponent<MeshRenderer>();
        renderer.enabled = false;
    }
}
