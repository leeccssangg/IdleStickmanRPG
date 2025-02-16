using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Character m_Character;
    public AttackType m_AttackType = AttackType.NORMAL_ATTACK;
    public void Hit() {
        m_Character.Attack();
    }
    public void Skill() {
        m_Character.Attack();
    }
    public void End() {
        m_Character.AttackDone();
    }
    public void Fade_In() {
        m_Character.m_Rigidbody.gravityScale = 1;
       // m_Character.FadeOut();
    }
}
