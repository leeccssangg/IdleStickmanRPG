using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StickmanControlState;

public class StickmanArcher : Stickman{
    public override void InitStickman(){
        base.InitStickman();
        m_Rigidbody.gravityScale = 1;
    }
    public override void Fight(){
        base.Fight();
        m_StateMachine.ChangeState(StickmanChaseEnemyState.Instance);
    }
}