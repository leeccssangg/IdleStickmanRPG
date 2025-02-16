using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickmanGlobalState : State<Stickman> {
    private static SickmanGlobalState m_Instance = null;
    public static SickmanGlobalState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new SickmanGlobalState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Stickman go ) {
    }
    public override void Execute( Stickman go ) {
        go.OnRunning();
    }
    public override void Exit( Stickman go ) {
    }
    public override bool OnMessage( Stickman go, Telegram msg ) {
        go.OnHandleGlobalMessage(msg);
        return true;
    }
}
public class StickmanWaitState : State<Stickman> {
    private static StickmanWaitState m_Instance = null;
    public static StickmanWaitState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StickmanWaitState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Stickman go ) {
        go.OnWaitStart();
    }
    public override void Execute( Stickman go ) {
        go.OnWaitExecute();
    }
    public override void Exit( Stickman go ) {
        go.OnWaitExit();
    }
    public override bool OnMessage( Stickman go, Telegram msg ) {
        switch (msg.Msg) {
            case MessageTypes.Msg_MoveToTarget:
                return true;
            case MessageTypes.Msg_Chase:
                return true;
            case MessageTypes.Msg_Follow:
                return true;
        }
        return false;
    }
}
public class StickmandleState : State<Stickman> {
    private static StickmandleState m_Instance = null;
    public static StickmandleState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StickmandleState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Stickman go ) {
        go.OnIdleStart();
    }
    public override void Execute( Stickman go ) {
        go.OnIdleExecute();
    }
    public override void Exit( Stickman go ) {
        go.OnIdleExit();
    }
    public override bool OnMessage( Stickman go, Telegram msg ) {
        return false;
    }
}
public class StickmanControlState : State<Stickman> {
    private static StickmanControlState m_Instance = null;
    public static StickmanControlState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StickmanControlState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Stickman go ) {
        go.OnControlStart();
    }
    public override void Execute( Stickman go ) {
        go.OnControlExecute();
    }
    public override void Exit( Stickman go ) {
        go.OnControlExit();
    }
    public override bool OnMessage( Stickman go, Telegram msg ) {
        return false;
    }
    public class StickmanAttackState : State<Stickman> {
        private static StickmanAttackState m_Instance = null;
        public static StickmanAttackState Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new StickmanAttackState();
                }
                return m_Instance;
            }
        }
        public override void Enter( Stickman go ) {
            go.OnAttackStart();
        }
        public override void Execute( Stickman go ) {
            go.OnAttackExecute();
        }
        public override void Exit( Stickman go ) {
            go.OnAttackExit();
        }
        public override bool OnMessage( Stickman go, Telegram msg ) {
            return false;
        }
    }
    public class StickmanChaseEnemyState : State<Stickman> {
        private static StickmanChaseEnemyState m_Instance = null;
        public static StickmanChaseEnemyState Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new StickmanChaseEnemyState();
                }
                return m_Instance;
            }
        }
        public override void Enter( Stickman go ) {
            go.OnChaseEnemyStart();
        }
        public override void Execute( Stickman go ) {
            go.OnChaseEnemyExecute();
        }
        public override void Exit( Stickman go ) {
            go.OnChaseEnemyExit();
        }
        public override bool OnMessage( Stickman go, Telegram msg ) {
            return false;
        }
    }
}
public class StickmanoDeadState : State<Stickman> {
    private static StickmanoDeadState m_Instance = null;
    public static StickmanoDeadState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StickmanoDeadState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Stickman go ) {
        go.OnDeadStart();
    }
    public override void Execute( Stickman go ) {
        go.OnDeadExecute();
    }
    public override void Exit( Stickman go ) {
        go.OnDeadExit();
    }
    public override bool OnMessage( Stickman go, Telegram msg ) {
        return false;
    }
}
public class StickmanMoveToNextCampaignRoundState : State<Stickman> {
    private static StickmanMoveToNextCampaignRoundState m_Instance = null;
    public static StickmanMoveToNextCampaignRoundState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StickmanMoveToNextCampaignRoundState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Stickman go ) {
        go.OnMoveToNextRoundStart();
    }
    public override void Execute( Stickman go ) {
        go.OnMoveToNextRoundExecute();
    }
    public override void Exit( Stickman go ) {
        go.OnMoveToNextRoundExit();
    }
    public override bool OnMessage( Stickman go, Telegram msg ) {
        return false;
    }
}
