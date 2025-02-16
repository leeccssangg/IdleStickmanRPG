using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { CREEP = 0, MEDIUM = 1, TOWER = 2, TRAP = 3, BOSS = 4, BIG_BOSS = 5 }
public class Enemy:Character {
    public int m_EnemyID;
    public int m_World;

    public EnemyType m_EnemyType;
    public BigNumber m_Gold;

    private float m_AttackRangeOffset;
    protected float m_DeadTimer = 2;
    protected float m_DyingTime;

    public StateMachine<Enemy> StateMachine => m_StateMachine;
    protected StateMachine<Enemy> m_StateMachine;
    public string m_CurrentState = "";
    private void Update() {
        m_StateMachine.Update();
        m_CurrentState = m_StateMachine.CurrentState.ToString();
    }
    public override void Init() {
        base.Init();
        InitStateMachine();
    }
    public virtual void InitEnemy() {
        m_OnDamagedCallback = null;
        m_OnDamagedCallback = null;
        m_Collider.enabled = true;
        RegisterInScene();
        InitWeapon(null);
        InitHealthbar();
    }
    protected virtual void InitStateMachine() {
        m_StateMachine = new StateMachine<Enemy>(this);
        m_StateMachine.SetCurrentState(EnemyWaitState.Instance);
        m_StateMachine.SetGlobalState(EnemyGlobalState.Instance);
    }
    protected virtual void InitWeapon(List<WeaponDataConfig> weaponConfigData) {
        m_NormalAttacks.Clear();
        //m_SkillAttacks.Clear();
        for(int i = 0;i < m_Weapons.Count;i++) {
            Weapon wp = m_Weapons[i];
            //WeaponDataConfig wpData = weaponConfigData[i];
            //wp.m_WeaponDataConfig = wpData;
            switch(wp.attackType) {
                case AttackType.NORMAL_ATTACK: {
                    wp.SetCurrentReloadTime(0.01f);
                    m_NormalAttacks.Add(wp);
                }
                break;
                case AttackType.SKILL_ATTACK: {
                    m_SkillAttacks.Add(wp);
                }
                break;
            }
        }
        InitNormalWeapon();
    }
    protected override void InitNormalWeapon(){
        for (int i = 0; i < m_NormalAttacks.Count; i++) {
            Weapon wp = m_NormalAttacks[i];
            //wp.Init(wp.m_WeaponDataConfig.id, m_Level, m_Rank, this);
            wp.InitWeapon(null,IngameType.PLAYER,this);
            //wp.SetCurrentReloadTime(0.01f);
        }
    }
    public override void BattleCry() {
        base.BattleCry();
        StateMachine.ChangeState(EnemyWaitState.Instance);
    }
    public override void Fight() {
        base.Fight();
        StateMachine.ChangeState(EnemyChasePlayerState.Instance);
    }
    public override void OnDamage(BigNumber damage,DamageInfo damageInfo) {
        base.OnDamage(damage,damageInfo);
        SetTriggerAnimator(ConstantString.ANIM_GETHIT);
    }
    public void SetDamage(BigNumber damage) {
        for(int i = 0;i < m_Weapons.Count;i++) {
            m_Weapons[i].SetDamage(damage);
        }
    }
    public void InitHP(BigNumber hp) {
        m_HP = m_MaxHP = hp;
    }
    public void WaitState(){
        if(IsDead()) return;
        StateMachine.ChangeState(EnemyWaitState.Instance);
    }
    protected override void FindingTarget() {
        if(m_Target == null || m_Target.IsDead()) {
            m_Target = IngameManager.Ins.MainCharacter;
        }
    }
    public override void Dead() {
        base.Dead();
        m_OnDamagedCallback?.Invoke();
        SetTriggerAnimator(ConstantString.ANIM_DIE);
        Vector3 v = Transform.position;
        IngameManager.Ins.DropGoldFromEnemy(100,v);
        StateMachine.ChangeState(EnemyDyingStateState.Instance);
        IngameManager.Ins.MarkKillCharacter(m_IngameTeam);
    }
    public override void Deactive() {
        base.Deactive();
        if(m_HealthBar != null) {
            m_HealthBar.Despawn();
            m_HealthBar = null;
        }
        // SimplePool.Despawn(this.gameObject);
    }
    public virtual void OnHandleGlobalMessage(Telegram msg) { }
    /// <summary>
    /// For the control state
    /// </summary>
    public virtual void OnControlStart() { }
    public virtual void OnControlExecute() { }
    public virtual void OnControlExit() { }
    /// <summary>
    /// For the Wait State
    /// </summary>
    public virtual void OnWaitStart() {
        SetTriggerAnimator(ConstantString.ANIM_IDLE);
    }
    public virtual void OnWaitExecute() { }
    public virtual void OnWaitExit() { }
    /// <summary>
    /// For the Idle state
    /// </summary>
    public virtual void OnIdleStart() { }
    public virtual void OnIdleExecute() { }
    public virtual void OnIdleExit() { }
    /// <summary>
    /// For the chase state
    /// </summary>
    public virtual void OnChaseStart() {
        SetTriggerAnimator(ConstantString.ANIM_IDLE);
        m_Target = IngameManager.Ins.MainCharacter;
        m_AttackRangeOffset = Random.Range(-0.2f,0.8f);
    }
    public virtual void OnChaseExecute() {
        if(Target == null || Target.IsDead()) {
            m_Target = IngameManager.Ins.MainCharacter;
            m_AttackRangeOffset = Random.Range(0,1f);
        }
        if(Target == null || Target.IsDead()) return;
        Vector3 targetPos = Target.Transform.position;
        targetPos.y = Transform.position.y;
        Vector3 dir = (targetPos - Transform.position).normalized;
        Vector3 pos = Transform.position + dir * 1.5f /*m_Movement.GetMoveSpeed()*/ * Time.deltaTime;
        transform.position = pos;

        m_NextAttackInfo = GetNextAttackInfo();
        float attackRange = 1;
        if(m_NextAttackInfo.attackType != AttackType.NONE) {
            attackRange = GetNextAttackRange(m_NextAttackInfo);
        } else {
            attackRange = GetNormalAttackRange();
        }
        attackRange += m_AttackRangeOffset;
        bool inAttackRange = IsInRange(Transform.position,targetPos,attackRange);
        if(inAttackRange) {
            StateMachine.ChangeState(EnemyAttackPlayerState.Instance);
            return;
        }
        m_Animator.SetBool(animatorIsRunningHash,true);

    }
    public virtual void OnChaseExit() {
        if(!IsDead()) {
            m_Animator.SetBool(animatorIsRunningHash,false);
        }
    }
    /// <summary>
    /// For the attack basic State
    /// </summary>
    public virtual void OnAttackStart() { }
    public virtual void OnAttackExecute() {
        if(m_IsAttacking)
            return;
        if(m_Target == null || m_Target.IsDead()) {
            StateMachine.ChangeState(EnemyChasePlayerState.Instance);
            return;
        }
        UpdateFacingToTarget();
        m_NextAttackInfo = GetNextAttackInfo();
        if(m_NextAttackInfo.attackType == AttackType.NONE)
            return;
        float attackRange = GetNextAttackRange(m_NextAttackInfo);
        attackRange += m_AttackRangeOffset;
        if(!IsInRange(Transform.position,m_Target.Transform.position,attackRange)) {
            StateMachine.ChangeState(EnemyChasePlayerState.Instance);
            return;
        }
        if(IsGoodToAttack()) {
            StartAttack(m_NextAttackInfo);
        }
    }
    public virtual void OnAttackExit() { }

