using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum IngameTeam { None = -1, Team1 = 0, Team2 = 1}
public class Character : Creature{
    public Transform m_BodyPoint;
    public static readonly int animatorIsRunningHash = Animator.StringToHash("IsRunning");
    public static readonly int animatorAttackSpeedHash = Animator.StringToHash("AttackSpeed");
    public static readonly int animatorStunHash = Animator.StringToHash("Stun");
    public static readonly int animatorIdleHash = Animator.StringToHash("Idle");
    public static readonly int animatorDieHash = Animator.StringToHash("Die");
    public int m_Level;
    public IngameType m_TargetType;
    public IngameTeam m_IngameTeam;
    //public UIStatusBar m_StatusBar;
    public List<Weapon> m_Weapons = new List<Weapon>();
    protected readonly List<Weapon> m_NormalAttacks = new List<Weapon>();
    protected readonly List<Weapon> m_SkillAttacks = new List<Weapon>();


    [SerializeField]
    protected IngameObject m_Target;
    public IngameObject Target {
        get => m_Target;
        set => m_Target = value;
    }
    [SerializeField]
    protected AttackInfo m_NextAttackInfo = new AttackInfo();

    protected CharacterAnimator m_CharacterAnimator;
    protected MeshRenderer m_MeshRenderer;
    protected MaterialPropertyBlock m_MaterialPropertyBlock;
    protected Shader m_ShaderFill;
    protected Shader m_ShaderTint;
    protected float m_GravityScale;
    public bool m_IsInCombat = true;
    public bool m_IsAttacking;
    protected UnityAction m_OnDamagedCallback;
    protected UnityAction m_OnDeadCallback;

    public override void Init() {
        base.Init();
        m_GravityScale = m_Rigidbody.gravityScale;
        m_NextAttackInfo = new AttackInfo();
        m_CharacterAnimator = m_Animator.GetComponent<CharacterAnimator>();
        m_CharacterAnimator.m_Character = this;
        //m_MaterialPropertyBlock = new MaterialPropertyBlock();
        //m_MeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
        m_ShaderFill = Shader.Find("Spine/Skeleton Fill");
        m_ShaderTint = Shader.Find("Spine/Skeleton Tint");
    }
    public override void RegisterInScene() {
        base.RegisterInScene();
        IngameEntityManager.Ins.RegisterTeam(this,m_IngameTeam);
    }
    public override void InitHealthbar() {
        base.InitHealthbar();
        m_HealthBar.Init();
        m_HealthBar.SetOwner(this);
        m_HealthBar.offset.y = m_OffSetYHealthBar;
    }
    #region Execute
    public override void OnRunning() {
        if(IsDead()) return;
        float dt = Time.deltaTime;
        OnWeaponRunning(dt);
        FindingTarget();
        OnBuffRunning();
    }
    #endregion
    
