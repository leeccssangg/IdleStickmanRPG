using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TW;

public interface IProperty {}

//Attack
public interface IAttack : IProperty
{
    public BigNumber GetAttack();
}
public interface IAttackOwnerEffect : IProperty
{
    public BigNumber GetAttackOwnerEffect();
}
public interface IAttackEquippedEffect : IProperty
{
    public BigNumber GetAttackEquippedEffect();
}
public interface IBonusAttack : IProperty
{
    public BigNumber GetBonusAttack();
}
///////  HP
public interface IHp : IProperty
{
    public BigNumber GetHp();
}
public interface IHpOwnerEffect : IProperty
{
    public BigNumber GetHpOwnerEffect();
}
public interface IHpEquippedEffect : IProperty
{
    public BigNumber GetHpEquippedEffect();
}
public interface IBonusHp : IProperty
{
    public BigNumber GetBonusHp();
}
public interface IHpRegen : IProperty
{
    public BigNumber GetHpRegen();
}
//Attack Speed  
public interface IAttackSpeed : IProperty
{
    public BigNumber GetAttackSpeed();
}
//Critical Chance
public interface ICriticalChance : IProperty
{
    public BigNumber GetCriticalChance();
}
public interface ICriticalDamage : IProperty
{
    public BigNumber GetCriticalDamage();
}
//Other
public interface ISkillCooldown : IProperty
{
    public BigNumber GetSkillCooldown();
}
public interface ISkillDamage : IProperty
{
    public BigNumber GetSkillDamage();
}
public interface IDamageReduce : IProperty
{
    public BigNumber GetDamageReduce();
}
public interface IGoldObtain : IProperty
{
    public BigNumber GetGoldObtain();
}
public interface ITimeReduceResearchMine : IProperty
{
    int GetTimeReduceResearchMine();
}