using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IWeaponInterface {
    Vector3 GetGunPos();
    Vector3 GetFireDirection();
    int GetBulletAmount();
    BigNumber GetDamage();
    float GetRateOfFire();
}
public class Weapon:MonoBehaviour {
    public AttackType attackType = AttackType.NORMAL_ATTACK;
    public WeaponConfig m_WeaponConfig;
    public ShootPattern m_ShootPattern;
    public Transform m_FirePos;
    public ParticleSystem m_FxHeadGun;
    public ParticleSystem m_FxStartAttack;
    public SkeletonAnimation m_SpineFx;
    public Animator m_EffectAttack;
    public string m_FxAnimName;
    public string m_AttackTriggerName = "Attack";

    [SerializeField] protected float m_CurrentReloadTime;
    [SerializeField] protected float m_AttackRange;
    protected Character m_Owner;
    protected BigNumber m_Damage;
    protected int m_SpreadAmount;
    protected float m_RateOfFire;
    protected float m_BonusAttackSpeed;
    protected float m_DoubleShotChance;
    protected float m_TripleShotChance;
    protected float m_BonusAttackBooster;
    protected List<Buff> m_Buffes = new List<Buff>();
    [SerializeField]protected BulletInfo m_BulletInfo;
    public WeaponDataConfig m_WeaponDataConfig;

    protected event UnityAction onStartShootEvent;
    protected event UnityAction onShootDoneEvent;

