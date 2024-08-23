using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Re_Monster
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
        monsterName = "Skeleton";

        base.SetInfo(monsterName);

        MonsterType = EMonsterType.Normal;
        bc.offset = offset;
        bc.size = size;

        hp = monster_Hp;
        currentHp = hp;

        atk = monster_Attack;

        searchDistance = new Vector2(5, 2);

        dir = 1;

        BaseSkill skill = gameObject.AddComponent<Skeleton_Attack>();
        BaseSkill normalAttack = gameObject.AddComponent<Skeleton_UpperAttack>();

        skillList.Add(skill);
        skillList.Add(normalAttack);

        foreach (var _skill in skillList)
        {
            _skill.SetInfo(this.gameObject.GetComponent<Creature>());
        }

        foreach (var _skill in skillList)
        {
            if (_skill.IsSkillEnable())
                enableSkillList.Add(_skill);
        }
    }


}
