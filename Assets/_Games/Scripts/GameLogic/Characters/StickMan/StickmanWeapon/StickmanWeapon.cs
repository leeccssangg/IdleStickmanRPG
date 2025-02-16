using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanWeapon:Weapon {
    public override void InitWeapon(WeaponDataConfig weaponDataConfig,IngameType targetType,Character owner) {
        m_WeaponDataConfig = weaponDataConfig;
        m_Owner = owner;
        m_SpreadAmount = weaponDataConfig.spreadAmount;
        //m_AttackRange = weaponDataConfig.attackRange;
        float rateOfFire = m_Owner.GetBaseAttackSpeed();
        InitRateOfFire(rateOfFire);
        StartCooldown();
        m_BulletInfo.targetTeam = IngameTeam.Team2;
        if(m_ShootPattern != null) {
            m_ShootPattern.ClearAllEvent();
            //m_BulletInfo = new BulletInfo {
            //    maxPiercingTarget = weaponDataConfig.maxPiercingTarget,
            //    speed = weaponDataConfig.bulletSpeed,
            //    maxSpeed = weaponDataConfig.bulletMaxSpeed,
            //    minSpeed = weaponDataConfig.bulletMinSpeed,
            //    accel = weaponDataConfig.bulletAccel,
            //    damage = m_Damage,
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
            m_BulletInfo.damageInfo = new DamageInfo {
                owner = m_Owner,
                characterID = m_Owner.m_CharacterID,
                classType = m_Owner.m_Class,
                damageType = DamageType.NORMAL
            };
            m_ShootPattern.SetBulletInfo(m_BulletInfo);
            m_ShootPattern.onBulletAppearCallback += PlayHeadGunEffect;
            m_ShootPattern.onShotDoneCallback += DoShootDoneEvent;
        }
        //InitRankAbility();
        InitHeadEffect();
        IsReady = true;
        IsHoldingFire = false;
        m_IsActive = true;
    }
    public override void Attack() {
        //Debug.Log("Attack " + gameObject.name);
        DoStartShootEvent();
        StopPrefireEffect();
        if(m_ShootPattern) {
            m_ShootPattern.SetWeapon(this);
            m_ShootPattern.SetFireDirectionDelegate(GetFireDirection);
            m_ShootPattern.m_FirePos = m_FirePos;
            m_ShootPattern.m_Damage = GetDamage();
            m_ShootPattern.BulletAmount = GetBulletAmount();
            m_ShootPattern.Shoot();
            StartCoroutine(OnShooting());
        }
    }
    // public override float GetReloadTime() {
    //     float num = m_RateOfFire;
    //     return num;
    // }
    public override BigNumber GetDamage() {
        BigNumber damage = m_Damage + m_Damage * (m_BonusAttackBooster) / 100;
        return damage;
    }
}