    protected bool m_IsActive = true;
    protected bool IsReady = true;
    protected bool IsHoldingFire = false;
    public virtual void InitWeapon(Character owner) {
        m_Owner = owner;
        m_AttackRange = m_WeaponConfig.attackRange;
        if(m_ShootPattern != null) {
            m_ShootPattern.ClearAllEvent();
            m_BulletInfo = new BulletInfo() {
                speed = m_WeaponConfig.bulletSpeed,
                maxSpeed = m_WeaponConfig.bulletMaxSpeed,
                minSpeed = m_WeaponConfig.bulletMinSpeed,
                accel = m_WeaponConfig.bulletAccel,
                targetTeam = m_WeaponConfig.targetTeam,
                ownerTeam = m_WeaponConfig.ownerTeam,
                endEffect = m_WeaponConfig.bulletEndEffectName,
                maxPiercingTarget = m_WeaponConfig.maxPiercingTarget,
                piercing = m_WeaponConfig.piercing,
                isComeBack = m_WeaponConfig.isComeBack,
                prefabName = m_WeaponConfig.bulletPrefabName,
            };
            m_ShootPattern.SetBulletInfo(m_BulletInfo);
            m_ShootPattern.onBulletAppearCallback += PlayHeadGunEffect;
            m_ShootPattern.onShotDoneCallback += DoShootDoneEvent;
        }
        StartCooldown();
    }
    public virtual void InitWeapon(WeaponDataConfig weaponDataConfig,IngameType targetType,Character owner) {
        m_Owner = owner;
        m_WeaponDataConfig = weaponDataConfig;
        m_SpreadAmount = weaponDataConfig.spreadAmount;
        m_AttackRange = weaponDataConfig.attackRange;
        float rateOfFire = 1;
        InitRateOfFire(rateOfFire);
        StartCooldown();
        m_Buffes.Clear();

        if(m_ShootPattern != null) {
            m_ShootPattern.ClearAllEvent();
            //m_BulletInfo = new BulletInfo {
            //    maxPiercingTarget = weaponDataConfig.maxPiercingTarget,
            //    speed = weaponDataConfig.bulletSpeed,
            //    maxSpeed = weaponDataConfig.bulletMaxSpeed,
            //    minSpeed = weaponDataConfig.bulletMinSpeed,
            //    accel = weaponDataConfig.bulletAccel,
            //    damage = weaponDataConfig.damage,
            //    prefabName = weaponDataConfig.bulletPrefabName,
            //    spriteName = weaponDataConfig.bulletSpriteName,
            //    targetTeam = GetTargetTeam(),
            //    ownerTeam = GetOwnerTeam(),
            //    buffes = m_Buffes,
            //    isRotate = weaponDataConfig.isRotate,
            //    isComeBack = weaponDataConfig.isComeBack,
            //    endEffect = weaponDataConfig.bulletEndEffectName,
            //    //criticalChance = m_CritChance,
            //    //criticalDamage = m_CritDamage,
            //    damageInfo = new DamageInfo {
            //        owner = m_Owner,
            //        characterID = m_Owner.m_CharacterID,
            //        classType = m_Owner.m_Class,
            //        damageType = DamageType.NORMAL
            //    }
            //};
            m_ShootPattern.SetBulletInfo(m_BulletInfo);
            m_ShootPattern.onBulletAppearCallback += PlayHeadGunEffect;
            m_ShootPattern.onShotDoneCallback += DoShootDoneEvent;
        }
        InitHeadEffect();
        IsReady = true;
        IsHoldingFire = false;
        m_IsActive = true;
    }
    public void InitRateOfFire(float rateOfFire) {
        m_RateOfFire = rateOfFire;
    }
    public virtual void OnRunning(float deltaTime) {
        OnReloadTimeRunning(deltaTime);
    }
    protected virtual void OnReloadTimeRunning(float deltaTime) {
        if(m_CurrentReloadTime > 0) {
            m_CurrentReloadTime -= deltaTime;
        }
    }
    public virtual bool IsReadyToAttack() {
        return m_CurrentReloadTime <= 0 && IsReady && !IsHoldingFire;
    }
    public virtual void Attack() {
        DoStartShootEvent();
        StopPrefireEffect();
        if(m_ShootPattern) {
            m_ShootPattern.SetWeapon(this);
            m_ShootPattern.SetFireDirectionDelegate(GetFireDirection);
            m_ShootPattern.BulletAmount = GetBulletAmount();
            m_ShootPattern.m_FirePos = m_FirePos;
            m_ShootPattern.m_Damage = GetDamage();
            m_ShootPattern.Shoot();
        }
    }
    public IEnumerator OnShooting() {
        if(IsGoodToDoubleShot()) {
            yield return Yielders.Get(0.1f);
            m_ShootPattern.Shoot();
        }
        if(IsGoodToTripleShot()) {
            yield return Yielders.Get(0.05f);
            m_ShootPattern.Shoot();
        }
    }
    private bool IsGoodToDoubleShot() {
        if(m_DoubleShotChance > 0 && Random.Range(0,100f) < m_DoubleShotChance) {
            return true;
        } else {
            return false;
        }
    }
    private bool IsGoodToTripleShot() {
        if(m_TripleShotChance > 0 && Random.Range(0,100f) < m_TripleShotChance) {
            return true;
        } else {
            return false;
        }
    }
    public virtual void StartCooldown() {
        float num = GetReloadTime();
        m_CurrentReloadTime = m_CurrentReloadTime + GetReloadTime();
        if(m_CurrentReloadTime > num) {
            m_CurrentReloadTime = num;
        }
    }
    public virtual void StartAttack() {
        if(m_EffectAttack != null) {
            m_EffectAttack.SetTrigger(m_AttackTriggerName);
        }
        if(m_FxStartAttack != null) {
            m_FxStartAttack.Play();
        }
    }
    public void SetDamage(BigNumber dmg) {
        m_Damage = dmg;
    }
    public void SetRateOfFire(float value) {
        m_RateOfFire = value;
    }
    public void UpdateStat(MainStatType stat,BigNumber value) {
        switch(stat) {
            case MainStatType.Attack:
                SetDamage(value);
                break;
            case MainStatType.AttackSpeed:
                SetRateOfFire(value.ToFloat());
                break;
            case MainStatType.CriticalChance:
                m_BulletInfo.criticalChance = value.ToFloat();
                break;
            case MainStatType.CriticalDamage:
                m_BulletInfo.criticalDamage = value.ToFloat();
                break;
            case MainStatType.DoubleShotChance:
                m_DoubleShotChance = value.ToFloat();
                break;
            case MainStatType.TripleShotChance:
                m_TripleShotChance = value.ToFloat();
                break;
            default:
                break;
        }
        m_ShootPattern.SetBulletInfo(m_BulletInfo);
    }
    public void SetCitDamage(BigNumber value) {
        m_BulletInfo.criticalDamage = value.ToFloat();
        m_ShootPattern.SetBulletInfo(m_BulletInfo);
    }
    public void SetBonusAttackBooster(float bonus) {
        m_BonusAttackBooster = bonus;
    }
    public void SetBonusAttackSpeed(float bonus) {
        m_BonusAttackSpeed = bonus;
    }
    public void AddBosnusAttackSpeed(float bonus) {
        m_BonusAttackSpeed += bonus;
    }
    public void SubtractBonusAttackSpeed(float bonus) {
        m_BonusAttackSpeed -= bonus;
    }
    public void SetCurrentReloadTime(float time) {
        m_CurrentReloadTime = time;
    }
    public virtual BigNumber GetDamage() {
        //float reduceAttack = m_Owner.GetReduceOutDamage();
        //float bonusAttack = m_Owner.GetBonusOutDamage();
        //BigNumber attackPoint = m_Owner.GetBonusAttackPoint();
        //BigNumber damage = m_Damage + m_Damage * (((m_BonusMultiplier + m_BonusAttackBooster) + bonusAttack - reduceAttack) / 100f) + attackPoint;
        //return damage;
        return m_Damage;
    }
    public virtual float GetReloadTime() {
        //float reduceAttackSpeed = m_Owner.GetReduceAttackSpeed();
        //float bonusAttackSpeed = m_Owner.GetBonusAttackSpeed();
        float num = m_RateOfFire - (m_BonusAttackSpeed ) * m_RateOfFire / 100f;
        //Debug.Log(m_Owner.name + " redu " + reduceAttackSpeed + " num " + num);
        return  num;
    }
    public IngameTeam GetOwnerTeam() {
        return m_Owner.GetTeam();
    }
    public IngameTeam GetTargetTeam() {
        return m_Owner.GetTargetTeam();
    }
    public float GetAttackRange() {
        return m_AttackRange;
    }
    public void InitHeadEffect() {
        if(m_SpineFx != null) {
            m_SpineFx.AnimationState.Complete += AnimationState_End;
            m_SpineFx.gameObject.SetActive(false);
        }
    }
    private void AnimationState_End(Spine.TrackEntry trackEntry) {
        m_SpineFx.gameObject.SetActive(false);
    }
    public void DoStartShootEvent(){
        onStartShootEvent?.Invoke();
    }
    public void DoShootDoneEvent(){
        onShootDoneEvent?.Invoke();
    }
    public void StopPrefireEffect(){
        onShootDoneEvent?.Invoke();
    }
    public virtual void PlayHeadGunEffect() {
        if(m_FxHeadGun != null) {
            m_FxHeadGun.Play();
        }
        if(m_SpineFx != null) {
            m_SpineFx.gameObject.SetActive(true);
            m_SpineFx.AnimationState.SetAnimation(0,m_FxAnimName,false);
        }
    }
    public virtual Vector3 GetFireDirection() {
        IngameObject target = m_Owner.Target;
        if(target == null) {
            return transform.forward;
        } else {
            Vector3 v = target.Transform.position + new Vector3(0,0.5f,0);
            if(m_FirePos != null) {
                Vector3 direct = v - m_FirePos.position;
                return direct.normalized;
            } else {
                Vector3 direct = v - transform.position;
                return direct.normalized;
            }
        }
    }
    public virtual int GetBulletAmount() {
        return m_WeaponConfig.bulletAmout;
    }
    public virtual string GetTriggerName() {
        return m_AttackTriggerName;
    }
}
