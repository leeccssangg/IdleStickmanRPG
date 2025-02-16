using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_7 : Skill{
    protected override void Attack() {
        DoStartAttackEvent();
        HeroManager.Ins.SetBonusAttackSpeed(50);
    }
    public override void OnAttackExit(){
        base.OnAttackExit();
        HeroManager.Ins.SetBonusAttackSpeed(0);
    }
    public override float GetAttackTime(){
        return 5;
    }
}