    #region Weapon
    protected virtual void InitWeapon() {
        m_NormalAttacks.Clear();
        //m_SkillAttacks.Clear();
        for (int i = 0; i < m_Weapons.Count; i++) {
            Weapon wp = m_Weapons[i];
            switch (wp.attackType) {
                case AttackType.NORMAL_ATTACK: {
                    wp.SetCurrentReloadTime(0.01f);
                    m_NormalAttacks.Add(wp);
                }
                break;
                case AttackType.SKILL_ATTACK: {
                    m_SkillAttacks.Add(wp);
                }
                break;
                case AttackType.NONE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        InitNormalWeapon();
        //InitSkillWeapon();
    }
    protected virtual void InitNormalWeapon() {
        for (int i = 0; i < m_NormalAttacks.Count; i++) {
            Weapon wp = m_NormalAttacks[i];
            //wp.Init(wp.m_WeaponDataConfig.id, m_Level, m_Rank, this);
            wp.InitWeapon(this);
            wp.SetCurrentReloadTime(0.01f);
        }
    }
    public virtual void InitSkillWeapon() {
        for (int i = 0; i < m_SkillAttacks.Count; i++) {
            Weapon wp = m_SkillAttacks[i];
            //wp.Init(wp.m_WeaponDataConfig.id, m_Level, m_Rank, this);
            wp.InitWeapon(wp.m_WeaponDataConfig, m_TargetType, this);
        }
    }
    public virtual void UpdateWeaponStat() {

    }
    protected virtual void OnWeaponRunning(float dt) {
        if(!m_IsInCombat) return;
        if(IsHoldingAttack) return;
        int num = m_Weapons.Count;
        for (int i = 0; i < num; i++) {
            Weapon wp = m_Weapons[i];
            wp.OnRunning(dt);
        }
    }
    #endregion
    public override void OnDamage( BigNumber damage, DamageInfo damageInfo ) {
        if(IngameManager.Ins.IsEndMatch) return;
        base.OnDamage(damage, damageInfo);
        OnGetHit();
        if (m_OnDamagedCallback != null) {
            m_OnDamagedCallback();
        }
    }
    public void OnGetHit() {
       // StopCoroutine(coFlashRoutine());
       // StartCoroutine(coFlashRoutine());
    }
    protected virtual void FindingTarget() {
        if(m_Target == null || m_Target.IsDead()) {
            m_Target = IngameEntityManager.Ins.GetNearestEnemy(m_Transform,m_IngameTeam);
        }
    }
    public virtual void SetTeam( IngameTeam ingameTeam ) {
        m_IngameTeam = ingameTeam;
        m_Collider.tag = ingameTeam.ToString();
    }
    public virtual void Fight() {
        m_IsInCombat = true;
        IsHoldingAttack = false;
    }
    protected AttackInfo GetNextAttackInfo() {
        bool hasAttack = false;
        for (int i = 0; i < m_SkillAttacks.Count; i++) {
            Weapon wp = m_SkillAttacks[i];
            if (wp.IsReadyToAttack()) {
                m_NextAttackInfo.attackIndex = i;
                m_NextAttackInfo.attackType = AttackType.SKILL_ATTACK;
                hasAttack = true;
            }
        }
        if (hasAttack) {
            return m_NextAttackInfo;
        }
        hasAttack = false;
        for (int i = 0; i < m_NormalAttacks.Count; i++) {
            Weapon wp = m_NormalAttacks[i];
            if (wp.IsReadyToAttack()) {
                m_NextAttackInfo.attackIndex = i;
                m_NextAttackInfo.attackType = AttackType.NORMAL_ATTACK;
                hasAttack = true;
            }
        }
        if (hasAttack) {
            return m_NextAttackInfo;
        }
        m_NextAttackInfo.attackIndex = -1;
        m_NextAttackInfo.attackType = AttackType.NONE;
        return m_NextAttackInfo;
    }
    protected float GetNextAttackRange( AttackInfo attackInfo ){
        int attackIndex = attackInfo.attackIndex;
        return attackInfo.attackType switch{
            AttackType.NORMAL_ATTACK => GetNextNormalAttackRange(attackIndex),
            AttackType.SKILL_ATTACK => GetNextSkillAttackRange(attackIndex),
            _ => 0
        };
    }
    protected float GetNormalAttackRange() {
        float normalAttackRange = m_NormalAttacks[0].GetAttackRange();
        return normalAttackRange;
    }
    public float GetNextNormalAttackRange( int index ) {
        return m_NormalAttacks[index].GetAttackRange();
    }
    public float GetNextSkillAttackRange( int index ) {
        return m_SkillAttacks[index].GetAttackRange();
    }
    protected virtual Weapon GetWeaponAttack( AttackInfo attackInfo ) {
        Weapon weapon = null;
        int attackIndex = attackInfo.attackIndex;
        AttackType attackType = attackInfo.attackType;
        switch (attackType) {
            case AttackType.NORMAL_ATTACK:
                weapon = m_NormalAttacks[attackIndex];
                break;
            case AttackType.SKILL_ATTACK:
                weapon = m_SkillAttacks[attackIndex];
                break;
            case AttackType.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return weapon;
    }
    protected bool IsGoodToAttack() {
        if (IsDead())
            return false;
        if (IsHoldingAttack)
            return false;
        for (int i = 0; i < m_SkillAttacks.Count; i++) {
            Weapon wp = m_SkillAttacks[i];
            if (wp.IsReadyToAttack()) {
                return true;
            }
        }
        for (int i = 0; i < m_NormalAttacks.Count; i++) {
            Weapon wp = m_NormalAttacks[i];
            if (wp.IsReadyToAttack()) {
                return true;
            }
        }
        return false;
    }
    protected virtual void StartAttack(AttackInfo attackInfo ) {
        m_IsAttacking = true;
    }
    public virtual void AttackDone() {
        m_IsAttacking = false;
    }
    protected virtual void SetTriggerAnimator( string trigger ) {
        if (m_Animator != null)
            m_Animator.SetTrigger(trigger);
    }
    protected virtual void UpdateFacingToTarget() {
    }
    public virtual void Attack() {
        if(m_Target == null) return;
        Weapon weapon = GetWeaponAttack(m_NextAttackInfo);
        if (weapon != null) {
            weapon.Attack();
            weapon.StartCooldown();
            m_CharacterAnimator.m_AttackType = weapon.attackType;
            //if (weapon.attackType == AttackType.NORMAL_ATTACK) {
                //SoundManager.Instance.PlayAttackSound(0);
           // }
        }
    }

#region  BUFF
    public override int ApplyBuff(Buff buff) {
        int len = m_AddedBuffes.Count;
        for (int i = 0; i < len; i++) {
            Buff iB = m_AddedBuffes[i];
            if (iB.GetOwnerID() == buff.GetOwnerID() && iB.BuffType == buff.BuffType) {
                if (!iB.IsPernamentBuff) {
                    iB.SetTime(buff.m_AffectedTime);
                }
                return iB.ID;
            }
        }
        Buff newBuff;
        if (!buff.IsLinkedBuff()) {
            newBuff = IngameManager.Ins.GetNewBuff(buff);
            newBuff.ApplyToOwner(this);
            m_AddedBuffes.Add(newBuff);
        } else {
            newBuff = buff;
            m_AddedBuffes.Add(buff);
        }
        newBuff.StartBuff();
        return newBuff.ID;
    }
#endregion
    #region Get
    public virtual BigNumber GetBaseAttack() {
        return 0;
    }
    public virtual float GetBaseAttackSpeed() {
        return 0.8f;
    }
    #endregion
    public IngameTeam GetTeam() {
        return m_IngameTeam;
    }
    public IngameTeam GetTargetTeam(){
        return m_IngameTeam switch{
            IngameTeam.Team1 => IngameTeam.Team2,
            IngameTeam.Team2 => IngameTeam.Team1,
            _ => IngameTeam.None
        };
    }
    protected virtual bool IsInRange(Vector3 from, Vector3 to,float range ) {
        bool isIn = Utilss.IsBetween(from, to, range);
        return isIn;
    }
    public override void Reset() {
        base.Reset();
        //RemoveAllBuff();
        //Material mat = m_MeshRenderer.materials[0];
        //mat.shader = m_ShaderFill;
        //m_MeshRenderer.materials[0] = mat;
        SetHP(m_MaxHP);
        m_Rigidbody.gravityScale = m_GravityScale;
        m_IsInCombat = false;
        m_IsAttacking = false;
        IsHoldingAttack = false;
        Transform.localEulerAngles = new Vector3(0, 0, 0);
        m_OnDamagedCallback = null;
        m_OnDeadCallback = null;
    }
    public bool IsDeactive(){
        return gameObject.activeInHierarchy;
    }
}
