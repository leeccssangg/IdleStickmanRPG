using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_19 : Skill
{
    protected override void SetFirePos(){
        m_FirePos.position = m_Character.Transform.position + Vector3.right * -2.5f;
    }
}
