using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Slash : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Slash"; 
        skillName = "Goblin_Slash";
        
        skillCoolTime = 6f;

        skillPos = new Vector2(1.43f, -0.33f);
        skillSize = new Vector2(1.34f, 0.89f);

        skillRange = 1.5f;

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
