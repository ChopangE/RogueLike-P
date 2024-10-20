using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath_HandOfDeath : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Cast_HandOfDeath"; 
        skillName = "BringerOfDeath_HandOfDeath";

        skillCoolTime = 5f;

        skillPos = new Vector2(0.1f, -0.01f);
        skillSize = new Vector2(0.83f, 0.89f);

        skillRange = 10f;

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
