using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_16 : Skill{
    public ParticleSystem m_Effect;

    protected override void Attack(){
        base.Attack();
        m_Effect.transform.position = m_FirePos.position + Vector3.up * 0.7f;
        m_Effect.Play();
    }
}