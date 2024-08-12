using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Attack : Normal_attack
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillName = "Skeleton_Attack"; 

        skillCoolTime = 0f;

        skillPos = new Vector2(1.26f, 0.63f);
        skillSize = new Vector2(2, 2);
        skillRange = 2f;

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
