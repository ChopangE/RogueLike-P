using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Attack : Normal_attack
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillName = "Mushroom_Attack";

        skillCoolTime = 0f;

        skillPos = new Vector2(0.86f, -0.23f);
        skillSize = new Vector2(1.3f, 1.5f);

        skillRange = 1f;

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
