using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Monster
{
    [SerializeField]
    string monsterName;

    [Header("# Monster Collider")]

    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 size;

    [Header("# Monster Status")]
    [SerializeField] float monster_Hp;
    [SerializeField] float monster_Attack;

    protected override void Init()
    {
        base.Init();
        SetInfo(monsterName);
    }

    protected override void SetInfo(string monsterName)
    {
        monsterName = "FlyingEye";

        base.SetInfo(monsterName);

        MonsterType = EMonsterType.Normal;
        
        bc.offset = offset;
        bc.size = size;

        hp = monster_Hp;
        currentHp = hp; 

        atk = monster_Attack;

        searchDistance = new Vector2(5, 2);

        dir = 1;

        BaseSkill skill1 = gameObject.AddComponent<FlyingEye_Rush>();
        BaseSkill normalSkill = gameObject.AddComponent<FlyingEye_Attack>();

        skillList.Add(skill1);
        skillList.Add(normalSkill);

        foreach (var skill in skillList)
        {
            skill.SetInfo(this.gameObject.GetComponent<Creature>());
        }

        foreach (var skill in skillList)
        {
            if (skill.IsSkillEnable())
                enableSkillList.Add(skill);
        }
    }
    void EnterRush()
    {
        rb.mass = 0.1f;
        rb.AddForce(Vector2.right * (dir) * 50, ForceMode2D.Force);
    }

    void ExitRush()
    {
        rb.mass = 1f;
        rb.velocity = Vector2.zero;
    }
}
