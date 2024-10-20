using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    #region Creature
    protected Creature Owner;
    #endregion 

    #region AnimationInfo
    protected Animator anim;
    protected AnimationClip clip;
    protected AnimationEvent evt;

    protected string skillState; // Animator State name 
    protected string skillName; // Animator Clip name 
    #endregion

    #region Skill
    protected float skillCoolTime;
    [SerializeField]
    protected float skillRange;

    public Vector2 skillPos; 
    public Vector2 skillSize;

    protected bool skillEnable;
    #endregion 

    private void Awake()
    {
        Init(); 
    }

    void Init()
    {
        skillEnable = true;
    }

    public virtual void SetInfo(Creature owner)
    {
        Owner = owner;
        anim = owner.gameObject.GetComponent<Animator>();
    }

    #region Animation Clip

    protected AnimationClip FindAnimationClip(string _clipName)
    {
        // Find Animation clip in the creature's animator 
        foreach(AnimationClip _clip in anim.runtimeAnimatorController.animationClips)
        {
            if(_clip.name == _clipName)
                return _clip; 
        }

        return null; 
    }

    public AnimationClip GetSkillAnimationClip()
    {
        // Return the skill clip 
        return clip;
    }

    #endregion

    #region Event 

    protected void MakeEvent()
    {
        evt = new AnimationEvent();
    }

    protected void SetEvent(string functionName, float time)
    {
        if (evt == null)
            return; 

        evt.functionName = functionName;
        evt.time = time; 
    }

    protected void AddEvent(AnimationClip clip)
    {
        clip.AddEvent(evt); 
    }

    #endregion

    #region Skill
    public virtual void DoSkill()
    {
        Owner.PlayAnimation(skillState);

        if (this.skillCoolTime > 0)
            Owner.UnableSkill = this; 

        StartCoroutine(CoWaitCoolTime(skillCoolTime));
    }
    #endregion

    #region Coroutine
    IEnumerator CoWaitCoolTime(float seconds)
    {
        if (seconds <= 0)
            yield return null;

        yield return new WaitForSeconds(seconds);
        skillEnable = true;

        if(this.skillCoolTime > 0)
            Owner.EnableSkill = this; 
    }
    #endregion 

    #region Check 
    public bool IsSkillEnable()
    {
        return skillEnable;
    }

    public bool IsSkillReachable(float distance)
    {
        return (distance <= skillRange);
    }

    #endregion 
}
