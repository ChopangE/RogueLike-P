using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class BringerOfDeath : Re_Monster
{
    [SerializeField]
    string monsterName;

    [Header("# Monster Collider")]
    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 size;

    [Header("# Monster Status")]
    [SerializeField] float monster_Hp;
    [SerializeField] float monster_Attack;

    public List<HandOfDeath> spells;
    public int summonNum; 

    protected override void Init()
    {
        base.Init();

        SetInfo(monsterName);
    }

    protected override void SetInfo(string monsterName)
    {
        monsterName = "Bringer Of Death";

        base.SetInfo(monsterName);

        MonsterType = EMonsterType.Boss;
        bc.offset = offset; // 0.1515 -0.127
        bc.size = size; // 1.84, 3.57

        hp = monster_Hp;
        atk = monster_Attack;

        searchDistance = new Vector2(30, 30);

        dir = -1;

        BaseSkill skill1 = gameObject.AddComponent<BringerOfDeath_SpellCast>();
        BaseSkill skill2 = gameObject.AddComponent<BringerOfDeath_ThunderStrike>(); 
        BaseSkill normalAttack = gameObject.AddComponent<BringerOfDeath_Attack>();

        skillList.Add(skill1);
        skillList.Add(skill2);
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
        
        spells = new List<HandOfDeath>();

        for(int i=0; i<this.transform.childCount; i++)
        {
            spells.Add(this.transform.GetChild(i).gameObject.GetComponent<HandOfDeath>());
        }

        summonNum = 1;
    }

    public override void OnDamaged(GameObject go)
    {
        base.OnDamaged(go);
        creatureFX.StartCoroutine("FlashFX"); 

    }

    void SummonSpell()
    {
        for(int i=0; i<summonNum; i++)
        {
            spells[i].StartSpell(player.transform.position.x); 
        }
    }
}
