using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StickmanControlState;

public class StickmanPriest : Stickman {
    public override void Fight() {
        base.Fight();
        m_StateMachine.ChangeState(StickmanChaseEnemyState.Instance);
    }
}
