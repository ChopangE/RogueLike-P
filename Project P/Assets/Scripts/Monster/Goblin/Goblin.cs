using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Re_Monster
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
        monsterName = "Goblin";

        base.SetInfo(monsterName);

        MonsterType = EMonsterType.Normal;
        bc.offset = offset;
        bc.size = size;

        hp = monster_Hp;
        atk = monster_Attack;

        searchDistance = new Vector2(5, 2);

        dir = 1;

        BaseSkill skill1 = gameObject.AddComponent<Goblin_Slash>();
        BaseSkill normalSkill = gameObject.AddComponent<Goblin_Attack>();

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
}
