using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGlobalState : State<Bullet>{
    private static BulletGlobalState m_Instance = null;

    public static BulletGlobalState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletGlobalState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){ }
    public override void Execute(Bullet go){
        go.OnRunning();
    }
    public override void Exit(Bullet go){ }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletWaitingState : State<Bullet>{
    private static BulletWaitingState m_Instance = null;

    public static BulletWaitingState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletWaitingState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnWaitingStart();
    }
    public override void Execute(Bullet go){
        go.OnWaitingExecute();
    }
    public override void Exit(Bullet go){
        go.OnWatingExit();
    }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletFireForwardState : State<Bullet>{
    private static BulletFireForwardState m_Instance = null;

    public static BulletFireForwardState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletFireForwardState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnStartFireToward();
    }
    public override void Execute(Bullet go){
        go.OnFiringToward();
    }
    public override void Exit(Bullet go){ }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletFireBoomerangState : State<Bullet>{
    private static BulletFireBoomerangState m_Instance = null;

    public static BulletFireBoomerangState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletFireBoomerangState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnStartFireBoomerang();
    }
    public override void Execute(Bullet go){
        go.OnExecuteFireBoomerang();
    }
    public override void Exit(Bullet go){ }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletFollowTargetState : State<Bullet>{
    private static BulletFollowTargetState m_Instance = null;

    public static BulletFollowTargetState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletFollowTargetState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnFollowTargetStart();
    }
    public override void Execute(Bullet go){
        go.OnFollowTargetExecute();
    }
    public override void Exit(Bullet go){ }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletBoomState : State<Bullet>{
    private static BulletBoomState m_Instance = null;

    public static BulletBoomState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletBoomState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnBoomStart();
    }
    public override void Execute(Bullet go){
        go.OnBoomExecute();
    }
    public override void Exit(Bullet go){
        go.OnBoomExit();
    }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletChaseTargetState : State<Bullet>{
    private static BulletChaseTargetState m_Instance = null;

    public static BulletChaseTargetState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletChaseTargetState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnChaseStart();
    }
    public override void Execute(Bullet go){
        go.OnChaseExecute();
    }
    public override void Exit(Bullet go){
        go.OnChaseExit();
    }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletMissileState : State<Bullet>{
    private static BulletMissileState m_Instance = null;

    public static BulletMissileState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletMissileState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnMissileStart();
    }
    public override void Execute(Bullet go){
        go.OnMissileExecute();
    }
    public override void Exit(Bullet go){
        go.OnMissileExit();
    }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}
public class BulletTweenState : State<Bullet>{
    private static BulletTweenState m_Instance = null;

    public static BulletTweenState Instance{
        get {
            if(m_Instance == null){
                m_Instance = new BulletTweenState();
            }

            return m_Instance;
        }
    }

    public override void Enter(Bullet go){
        go.OnTweenFireStart();
    }
    public override void Execute(Bullet go){
        go.OnTweenFireExecute();
    }
    public override void Exit(Bullet go){
        go.OnTweenFireExit();
    }
    public override bool OnMessage(Bullet go, Telegram msg){
        return false;
    }
}