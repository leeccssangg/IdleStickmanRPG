using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGlobalState : State<MainCharacter> {
    private static CharacterGlobalState m_Instance = null;
    public static CharacterGlobalState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterGlobalState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
    }
    public override void Execute( MainCharacter go ) {
        go.OnRunning();
    }
    public override void Exit( MainCharacter go ) {
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        go.OnHandleGlobalMessage(msg);
        return true;
    }
}
public class CharacterControlState : State<MainCharacter> {
    private static CharacterControlState m_Instance = null;
    public static CharacterControlState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterControlState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnControlStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnControlExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnControlExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        //switch (msg.Msg) {
        //}
        return false;
    }
}
public class CharacterWaitState : State<MainCharacter> {
    private static CharacterWaitState m_Instance = null;
    public static CharacterWaitState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterWaitState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnWaitStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnWaitExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnWaitExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
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
public class CharacterIdleState : State<MainCharacter> {
    private static CharacterIdleState m_Instance = null;
    public static CharacterIdleState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterIdleState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnIdleStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnIdleExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnIdleExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        return false;
    }
}
public class CharacterChaseEnemyState : State<MainCharacter> {
    private static CharacterChaseEnemyState m_Instance = null;
    public static CharacterChaseEnemyState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterChaseEnemyState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnChaseStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnChaseExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnChaseExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        return false;
    }
}
public class CharacterAttackEnemyState : State<MainCharacter> {
    private static CharacterAttackEnemyState m_Instance = null;
    public static CharacterAttackEnemyState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterAttackEnemyState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnAttackStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnAttackExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnAttackExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        return false;
    }
}
public class CharacterDyingStateState : State<MainCharacter> {
    private static CharacterDyingStateState m_Instance = null;
    public static CharacterDyingStateState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterDyingStateState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnDyingStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnDyingExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnDyingExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        return false;
    }
}
public class CharacterDeadState : State<MainCharacter> {
    private static CharacterDeadState m_Instance = null;
    public static CharacterDeadState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new CharacterDeadState();
            }
            return m_Instance;
        }
    }
    public override void Enter( MainCharacter go ) {
        go.OnDeadStart();
    }
    public override void Execute( MainCharacter go ) {
        go.OnDeadExecute();
    }
    public override void Exit( MainCharacter go ) {
        go.OnDeadExit();
    }
    public override bool OnMessage( MainCharacter go, Telegram msg ) {
        return false;
    }
}
public class CharacterMoveToNextRound :State<MainCharacter> {
    private static CharacterMoveToNextRound m_Instance = null;
    public static CharacterMoveToNextRound Instance {
        get {
            if(m_Instance == null) {
                m_Instance = new CharacterMoveToNextRound();
            }
            return m_Instance;
        }
    }
    public override void Enter(MainCharacter go) {
        go.OnMoveToNextRoundStart();
    }
    public override void Execute(MainCharacter go) {
        go.OnMoveToNextRoundExecute();
    }
    public override void Exit(MainCharacter go) {
        go.OnMoveToNextRoundExit();
    }
    public override bool OnMessage(MainCharacter go,Telegram msg) {
        return false;
    }
}
