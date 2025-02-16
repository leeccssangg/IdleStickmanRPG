using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_18 : Skill
{
    protected override void SetFirePos(){
        m_FirePos.position = m_Character.Transform.position;
    }
}
