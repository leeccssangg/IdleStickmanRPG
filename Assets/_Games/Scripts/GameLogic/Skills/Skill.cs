using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public enum SkillState{ COOLDOWN = 0, ATTACK = 1, WAIT = 2 }
public enum SkillType{ NONE = 0, NORMAL = 1,ARTIFACT = 2 }

public class Skill : MonoBehaviour{
    public SkillInfo m_Info;

    [ReadOnly]
    [SerializeField]
    private SkillState m_State;
    [ReadOnly]
    [SerializeField]
    private SkillType m_SkillType;

    private bool m_IsAuto;

    [SerializeField]
    private string m_EffectName;

    public Transform m_FirePos;
    public ShootPattern m_ShootPattern;
    protected IngameObject m_Target;

    [SerializeField]
    private BigNumber m_OutDamage;

    [SerializeField]
    protected BulletInfo m_BulletInfo;

    [SerializeField]
    protected int m_BulletAmount;

    [SerializeField]
    private List<Buff> m_Buffs = new();

    [SerializeField]
    protected float m_AttackTime;

    protected Character m_Character;
    private bool m_IsHoldingAttack;
    private bool m_IsReady;
    
    //ArtifactsBuffSkill
    private ArtifactBuffSkill m_ArtifactBuff;
    public event UnityAction OnStartAttackEvent;
    public event UnityAction OnStartCooldownEvent;
    public string m_CurrentState = "";
    public SkillState State{ get => m_State; private set => m_State = value; }
    public int ID => m_Info.SkillID;
    public float CurrentReloadTime { get; set; }
    public float CooldownTime { get; private set; }
    public float AttackTime{ get => m_AttackTime; set => m_AttackTime = value; }
    public float CurrentCountingTime { get; private set; }
    private bool IsAttacking{ get; set; }

    public SkillType Type
    {
        get => m_SkillType;
        private set => m_SkillType = value;
    }

    private StateMachine<Skill> m_StateMachine;
    public virtual void Running(){
        m_StateMachine.Update();
        m_CurrentState = m_StateMachine.CurrentState.ToString();
        FindingTarget();
    }
    public virtual void Init(SkillInfo info,SkillType type, Character character){
        m_Character = character;
        m_Info = info;
        m_SkillType = type;
        InitStateMachine();
        InitShootPattern();
        //UpdateStat();
        if(m_ArtifactBuff == null){
            m_ArtifactBuff = GetComponent<ArtifactBuffSkill>();
        }
        CooldownTime = GetReloadTime();
        m_IsAuto = false;
        m_IsReady = true;
        m_IsHoldingAttack = false;
        IsAttacking = false;
        OnStartAttackEvent = null;
        OnStartCooldownEvent = null;
        transform.position = character.Transform.position;
    }
    protected virtual void UpdateStat(){
        CalculateDamage();
    }

    private void InitStateMachine(){
        m_StateMachine = new StateMachine<Skill>(this);
        m_StateMachine.SetCurrentState(SkillCooldownState.Instance);
    }
    private void InitShootPattern(){
        if(m_ShootPattern == null) return;
        m_ShootPattern.ClearAllEvent();
        m_ShootPattern.BulletAmount = m_BulletAmount;
        m_ShootPattern.SetBulletInfo(m_BulletInfo);
        m_IsReady = true;
    }

    protected virtual void FindingTarget(){
        if(m_Target == null || m_Target.IsDead()){
            m_Target = IngameEntityManager.Ins.GetRandomEnemy();
        }
    }
    public virtual void StopToAttacking(){
        m_IsHoldingAttack = true;
        m_ShootPattern.StopShot();
        m_StateMachine.ChangeState(SkillWaitState.Instance);
    }
    public void ContinueToAttack(){
        m_IsHoldingAttack = false;
    }
    public void StartRunning(){
        m_StateMachine.ChangeState(SkillCooldownState.Instance);
    }
    public void WaitingToAttack(){
        m_StateMachine.ChangeState(SkillWaitState.Instance);
    }
    public bool IsReadyToAttack(){
        return CurrentReloadTime <= 0 && !m_IsHoldingAttack && !IsAttacking;
    }
    public bool IsInUse(){
        return AttackTime > 0;
    }
    public virtual void StartAttack(){
        if(!IsGoodToAttack()) return;
        m_StateMachine.ChangeState(SkillAttackState.Instance);
        IsAttacking = true;
    }

