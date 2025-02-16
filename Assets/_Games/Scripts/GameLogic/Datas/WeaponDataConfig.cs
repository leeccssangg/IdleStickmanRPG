using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataConfig : DeviceConfigData
{
    public float damage;
    public float increaseDamage;
    public int spreadAmount = 1;
    public int maxPiercingTarget;
    public float attackSpeed;
    public float increaseATKSpeed;
    public float bulletSpeed;
    public float bulletMaxSpeed;
    public float bulletMinSpeed;
    public float bulletAccel;
    public float attackRange;
    public bool isRotate;
    public bool isComeBack;
    public string bulletPrefabName;
    public string bulletSpriteName;
    public string bulletEndEffectName;
    //public List<WeaponRankStats> rankStats = new List<WeaponRankStats>();
}
