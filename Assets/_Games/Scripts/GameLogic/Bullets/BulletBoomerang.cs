using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoomerang : Bullet
{
    public SkeletonAnimation m_SkeletonAnimation;
    public void SetTypeBullet( AttackType attackType ) {
        if (m_SkeletonAnimation != null) {
            string animationName = "Boomerang 1";
            if (attackType == AttackType.NORMAL_ATTACK) {
                animationName = "Boomerang 1";
            } else if (attackType == AttackType.SKILL_ATTACK) {
                animationName = "Boomerang 2";
            }
            m_SkeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        }
    }
    public override void OnExecuteTriggerEnter2D( Collider2D collider ) {
        base.OnExecuteTriggerEnter2D(collider);
        if (IsCommingBack && collider.gameObject.tag == m_OwnerTeam.ToString()) {
            Creature hero = collider.attachedRigidbody.gameObject.GetComponent<Creature>();
            int heroID = hero.ID;
            if (m_OwnerID == heroID) {
                ForceExecute();
            }
        }
    }
    public override bool IsOutCamera(){
        return false;
    }
}
