using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath_MeteoStrike : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Cast_MeteoStrike";
        skillName = "BringerOfDeath_MeteoStrike";

        skillCoolTime = 10f;

        skillPos = new Vector2(0f, 0f);
        skillSize = new Vector2(0f, 0f);

        skillRange = 10f;

        clip = FindAnimationClip(skillName);

        if (clip == null)
        {
            Debug.Log($"No such {"skillName"} clip in the animator");
            return;
        }

        float delay = clip.length;

        MakeEvent(); 
        SetEvent("EndAttack", delay);
        AddEvent(clip);
    }

}
