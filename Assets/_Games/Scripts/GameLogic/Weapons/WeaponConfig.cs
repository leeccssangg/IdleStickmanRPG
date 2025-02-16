using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponConfig",menuName = "Configs/WeaponConfig")]
public class WeaponConfig:ScriptableObject {
    public int bulletAmout = 1;
    public float bulletSpeed;
    public float bulletMaxSpeed;
    public float bulletMinSpeed;
    public float bulletAccel;
    public float attackRange;
    public int maxPiercingTarget;
    public int piercing;
    public bool isComeBack;
    public string bulletPrefabName;
    public string bulletEndEffectName;
    public IngameTeam targetTeam;
    public IngameTeam ownerTeam;
    public List<StatConfig> statConfigs = new List<StatConfig>();
}
