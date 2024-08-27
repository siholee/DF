using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string NAME;
    public string TYPE;
    public string RANK;
    public int HP;
    public int HP_MAX;
    public int ATK;
    public int DEF;
    public int ATK_RANGE;
    public double ATK_SPD;
    public double MOVE_SPD;
    public float CRT_POS;
    public float CRT_DMG;

    public int HP_BASE;
    public float HP_MULBUFF;
    public float HP_SUMBUFF;

    public int ATK_BASE;
    public float ATK_MULBUFF;
    public float ATK_SUMBUFF;

    public int DEF_BASE;
    public float DEF_MULBUFF;
    public float DEF_SUMBUFF;

    public int ATK_SPD_BASE;
    public float ATK_SPD_MULBUFF;
    public float ATK_SPD_SUMBUFF;

    public int MOVE_SPD_BASE;
    public float MOVE_SPD_MULBUFF;
    public float MOVE_SPD_SUMBUFF;

    public float CRT_POS_BASE;
    public float CRT_POS_BUFF;
    
    public float CRT_DMG_BASE;
    public float CRT_DMG_BUFF;

    public List<Status> STATUS_MANAGER;

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

    public void StatusUpdate()
    {
        HP_MAX = (int)(HP_BASE * (1 + (HP_MULBUFF * 0.01f)) + HP_SUMBUFF);
        ATK = (int)(ATK_BASE * (1 + (ATK_MULBUFF * 0.01f)) + ATK_SUMBUFF);
        DEF = (int)(DEF_BASE * (1 + (DEF_MULBUFF * 0.01f)) + DEF_SUMBUFF);
        ATK_SPD = (int)(ATK_SPD_BASE * (1 + (ATK_SPD_MULBUFF * 0.01f)) + ATK_SPD_SUMBUFF);
        MOVE_SPD = (int)(MOVE_SPD_BASE * (1 + (MOVE_SPD_MULBUFF * 0.01f)) + MOVE_SPD_SUMBUFF);
        CRT_POS = CRT_POS_BASE + CRT_POS_BUFF;
        CRT_DMG = CRT_DMG_BASE + CRT_DMG_BUFF;
    }
}
