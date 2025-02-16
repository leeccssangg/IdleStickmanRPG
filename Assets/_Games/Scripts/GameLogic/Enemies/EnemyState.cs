using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalState : State<Enemy> {
    private static EnemyGlobalState m_Instance = null;
    public static EnemyGlobalState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyGlobalState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
    }
    public override void Execute( Enemy go ) {
        go.OnRunning();
    }
    public override void Exit( Enemy go ) {
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        go.OnHandleGlobalMessage(msg);
        return true;
    }
}
public class EnemyControlState : State<Enemy> {
    private static EnemyControlState m_Instance = null;
    public static EnemyControlState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyControlState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnControlStart();
    }
    public override void Execute( Enemy go ) {
        go.OnControlExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnControlExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        //switch (msg.Msg) {
        //}
        return false;
    }
}
public class EnemyWaitState : State<Enemy> {
    private static EnemyWaitState m_Instance = null;
    public static EnemyWaitState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyWaitState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnWaitStart();
    }
    public override void Execute( Enemy go ) {
        go.OnWaitExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnWaitExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
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
public class EnemyIdleState : State<Enemy> {
    private static EnemyIdleState m_Instance = null;
    public static EnemyIdleState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyIdleState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnIdleStart();
    }
    public override void Execute( Enemy go ) {
        go.OnIdleExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnIdleExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        return false;
    }
}
public class EnemyChasePlayerState : State<Enemy> {
    private static EnemyChasePlayerState m_Instance = null;
    public static EnemyChasePlayerState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyChasePlayerState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnChaseStart();
    }
    public override void Execute( Enemy go ) {
        go.OnChaseExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnChaseExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        return false;
    }
}
public class EnemyAttackPlayerState : State<Enemy> {
    private static EnemyAttackPlayerState m_Instance = null;
    public static EnemyAttackPlayerState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyAttackPlayerState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnAttackStart();
    }
    public override void Execute( Enemy go ) {
        go.OnAttackExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnAttackExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        return false;
    }
}
public class EnemyDyingStateState : State<Enemy> {
    private static EnemyDyingStateState m_Instance = null;
    public static EnemyDyingStateState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyDyingStateState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnDyingStart();
    }
    public override void Execute( Enemy go ) {
        go.OnDyingExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnDyingExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        return false;
    }
}
public class EnemyDeadState : State<Enemy> {
    private static EnemyDeadState m_Instance = null;
    public static EnemyDeadState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new EnemyDeadState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Enemy go ) {
        go.OnDeadStart();
    }
    public override void Execute( Enemy go ) {
        go.OnDeadExecute();
    }
    public override void Exit( Enemy go ) {
        go.OnDeadExit();
    }
    public override bool OnMessage( Enemy go, Telegram msg ) {
        return false;
    }
}