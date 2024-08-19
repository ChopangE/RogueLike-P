using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Bite : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Bite"; 
        skillName = "Mushroom_Bite";

        skillCoolTime = 3f;

        skillPos = new Vector2(1.27f, -0.328f);
        skillSize = new Vector2(1.54f, 1.25f);

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
