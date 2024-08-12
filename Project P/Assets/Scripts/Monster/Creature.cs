using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : BaseObject
{
    public enum ECreatureState
    {
        Idle,
        Move,
        Attack,
        Death
    }

    [SerializeField] 
    protected ECreatureState _creatureState;
    protected virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
                UpdateAnimation();
            }
        }
    }

    protected CreatureFX creatureFX; 

    protected float hp { set; get; }
    protected float atk { set; get; }
    protected float speed { set; get; }


    [field: SerializeField]
    protected List<BaseSkill> skillList;
    [field: SerializeField]
    protected List<BaseSkill> enableSkillList;
    public BaseSkill EnableSkill
    {
        set
        {
            enableSkillList.Add(value);
            return;
        }
    }

    public BaseSkill UnableSkill
    {
        set 
        { 
            enableSkillList.Remove(value);
            return; 
        }
    }

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init(); 
        creatureFX = this.gameObject.GetComponent<CreatureFX>(); 
    }

    protected override void SetInfo(string animatorName)
    {
        base.SetInfo(animatorName); 
        _creatureState = ECreatureState.Idle;
    }

    protected virtual void UpdateAnimation()
    {

    }

    protected virtual void UpdateIdle()
    {

    }

    protected virtual void UpdateMove()
    {

    }

    protected virtual void UpdateAttack()
    {

    }

    protected virtual void UpdateDeath()
    {

    }

    protected virtual void OnHit()
    {

    }

    public virtual void OnDamaged(GameObject go)
    {

    }

}
