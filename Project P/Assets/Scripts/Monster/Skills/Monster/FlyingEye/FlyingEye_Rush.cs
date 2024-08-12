using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye_Rush : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Rush";
        skillName = "FlyingEye_Rush"; 

        skillCoolTime = 5f;

        skillPos = new Vector2(0.2f, 0.003f);
        skillSize = new Vector2(1.2f, 1.17f);
        skillRange = 5f;

        clip = FindAnimationClip(skillName);

        if (clip == null)
        {
            Debug.Log($"No such {clip.name} clip in the animator");
            return;
        }

        float delay = clip.length;

        MakeEvent(); 
        SetEvent("EndAttack", delay);
        AddEvent(clip);
    }

}
