using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Normal_attack : BaseSkill
{
    public override void SetInfo(Creature owner)
    {
        base.SetInfo(owner);

        skillState = "Attack"; 
    }

    public override void DoSkill()
    {
        base.DoSkill();
    }

}
