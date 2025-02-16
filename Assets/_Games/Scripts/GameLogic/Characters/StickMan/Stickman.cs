using Spine.Unity;
using UnityEngine;
using static StickmanControlState;

public class Stickman : Character{
    public int SlotID{ get; set; }
    protected Transform m_FollowTransform;
    public string m_CurrentState = "";
    public StateMachine<Stickman> Statemachine => m_StateMachine;
    public Transform FollowTransform{ get => m_FollowTransform; set => m_FollowTransform = value; }
    public int AttackCount { get; set; }

    protected StateMachine<Stickman> m_StateMachine;
    private SkeletonMecanim m_SkeletonMecanim;
    private StickmanInfo m_StickmanInfo;
    private float m_AttackRangeOffset;

    public void Update(){
        m_StateMachine.Update();
        m_CurrentState = m_StateMachine.CurrentState.ToString();
    }
    public override void Init(){
        base.Init();
        InitStateMachine();
    }
    protected void InitStateMachine(){
        m_StateMachine = new StateMachine<Stickman>(this);
        m_StateMachine.SetCurrentState(StickmanWaitState.Instance);
        m_StateMachine.SetGlobalState(SickmanGlobalState.Instance);
    }
    public virtual void InitStickman(){
        RegisterInScene();
        UpdateStickmanStat();
        m_IsAttacking = false;
        IsHoldingAttack = false;
        m_TargetType = IngameType.ENEMY;
        InitAnimator();
        m_OnDamagedCallback = null;
        m_OnDeadCallback = null;
        InitWeapon();
        SetSkinStickman();
        Reset();
        FaceRight();
        Fight();
    }
    public void InitStickman(StickmanInfo info){
        m_StickmanInfo = info;
        RegisterInScene();
        Reset();
        InitAnimator();
        SetSkinStickman();
        InitMovement();
        InitWeapon();
        FaceRight();
    }
    protected void InitMovement(){
        m_Movement.maxSpeed = 4.5f;
        m_Movement.minSpeed = 4.5f;
        m_Movement.moveSpeed = 4.5f;
    }
    private void SetSkinStickman(){
        if(m_SkeletonMecanim.skeleton == null){
            m_SkeletonMecanim.Initialize(false);
        }

        int skinID = Mathf.Clamp((int)m_StickmanInfo.Rarity, 1, 5);
        if(m_SkeletonMecanim.skeleton == null) return;
        m_SkeletonMecanim.skeleton.SetSkin(skinID.ToString());
        m_SkeletonMecanim.skeleton.SetSlotsToSetupPose();
    }
    public virtual void UpdateStickmanStat(){
        UpdateWeaponStat();
    }
    public void InitAnimator(){
        m_CharacterAnimator = m_Animator.GetComponent<CharacterAnimator>();
        m_SkeletonMecanim = m_Animator.GetComponent<SkeletonMecanim>();
    }
    public void InitWeapon(){
        m_NormalAttacks.Clear();
        m_SkillAttacks.Clear();
        for(int i = 0; i < m_Weapons.Count; i++){
            Weapon wp = m_Weapons[i];
            wp.InitWeapon(this);
            if(wp.attackType == AttackType.NORMAL_ATTACK){
                m_NormalAttacks.Add(wp);
                wp.SetRateOfFire(m_StickmanInfo.GetAttackSpeed());
            } else if(wp.attackType == AttackType.SKILL_ATTACK){
                m_SkillAttacks.Add(wp);
                wp.SetRateOfFire(5);
            }
        }

        UpdateWeaponStat();
    }
    public override void UpdateWeaponStat(){
        for(int i = 0; i < m_Weapons.Count; i++){
            Weapon wp = m_Weapons[i];
            wp.SetDamage(m_StickmanInfo.GetAttackStat());
        }
    }
    public override void Fight(){
        base.Fight();
        m_StateMachine.SetCurrentState(StickmanChaseEnemyState.Instance);
    }

#region Get Set
    public void SetTriggerAnimator(int trigger){
        if(m_Animator != null)
            m_Animator.SetTrigger(trigger);
    }
    public void SetBonusAttackSpeed(float value){
        for(int i = 0; i < m_Weapons.Count; i++){
            Weapon wp = m_Weapons[i];
            wp.SetBonusAttackSpeed(value);
        }
    }
#endregion

#region Execute
    protected virtual void FaceLeft(){
        Transform.localEulerAngles = new Vector3(0, 180, 0);
    }
    protected virtual void FaceRight(){
        Transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    public void MoveToNextRound(){
        m_StateMachine.ChangeState(StickmanMoveToNextCampaignRoundState.Instance);
    }
    public void WaitState(){
        m_StateMachine.ChangeState(StickmanWaitState.Instance);
    }
    protected override void StartAttack(AttackInfo attackInfo){
        base.StartAttack(attackInfo);
        Weapon weapon = GetWeaponAttack(attackInfo);
        SetBonusAttackSpeed(HeroManager.Ins.GetBonusAttackSpeed());
        weapon.SetBonusAttackSpeed(HeroManager.Ins.GetBonusAttackSpeed());
        weapon.StartAttack();
        m_Animator.SetTrigger(weapon.GetTriggerName());
    }
    public override void Attack(){
        base.Attack();
        AttackCount++;
        if(m_NextAttackInfo.attackType == AttackType.SKILL_ATTACK && m_Class == Class.WARRIOR){
            CameraManger.Ins.ShakeCamera(0.12f);
        }
    }
    public override void Reset(){
        m_Rigidbody.gravityScale = m_GravityScale;
        m_IsAttacking = false;
        m_IsInCombat = false;
        IsHoldingAttack = false;
        Transform.localEulerAngles = new Vector3(0, 0, 0);
        Transform.position = FollowTransform.position;
        m_StateMachine.SetCurrentState(StickmanWaitState.Instance);
        m_OnDamagedCallback = null;
        m_OnDeadCallback = null;
        AttackCount = 0;
        FaceRight();
    }
    // public void Reborn(bool isResetPosition) {
    //     //if(IsDead() || isResetPosition) {
    //     //    Vector3 v = GetSummonPosition();
    //     //    Transform.position = v;
    //     //}
    //     //m_Animator.SetTrigger(animatorIdleHash);
    //     //m_StateMachine.ChangeState(StickmanWaitState.Instance);
    // }
#endregion

    public void OnHandleGlobalMessage(Telegram msg){ }

#region StateMachine
    public override void OnRunning(){
        float dt = Time.deltaTime;
        OnWeaponRunning(dt);
        FindingTarget();
    }
    //==================Wait================//
    public virtual void OnWaitStart(){
        
    }
    public virtual void OnWaitExecute(){
        m_Rigidbody.velocity = Vector2.zero;
        m_Animator.SetBool(animatorIsRunningHash, false);
    }
    public virtual void OnWaitExit(){ }
    //================Idle===============//
    public virtual void OnIdleStart(){ }
    public virtual void OnIdleExecute(){ }
    public virtual void OnIdleExit(){ }

    //==================Control============//
    public virtual void OnControlStart(){ }
    public virtual void OnControlExecute(){ }
    public virtual void OnControlExit(){ }

    //================Chase==========//
    public virtual void OnChaseEnemyStart(){
        //SetTriggerAnimator(animatorIdleHash);
        m_Target = IngameEntityManager.Ins.GetNearestEnemy(Transform, m_IngameTeam);
        m_AttackRangeOffset = Random.Range(-0.2f, 0.4f);
    }
    public virtual void OnChaseEnemyExecute(){
        if(m_Target == null || m_Target.IsDead()){
            m_Target = IngameEntityManager.Ins.GetNearestEnemy(Transform, m_IngameTeam);
            m_AttackRangeOffset = Random.Range(0, 1f);
        }

        if(m_Target == null || m_Target.IsDead()){
            m_Animator.SetBool(animatorIsRunningHash, false);
            m_Rigidbody.velocity = Vector2.zero;
            m_StateMachine.ChangeState(StickmanMoveToNextCampaignRoundState.Instance);
            return;
        }

        Vector3 v1 = Transform.position;
        Vector3 targetPos = m_Target.Transform.position;
        targetPos.y = v1.y;
        targetPos.z = v1.z;
        m_Rigidbody.velocity = (targetPos - v1).normalized * m_Movement.GetMoveSpeed();
        float attackRange = 1;
        m_NextAttackInfo = GetNextAttackInfo();
        if(m_NextAttackInfo.attackType != AttackType.NONE){
            attackRange = GetNextAttackRange(m_NextAttackInfo);
        } else{
            attackRange = GetNormalAttackRange();
        }

        attackRange += m_AttackRangeOffset;
        bool inAtAttackRange = IsInRange(Transform.position, targetPos, attackRange);
        if(inAtAttackRange){
            m_Rigidbody.velocity = Vector2.zero;
            m_Animator.SetBool(animatorIsRunningHash, false);
            m_StateMachine.ChangeState(StickmanAttackState.Instance);
            return;
        }

        m_Animator.SetBool(animatorIsRunningHash, true);
    }
    public virtual void OnChaseEnemyExit(){
        m_Animator.SetBool(animatorIsRunningHash, false);
        m_Rigidbody.velocity = Vector2.zero;
    }
    //===============Attack=============//
    public virtual void OnAttackStart(){
        //SetTriggerAnimator(animatorIdleHash);
    }
    public virtual void OnAttackExecute(){
        if(m_IsAttacking) return;
        if(m_Target == null || m_Target.IsDead()){
            m_StateMachine.ChangeState(StickmanChaseEnemyState.Instance);
            return;
        }

        UpdateFacingToTarget();
        m_NextAttackInfo = GetNextAttackInfo();
        if(m_NextAttackInfo == null || m_NextAttackInfo.attackType == AttackType.NONE) return;
        float attackRange = GetNextAttackRange(m_NextAttackInfo);
        attackRange += m_AttackRangeOffset;
        bool inAtAttackRange = IsInRange(Transform.position, m_Target.Transform.position, attackRange);
        if(!inAtAttackRange){
            m_StateMachine.ChangeState(StickmanChaseEnemyState.Instance);
            return;
        }

        if(IsGoodToAttack()){
            StartAttack(m_NextAttackInfo);
        }
    }
    public virtual void OnAttackExit(){
        m_IsAttacking = false;
    }
    //================Dead==============//
    public virtual void OnDeadStart(){ }
    public virtual void OnDeadExecute(){ }
    public virtual void OnDeadExit(){ }

    //==================MoveToNextCampain==============//
    public Vector3 m_TargetPosition;
    public virtual void OnMoveToNextRoundStart(){
        m_Animator.SetTrigger(animatorIdleHash);
        //m_TargetPosition = IngameManager.Ins.m_HeroManager.m_CharacterPosition.GetFreePosition(m_SlotID,m_Platform);
        FaceRight();
    }
    public virtual void OnMoveToNextRoundExecute(){
        // Vector3 v1 = Transform.position;
        // Vector3 pos = FollowTransform.position;
        // pos.y = v1.y;
        // pos.z = v1.z;
        // m_Rigidbody.velocity = (pos - v1).normalized * m_Movement.GetMoveSpeed();
        m_Rigidbody.velocity = Vector2.right * m_Movement.GetMoveSpeed();
        m_NextAttackInfo = GetNextAttackInfo();
        // float range = 0.05f;
        // bool inAttackRange = IsInRange(transform.position, pos, range);
        if(m_Target){
            m_Rigidbody.velocity = Vector2.zero;
            m_Animator.SetBool(animatorIsRunningHash, false);
            m_StateMachine.ChangeState(StickmanChaseEnemyState.Instance);
            Fight();
            return;
        }

        m_Animator.SetBool(animatorIsRunningHash, true);
    }
    public virtual void OnMoveToNextRoundExit(){
        m_Rigidbody.velocity = new Vector2(0, 0);
    }
#endregion

    protected override void UpdateFacingToTarget(){
        base.UpdateFacingToTarget();
        if(m_Target == null)
            return;
        if(m_Target.Transform.position.x < Transform.position.x){
            Transform.localEulerAngles = new Vector3(0, 180, 0);
        } else{
            Transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    public override bool IsDead(){
        return false;
    }
}