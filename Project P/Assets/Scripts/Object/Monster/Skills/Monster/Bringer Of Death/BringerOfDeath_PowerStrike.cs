using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BringerOfDeath_PowerStrike : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Power Strike";
        skillName = "BringerOfDeath_PowerStrike";

        skillCoolTime = 5f;

        skillPos = new Vector2(-4.3f, -0.4f);
        skillSize = new Vector2(4.18f, 4.65f);

        skillRange = 5f;

        clip = FindAnimationClip(skillName);

        if (clip == null)
        {
            Debug.Log("No such clip in the animator");
            return;
        }

        float delay = clip.length;

        MakeEvent();
        SetEvent("EndAttack", delay);
        AddEvent(clip);
    }

    void StartEffect()
    {
        GameObject go = Owner.GetComponent<BringerOfDeath>().chargingEffect;
        GameObject _go = Manager.Pool.Pop(go);
        _go.transform.position = Owner.gameObject.transform.position + new Vector3(transform.localScale.x * 1.15f, 2f, 0f);
    }
}
