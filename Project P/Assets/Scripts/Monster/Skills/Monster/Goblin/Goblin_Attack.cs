using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Attack : Normal_attack
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillName = "Goblin_Attack";

        skillCoolTime = 0f;

        skillPos = new Vector2(0.25f, -0.144f);
        skillSize = new Vector2(1.76f, 1.09f);

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
