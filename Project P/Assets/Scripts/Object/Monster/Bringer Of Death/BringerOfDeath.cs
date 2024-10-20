using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BringerOfDeath : Monster
{
    [SerializeField]
    string monsterName;

    [Header("# Monster Collider")]
    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 size;

    [Header("# Monster Status")]
    [SerializeField] float monster_Hp;
    [SerializeField] float monster_Attack;

    [SerializeField] public Image healthBarFill;

    public List<HandOfDeath> hands;
    public List<Meteo> meteos; 

    public int handNum;
    public int meteoNum;

    public GameObject handPrefab;
    public GameObject meteoPrefab;

    public GameObject chargingEffect;
    public GameObject collisionEffect;

    public bool bossReady; 

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
        currentHp = hp; 

        atk = monster_Attack;

        searchDistance = new Vector2(30, 10);

        dir = -1;

        BaseSkill skill1 = gameObject.AddComponent<BringerOfDeath_PowerStrike>();
        BaseSkill skill2 = gameObject.AddComponent<BringerOfDeath_HandOfDeath>();
        BaseSkill skill3 = gameObject.AddComponent<BringerOfDeath_MeteoStrike>();
        BaseSkill normalAttack = gameObject.AddComponent<BringerOfDeath_Attack>();

        skillList.Add(skill1);
        skillList.Add(skill2);
        skillList.Add(skill3); 
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

        handNum = 1;
        meteoNum = 3;

        selectedSkill = normalAttack;

        bossReady = false; 
    }

    protected override void Update()
    {
        if (!bossReady)
            return;

        base.Update();
    }

    public void UpdateBossReady(bool value)
    {
        bossReady = value; 
    }

    protected override void UpdateDeath()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
            return; 

        base.UpdateDeath();
    }

    public override void OnDamaged(GameObject go)
    {
        base.OnDamaged(go);

        creatureFX.StartCoroutine("FlashFX");

        healthBarFill.fillAmount = currentHp/hp; 


        if(currentHp <= 0)
        {
            CreatureState = ECreatureState.Death;
            return; 
        }

    }

    void SummonSpell()
    {
        for(int i=0; i<handNum; i++)
        {
            GameObject go = Manager.Pool.Pop(handPrefab);

            if (go == null)
                return; 

            go.GetComponent<HandOfDeath>().StartSpell(player.transform.position.x, this.transform.position.y + 3.86f);
        }
    }

    void SummonMeteo()
    {
        for (int j=0; j<meteoNum; j++)
        {
            GameObject go = Manager.Pool.Pop(meteoPrefab);

            if (go.GetComponent<Meteo>() == null)
                return; 

            go.GetComponent<Meteo>().StartMeteo(this.transform.position.x + 5 * j, this.transform.position.y + 7f); ;
        }
    }

}
