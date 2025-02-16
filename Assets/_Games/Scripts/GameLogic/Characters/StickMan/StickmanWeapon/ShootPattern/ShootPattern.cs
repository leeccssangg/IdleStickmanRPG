using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public delegate Vector3 GetFireDirectionDelegate();
public abstract class ShootPattern : MonoBehaviour {
    [HideInInspector]public BulletInfo m_BulletInfo;
    protected Weapon m_Weapon;
    public BigNumber m_Damage;
    public Transform m_FirePos;
    protected int m_BulletAmount;
    public float m_FireRate = 0.2f;
    public event UnityAction onShotDoneCallback;
    public event UnityAction onStartShootCallback;
    public event UnityAction onBulletAppearCallback;
    public event UnityAction onPreFireDoneCallback;
    public GetFireDirectionDelegate m_GetFireDirection;
    public UnityAction m_SpecialCallback;
    public UnityAction m_BulletImpactCallback;
    protected string m_BulletSpriteName = "";
    public Vector3 TargetPos{ get; set; }
    public int BulletAmount{ get => m_BulletAmount; set => m_BulletAmount = value; }
    public float FireRate => m_FireRate;
    protected Coroutine m_COShooting;
    public abstract void Shoot();
    public virtual void init( int _weaponLevel ) {
    }
    public void SetFireDirectionDelegate( GetFireDirectionDelegate method ) {
        m_GetFireDirection = method;
    }
    public void SetBulletInfo( BulletInfo bif ) {
        m_BulletInfo = bif;
    }
    public void ClearAllEvent() {
        onStartShootCallback = null;
        onBulletAppearCallback = null;
        onShotDoneCallback = null;
    }
    public virtual void StopShot(){
        StopAllCoroutines();
        if(m_COShooting != null) StopCoroutine(m_COShooting);
    }
    public virtual void OnShotDone(){
        onShotDoneCallback?.Invoke();
    }
    public virtual void OnStartShoot(){
        onStartShootCallback?.Invoke();
    }
    public virtual void OnPreFireDone(){
        onPreFireDoneCallback?.Invoke();
    }
    public virtual void OnBulletAppear(){
        onBulletAppearCallback?.Invoke();
    }
    public virtual void SetSpecialCallback( UnityAction specialCallback ) {
        m_SpecialCallback = specialCallback;
    }
    public void SetWeapon( Weapon wp ) {
        m_Weapon = wp;
    }
    public Weapon GetWeapon() {
        return m_Weapon;
    }
    protected virtual void ConfigBullet( Bullet bl ) {
        float _bulletMaxSpeed = m_BulletInfo.maxSpeed;
        float _bulletMinSpeed = m_BulletInfo.minSpeed;
        float _bulletSpeed = m_BulletInfo.speed;
        float _bulletAccel = m_BulletInfo.accel;
        float _chaosValue = m_BulletInfo.chaosValue;
        float _knockBack = m_BulletInfo.knockBack;
        float _critChance = m_BulletInfo.criticalChance;
        float _critDamage = m_BulletInfo.criticalDamage;
        DamageType damageType = DamageType.NORMAL;
        BigNumber _damage = GetDamage();
        if (_chaosValue > 0) {
            float min = _chaosValue;
            float max = _chaosValue + 2;
            float value = Random.Range(min, max);
            _damage = _damage * value;
            damageType = DamageType.CHAOS;
        } else {
            if (_critChance > 0 && Random.Range(0f, 100f) < _critChance) {
                float bonusDamage = 150f + _critDamage;
                _damage = _damage + _damage * bonusDamage / 100f;
                damageType = DamageType.CRIT;
            } else if (GetWeapon() != null) { 
                if (GetWeapon().attackType == AttackType.SKILL_ATTACK) {
                    damageType = DamageType.SKILL;
                }
            }
        }
        _damage = _damage * Random.Range(0.95f, 1.05f);
        // bl.InitBullet(_damage, m_BulletInfo.targetTeam, m_BulletInfo.ownerTeam, m_BulletInfo.spriteName);
        bl.InitBullet(_damage, m_BulletInfo.targetTeam, m_BulletInfo.ownerTeam, "");
        bl.InitSpeed(_bulletSpeed, _bulletMaxSpeed, _bulletMinSpeed, _bulletAccel);
        bl.InitPiercingValue(m_BulletInfo.piercing);
        bl.InitMaxPiercingTarget(m_BulletInfo.maxPiercingTarget);
        bl.InitGrowUpValue(m_BulletInfo.grow);
        bl.m_KnockBack = _knockBack;
        //bl.m_DamageInfo = m_BulletInfo.damageInfo;
        //bl.m_DamageInfo.damageType = damageType;
        bl.IsRotate = m_BulletInfo.isRotate;
        bl.IsComeBack = m_BulletInfo.isComeBack;
        //if(GetWeapon() !=null) bl.m_OwnerID = GetWeapon().m_Owner.ID;
        bl.m_EndEffect = m_BulletInfo.endEffect;

        for (int i = 0; i < m_BulletInfo.buffes.Count; i++) {
            Buff buff = m_BulletInfo.buffes[i];
            bl.AddBuff(buff);
        }
    }
    public void SetBulletAppearCallback( UnityAction callback ) {
        onBulletAppearCallback = callback;
    }
    public void SetShotDoneCallback( UnityAction callback ) {
        onShotDoneCallback = callback;
    }
    public void SetStartShootCallback( UnityAction callback ) {
        onStartShootCallback = callback;
    }
    public void SetBulletImpactCallback( UnityAction callback ) {
        m_BulletImpactCallback = callback;
    }
    public void SetDamage(BigNumber damage) {
        m_Damage = damage;
    }
    protected virtual Vector3 GetFirePos(){
        return m_FirePos == null ? transform.position : m_FirePos.position;
    }
    public virtual Vector3 GetFireDirection() {
        return m_GetFireDirection();
    }
    public virtual int GetBulletAmount() {
        return BulletAmount;
    }
    public virtual BigNumber GetDamage() {
        return m_Damage;
    }
    protected virtual string GetBulletSpriteName( string root, int level ) {
        return "";
    }
    public virtual void Reset() {
    }
    public IngameTeam GetOwnerTeam() {
        return GetWeapon().GetOwnerTeam();
    }
}