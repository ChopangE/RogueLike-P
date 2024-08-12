using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_mushroom : Monster
{
    string monsterName;

    [Header("# Monster Collider")]

    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 size;

    [Header("# Monster Status")]
    [SerializeField] float monster_Hp;
    [SerializeField] float monster_Attack;
    [SerializeField] float monster_Speed;
    [SerializeField] float monster_AttackRange;
    [SerializeField] float monster_SkillRange;
    [SerializeField] float monster_AttackDelay;
    [SerializeField] float monster_SkillCoolTime;

    [Header("# Monster Attack")]
    [SerializeField] Vector2 monster_attackSize;
    [SerializeField] Vector2 monster_skillSize;

    protected override void Init()
    {
        base.Init();
    }

    protected override void SetInfo(string monsterName)
    {
        monsterName = "Mushroom";

        base.SetInfo(monsterName);

        bc.offset = offset;
        bc.size = size;

        hp = monster_Hp;
        atk = monster_Attack;
        speed = monster_Speed;

        attackRangeDistance = monster_AttackRange;
        skillRangeDistance = monster_SkillRange; 

        playerDetected = false;

        attackDelay = monster_AttackDelay;
        skillCoolTime = monster_SkillCoolTime;

        hitRange = monster_attackSize;
        skillRange = monster_skillSize;

        dir = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();

        SetInfo(monsterName);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        IsWallChecked();
        IsGroundChecked();
    }
}
