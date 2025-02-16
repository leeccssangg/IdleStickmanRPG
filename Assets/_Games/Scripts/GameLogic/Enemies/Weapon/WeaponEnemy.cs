using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class WeaponEnemy : Weapon
{
    public int m_BulletAmount;
    public override void InitWeapon( WeaponDataConfig weaponDataConfig, IngameType targetType, Character owner ) {
        InitBulletInfo();
        m_Owner = owner;
        InitRateOfFire(0.5f);
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
        StartCooldown();
    }
    public virtual void InitBulletInfo() {
        m_BulletInfo.maxPiercingTarget = 1;
        m_BulletInfo.ownerTeam = IngameTeam.Team2;
        m_BulletInfo.targetTeam = IngameTeam.Team1;
    }
    public override int GetBulletAmount() {
        return m_BulletAmount;
    }
    public override BigNumber GetDamage() {
        return m_Damage;
    }
    public override float GetReloadTime(){
        float num = m_RateOfFire;
        //Debug.Log(m_Owner.name + " redu " + reduceAttackSpeed + " num " + num);
        return  num;
    }
}
