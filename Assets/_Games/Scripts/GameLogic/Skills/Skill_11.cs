using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_11 : Skill{
    protected override void SetFirePos(){
        m_FirePos.position = new Vector3(m_Character.Transform.position.x - 2.5f, 3f, 0);
    }
}