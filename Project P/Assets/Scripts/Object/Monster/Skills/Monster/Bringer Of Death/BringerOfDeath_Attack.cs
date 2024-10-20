using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath_Attack : Normal_attack
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillName = "BringerOfDeath_Attack";

        skillCoolTime = 0f;

        skillPos = new Vector2(-2.8f, -0.745f);
        skillSize = new Vector2(3.77f, 2.15f);

        skillRange = 3f;

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
}
