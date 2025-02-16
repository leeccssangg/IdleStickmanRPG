using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_20 : Skill {
    protected override void SetFirePos() {
        m_FirePos.position = IngameManager.Ins.MainCharacter.transform.position + Vector3.right * -2;

    }
}
 