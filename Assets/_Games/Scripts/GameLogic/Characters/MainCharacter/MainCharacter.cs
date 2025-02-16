using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character{

    [SerializeField]
    private string m_CurrentState;
    
    private BigNumber m_HpRegen;
    private float m_HpRegenTimer;
    private float m_DyingTime;
    private float m_DeadTimer;

    private Transform m_FollowPos;
    public StateMachine<MainCharacter> StateMachine{ get; private set; }
    public Transform FollowPosition{ get => m_FollowPos; set => m_FollowPos = value; }
    private void Update(){
        StateMachine.Update();
        m_CurrentState = StateMachine.CurrentState.ToString();
    }
    public virtual void InitCharacter(){
        SetTeam(IngameTeam.Team1);
        InitStateMachine();
        BattleCry();
        InitWeapon();
        UpdateStat();
        m_Movement = new Movement{ maxSpeed = 5, minSpeed = 5, moveSpeed = 5 };
    }
    private void InitStateMachine(){
        StateMachine = new StateMachine<MainCharacter>(this);
        StateMachine.SetCurrentState(CharacterWaitState.Instance);
        StateMachine.SetGlobalState(CharacterGlobalState.Instance);
    }
    protected override void InitNormalWeapon(){
        base.InitNormalWeapon();
    }
    public override void BattleCry(){
        base.BattleCry();
        StateMachine.ChangeState(CharacterWaitState.Instance);
        m_Collider.enabled = true;
        m_IsAttacking = false;
    }
    public override void Fight(){
        base.Fight();
        StateMachine.ChangeState(CharacterChaseEnemyState.Instance);
    }
    public override void SetTeam(IngameTeam ingameTeam){
        base.SetTeam(ingameTeam);
        m_Collider.tag = (ConstantString.TAG_CHARACTER);
    }
#region stat
    public void UpdateStat(){
        List<StatData> listStat = StatManager.Ins.StatDataList;
        for(int i = 0; i < listStat.Count; i++){
            StatData stat = listStat[i];
            ExecuteUpdateStat(stat);
        }
    }
    public void ExecuteUpdateStat(StatData statData){
        BigNumber value = StatManager.Ins.GetStatValue2(statData);
        MainStatType stat = statData.st;
        switch(stat){
            case MainStatType.Attack:
                for(int i = 0; i < m_Weapons.Count; i++){
                    m_Weapons[i].UpdateStat(stat, PropertiesManager.Ins.TotalAttack);
                }

                break;
            case MainStatType.AttackSpeed:
                for(int i = 0; i < m_Weapons.Count; i++){
                    m_Weapons[i].UpdateStat(stat, value);
                }

                break;
            case MainStatType.HP:
                SetMaxHP(PropertiesManager.Ins.TotalHp);
                break;
            case MainStatType.HPRegen:
                SetHPRegen(value);
                break;
            case MainStatType.CriticalChance:
                for(int i = 0; i < m_Weapons.Count; i++){
                    m_Weapons[i].UpdateStat(stat, value);
                }

                break;
            case MainStatType.CriticalDamage:
                for(int i = 0; i < m_Weapons.Count; i++){
                    m_Weapons[i].UpdateStat(stat, value);
                }

                break;
            case MainStatType.DoubleShotChance:
                for(int i = 0; i < m_Weapons.Count; i++){
                    m_Weapons[i].UpdateStat(stat, value);
                }

                break;
            case MainStatType.TripleShotChance:
                for(int i = 0; i < m_Weapons.Count; i++){
                    m_Weapons[i].UpdateStat(stat, value);
                }

                break;
            case MainStatType.DamamgeReduction:
                SetReduceInDamage(value.ToFloat());
                break;
            case MainStatType.DamageAmplify:
                break;
            default:
                break;
        }
    }
#endregion
    public override void OnDamage(BigNumber damage, DamageInfo damageInfo){
        if(m_ReduceInDamage > 0){
            damage -= damage * m_ReduceInDamage / 100;
        }
        if(damage < 0) damage = 0;
        base.OnDamage(damage, damageInfo);
    }
#region  Control
    public void StopAttack(){
        StateMachine.ChangeState(CharacterWaitState.Instance);
        m_IsAttacking = false;
    }
#endregion
    public void MoveToNextRound(){
        StateMachine.ChangeState(CharacterMoveToNextRound.Instance);
    }
    public override void OnRunning(){
        base.OnRunning();
        float dt = Time.deltaTime;
        OnHpRegen(dt);
    }
    public void OnHpRegen(float dt){
        if(IsDead())
            return;
        if(m_HpRegenTimer > 0){
            m_HpRegenTimer -= dt;
        } else{
            m_HpRegenTimer = 1;
            AddHP(m_HpRegen);
        }
    }
    private void SetHPRegen(BigNumber amount){
        m_HpRegen = amount;
    }
    public void SetBonusAttackSpeed(float value){
        for(int i = 0; i < m_Weapons.Count; i++){
            Weapon wp = m_Weapons[i];
            wp.SetBonusAttackSpeed(value);
        }
    }
#region Execute
    //
#endregion
    public virtual void OnHandleGlobalMessage(Telegram msg){ }
    /// <summary>
    /// For the control state
    /// </summary>
    public virtual void OnControlStart(){ }
    public virtual void OnControlExecute(){ }
    public virtual void OnControlExit(){ }
    /// <summary>
    /// For the Wait State
    /// </summary>
    public virtual void OnWaitStart(){
        SetTriggerAnimator(ConstantString.ANIM_IDLE);
    }
    public virtual void OnWaitExecute(){ }
    public virtual void OnWaitExit(){ }
    /// <summary>
    /// For the Idle state
    /// </summary>
    public virtual void OnIdleStart(){
        SetTriggerAnimator(ConstantString.ANIM_IDLE);
    }
    public virtual void OnIdleExecute(){ }
    public virtual void OnIdleExit(){ }
    /// <summary>
    /// For the chase state
    /// </summary>
    public virtual void OnChaseStart(){
        SetTriggerAnimator(ConstantString.ANIM_IDLE);
        // m_Target = IngameEntityManager.Ins.GetNearestEnemy(Transform, m_IngameTeam);
        // m_AttackRangeOffset = UnityEngine.Random.Range(-0.4f, 0.5f);
    }
    public virtual void OnChaseExecute(){
        if(Target == null || Target.IsDead()){
            StateMachine.ChangeState(CharacterMoveToNextRound.Instance);
            return;
        }
        Vector3 targetPos = Target.Transform.position;
        Vector3 dir = (targetPos - Transform.position).normalized;
        Vector3 pos = Transform.position + dir * 1 /*m_Movement.GetMoveSpeed()*/ * Time.deltaTime;
        transform.position = pos;

        m_NextAttackInfo = GetNextAttackInfo();
        float attackRange = 1;
        if(m_NextAttackInfo.attackType != AttackType.NONE){
            attackRange = GetNextAttackRange(m_NextAttackInfo);
        } else{
            attackRange = GetNormalAttackRange();
        }

        //attackRange += m_AttackRangeOffset;
        bool inAttackRange = IsInRange(Transform.position, targetPos, attackRange);
        if(inAttackRange){
            StateMachine.ChangeState(CharacterAttackEnemyState.Instance);
            return;
        }

        m_Animator.SetBool(animatorIsRunningHash, true);
    }
    public virtual void OnChaseExit(){
        if(!IsDead()){
            m_Animator.SetBool(animatorIsRunningHash, false);
        }
    }
    /// <summary>
    /// For the attack basic State
    /// </summary>
    public virtual void OnAttackStart(){ }
    public virtual void OnAttackExecute(){
        if(m_IsAttacking) return;
        if(m_Target == null || m_Target.IsDead()){
            StateMachine.ChangeState(CharacterChaseEnemyState.Instance);
            return;
        }

        UpdateFacingToTarget();
        m_NextAttackInfo = GetNextAttackInfo();
        if(m_NextAttackInfo.attackType == AttackType.NONE)
            return;
        float attackRange = GetNextAttackRange(m_NextAttackInfo);
        //attackRange += m_AttackRangeOffset;
        if(!IsInRange(Transform.position, m_Target.Transform.position, attackRange)){
            StateMachine.ChangeState(CharacterChaseEnemyState.Instance);
            return;
        }

        if(IsGoodToAttack()){
            StartAttack(m_NextAttackInfo);
        }
    }
    public virtual void OnAttackExit(){ }
    /// <summary>
    /// For the Dying state
    /// </summary>
    public virtual void OnDyingStart(){ }
    public virtual void OnDyingExecute(){
        m_DyingTime -= Time.deltaTime;
        if(m_DyingTime <= 0){
            StateMachine.ChangeState(CharacterDeadState.Instance);
        }
    }
    public virtual void OnDyingExit(){ }
    /// <summary>
    /// For the dead state
    /// </summary>
    public virtual void OnDeadStart(){
        m_DeadTimer = 0.5f;
        // m_Rigidbody.AddForce(new Vector2(5, 5) * UnityEngine.Random.Range(100, 100), ForceMode2D.Force);
    }
    public virtual void OnDeadExecute(){
        if(m_DeadTimer > 0){
            m_DeadTimer -= Time.deltaTime;
            if(m_DeadTimer <= 0){
                //Deactive();
                //Reset();
            }
        }
    }
    public virtual void OnDeadExit(){ }

    //=====================================================//
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
    protected override void StartAttack(AttackInfo attackInfo){
        base.StartAttack(attackInfo);
        SetBonusAttackSpeed(HeroManager.Ins.GetBonusAttackSpeed());
        Weapon weapon = GetWeaponAttack(attackInfo);
        weapon.StartAttack();
        SetTriggerAnimator(weapon.GetTriggerName());
    }
    public override void Dead(){
        base.Dead();
        if(m_OnDeadCallback != null){
            m_OnDeadCallback();
        }

        IngameManager.Ins.Lose();
        //m_Rigidbody.gravityScale = 1;
        //m_DeadPosition = Transform.position + new Vector3(0, 0.2f, 0);
        m_Animator.SetTrigger(animatorDieHash);
        StateMachine.ChangeState(CharacterDeadState.Instance);
        //IngameManager.Instance.MarkKillCharacter(m_IngameTeam);
    }
    public override void Deactive(){
        //Debug.Log("Deactive " + gameObject.name);
        DeactiveHPBar();
        UnregisterInScene();
        SimplePool.Despawn(gameObject);
    }

    //==================================================//
    public Vector3 m_TargetPosition;
    public virtual void OnMoveToNextRoundStart(){
        SetTriggerAnimator(ConstantString.ANIM_IDLE);
    }
    public virtual void OnMoveToNextRoundExecute(){
        // Vector3 v1 = Transform.position;
        // Vector3 pos = m_TargetPosition;
        // pos.y = v1.y;
        // pos.z = v1.z;
        // m_Rigidbody.velocity = (pos - v1).normalized * m_Movement.GetMoveSpeed();
        m_Rigidbody.velocity = Vector2.right * m_Movement.GetMoveSpeed();
        m_NextAttackInfo = GetNextAttackInfo();
       // float range = 0.05f;
        //bool inAttackRange = IsInRange(transform.position, pos, range);
        if(m_Target != null ){
            if(!m_Target.IsDead())
            {
                // m_Rigidbody.velocity = Vector2.zero;
                // m_Animator.SetBool(animatorIsRunningHash, false);
                StateMachine.ChangeState(CharacterChaseEnemyState.Instance);
                Fight();
                return;
            }
        }

        m_Animator.SetBool(animatorIsRunningHash, true);
    }
    public virtual void OnMoveToNextRoundExit(){
        m_Rigidbody.velocity = new Vector2(0, 0);
        m_Animator.SetBool(animatorIsRunningHash, false);
    }
    public override void Reset(){
        base.Reset();
        InitHealthbar(); 
        StateMachine.ChangeState(CharacterWaitState.Instance);    
    }
}