using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_12 : Skill{
    protected override void SetFirePos(){
        m_FirePos.position = new Vector3(m_Character.Transform.position.x - 2.5f, 0.3f, 0);
    }

    protected override Vector3 GetFireDirection(){
        return m_FirePos.right + new Vector3(0, 0.1f,0f);
    }
}