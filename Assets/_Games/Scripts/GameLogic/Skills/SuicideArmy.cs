using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideArmy : Bullet
{
    public IngameObject m_TargetAlive;
    public override void InitBullet( BigNumber damage, IngameTeam targetTeam, IngameTeam ownerTeam, string spriteName ) {
        base.InitBullet(damage, targetTeam, ownerTeam, spriteName);
        m_LifeTime = 10;
    }
    public override void OnFollowTargetStart() {
        base.OnFollowTargetStart();
        Speed = m_MaxSpeed;
    }
    public override void OnFollowTargetExecute() {
        if(m_TargetAlive == null || m_TargetAlive.IsDead()) {
            m_TargetAlive = GetTarget();
        }
        if(m_TargetAlive == null) {
            m_StateMachine.ChangeState(BulletWaitingState.Instance);
            return;
        }
        Transform.position += MoveToTarget(m_TargetAlive.Transform.position);
    }
    public override void OnWaitingStart() {
        base.OnWaitingStart();
        Speed = m_MaxSpeed /2;
        m_Direction = Transform.right;
    }
    public override void OnWaitingExecute() {
        base.OnWaitingExecute();
        m_TargetAlive = GetTarget();
        if(m_TargetAlive != null) {
            m_StateMachine.ChangeState(BulletFollowTargetState.Instance);
            return;
        }
        OnFiringToward();
    }

    public IngameObject GetTarget() {
        return IngameEntityManager.Ins.GetNearestCharacterTeam2(Transform.position);
    }
}
