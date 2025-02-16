using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGolbalState : State<Skill> {
    private static SkillGolbalState m_Instance = null;
    public static SkillGolbalState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new SkillGolbalState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Skill go ) {
    }
    public override void Execute( Skill go ) {
        go.OnRunning();
    }
    public override void Exit( Skill go ) {
    }
    public override bool OnMessage( Skill go, Telegram msg ) {
        return true;
    }
}
public class SkillCooldownState : State<Skill> {
    private static SkillCooldownState m_Instance = null;
    public static SkillCooldownState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new SkillCooldownState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Skill go ) {
        go.OnCooldownStart();
    }
    public override void Execute( Skill go ) {
        go.OnCooldownExecute();
    }
    public override void Exit( Skill go ) {
        go.OnCooldownExit();
    }
    public override bool OnMessage( Skill go, Telegram msg ) {
        return true;
    }
}
public class SkillAttackState : State<Skill> {
    private static SkillAttackState m_Instance = null;
    public static SkillAttackState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new SkillAttackState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Skill go ) {
        go.OnAttackStart();
    }
    public override void Execute( Skill go ) {
        go.OnAttackExecute();
    }
    public override void Exit( Skill go ) {
        go.OnAttackExit();
    }
    public override bool OnMessage( Skill go, Telegram msg ) {
        return true;
    }
}
public class SkillWaitState : State<Skill> {
    private static SkillWaitState m_Instance = null;
    public static SkillWaitState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new SkillWaitState();
            }
            return m_Instance;
        }
    }
    public override void Enter( Skill go ) {
        go.OnWaitStart();
    }
    public override void Execute( Skill go ) {
        go.OnWaitExecute();
    }
    public override void Exit( Skill go ) {
        go.OnWaitExit();
    }
    public override bool OnMessage( Skill go, Telegram msg ) {
        return true;
    }
}