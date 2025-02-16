using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackType { NORMAL_ATTACK = 0, SKILL_ATTACK = 1 ,NONE = 2}
[System.Serializable]
public class AttackInfo 
{
    public int attackIndex;
    public AttackType attackType;
}
