using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FlyingEye_Attack : Normal_attack
{    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Attack"; 
        skillName = "FlyingEye_Attack";

        skillCoolTime = 0f;

        skillPos = new Vector2(0.1f, -0.01f);
        skillSize = new Vector2(0.83f, 0.89f);

        skillRange = 0.5f;

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