    /// <summary>
    /// For the Dying state
    /// </summary>
    public virtual void OnDyingStart() { }
    public virtual void OnDyingExecute() {
        m_DyingTime -= Time.deltaTime;
        if(m_DyingTime <= 0) {
            StateMachine.ChangeState(EnemyDeadState.Instance);
            return;
        }
    }
    public virtual void OnDyingExit() { }
    /// <summary>
    /// For the dead state
    /// </summary>
    public virtual void OnDeadStart() {
        m_DeadTimer = 1f;
        m_Rigidbody.AddForce(new Vector2(1.5f,1.5f) * UnityEngine.Random.Range(100, 100), ForceMode2D.Force);
    }
    public virtual void OnDeadExecute() {
        m_DeadTimer -= Time.deltaTime;
        if(m_DeadTimer <= 0) {
            Deactive();
            Reset();
        }
    }
    public virtual void OnDeadExit() { }

    //===================================================================//
    protected override void UpdateFacingToTarget() {
        base.UpdateFacingToTarget();
        if(m_Target == null)
            return;
        if(m_Target.Transform.position.x > Transform.position.x) {
            Transform.localEulerAngles = new Vector3(0,180,0);
        } else {
            Transform.localEulerAngles = new Vector3(0,0,0);
        }
    }
    protected override void StartAttack(AttackInfo attackInfo) {
        base.StartAttack(attackInfo);
        Weapon weapon = GetWeaponAttack(attackInfo);
        weapon.StartAttack();
        SetTriggerAnimator(weapon.GetTriggerName());
    }
    public override void Reset() {
        base.Reset();
        m_StateMachine.SetCurrentState(EnemyWaitState.Instance);
    }
    public override bool IsOutCamera(){
        var screenPoint = CameraManger.Ins.ScreenPoint(Transform.position);
        bool onScreen = screenPoint is{
            x: < 0.9f,
        };
        return !onScreen;
    }
}