    protected virtual void Attack(){
        DoStartAttackEvent();
        if(m_ShootPattern != null){
            SetFirePos();
            m_ShootPattern.m_FirePos = m_FirePos;
            m_ShootPattern.SetFireDirectionDelegate(GetFireDirection);
            m_ShootPattern.BulletAmount = m_BulletAmount;
            m_ShootPattern.SetDamage(GetDamage());
            m_ShootPattern.Shoot();
        }
    }

    protected void DoStartAttackEvent(){
        OnStartAttackEvent?.Invoke();
    }

    private void DoStartCooldownEvent(){
        OnStartCooldownEvent?.Invoke();
    }

    protected virtual void SetFirePos(){
        if(m_Target == null || m_Target.IsDead()){
            m_Target = IngameEntityManager.Ins.GetRandomEnemy();
        }

        if(m_Target == null || m_Target.IsDead()){
            m_FirePos.position = m_Character.Transform.position + Vector3.right * 2.5f;
        } else{
            m_FirePos.position = m_Target.Transform.position;
        }
    }
    public void SetAuto(bool isAuto){
        m_IsAuto = isAuto;
    }

    protected virtual Vector3 GetFireDirection(){
        if(m_Target == null){
            return m_FirePos.forward + Vector3.up * 0.5f;
        } else{
            Vector3 v = m_Target.Transform.position + Vector3.up * 0.5f;
            Vector3 direct = v - m_FirePos.position;
            return direct.normalized;
        }
    }
    public virtual float GetAttackTime(){
        return m_AttackTime;
    }

    protected virtual BigNumber GetDamage()
    {
        CalculateDamage();
        return m_OutDamage;
    }

    private void CalculateDamage()
    {   
        //Base Attack
        BigNumber damage = PropertiesManager.Ins.TotalAttack;
        //Damage cua Skill
        BigNumber skillDamage = damage * m_Info.GetDpsRate() * 0.01f ;
        //Damage Rate cua Artifact
        BigNumber artifactDmgRate = m_ArtifactBuff ? m_ArtifactBuff.GetBuff() : 1;
        Debug.Log($"ArtifactBuffSkill = {artifactDmgRate}");
        
        skillDamage += skillDamage * artifactDmgRate*0.01f;
        //Damage Rate cua Skill
        float dmgRate = m_Info.DmgRate;
        // Debug.Log($"{damage} - {dpsRate} - {dmgRate}");
        
        m_OutDamage = skillDamage * dmgRate;
    }
    private float GetReloadTime()
    {
        float num = m_Info.CoolDownTime;
        if (Type == SkillType.NORMAL)
        {
            num -= (num * PropertiesManager.Ins.SkillCoolDownReduction.ToFloat() * 0.01f);
        }
        return num;
    }

    private bool IsGoodToAttack(){
        if(m_Target == null || m_Target.IsDead())
            return false;
        if(m_IsHoldingAttack || IsAttacking)
            return false;
        if (m_Info.IsLock()) return false;
        return true;
    }
    //=====================State====================//
    public virtual void OnRunning(){ }
    //=====================Cooldown=================//
    public virtual void OnCooldownStart(){
        State = SkillState.COOLDOWN;
        CurrentCountingTime = GetReloadTime();
        DoStartCooldownEvent();
    }
    public virtual void OnCooldownExecute(){
        if(CurrentCountingTime > 0){
            CurrentCountingTime -= Time.deltaTime;
            if(CurrentCountingTime <= 0){
                m_StateMachine.ChangeState(SkillWaitState.Instance);
            }
        }
    }
    public virtual void OnCooldownExit(){ }
    //=====================Attack===================//
    public virtual void OnAttackStart(){
        Attack();
        State = SkillState.ATTACK;
        CurrentCountingTime = GetAttackTime();
    }
    public virtual void OnAttackExecute(){
        if(CurrentCountingTime > 0){
            CurrentCountingTime -= Time.deltaTime;
            if(CurrentCountingTime <= 0){
                m_StateMachine.ChangeState(SkillCooldownState.Instance);
            }
        }
    }
    public virtual void OnAttackExit(){
        IsAttacking = false;
    }
    //===================Wait=====================//
    public virtual void OnWaitStart(){
        State = SkillState.WAIT;
    }
    public virtual void OnWaitExecute(){
        if(m_Target == null || m_Target.IsDead()){
            m_Target = IngameEntityManager.Ins.GetRandomEnemy();
        }

        if(m_IsAuto) StartAttack();
    }
    public virtual void OnWaitExit(){ }
}