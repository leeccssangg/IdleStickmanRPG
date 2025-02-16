using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PropertiesManager : SingletonFree<PropertiesManager>
{
    
    [field: SerializeField] private StickManManager m_StickManManager;
    [field: SerializeField] private SkillManager m_SkillManager;
    [field: SerializeField] private GearManager m_GearManager;
    public StickManManager StickManManager => m_StickManManager;
    public SkillManager SkillManager => m_SkillManager;
    public GearManager GearManager => m_GearManager;
    
    [ShowInInspector]
    public List<IProperty> Properties{ get ; private set; }

    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber TotalAttack { get; private set; }
    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber TotalHp { get; private set; }
    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber AttackSpeed { get; private set; }
    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber CriticalChange { get; private set; }
    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber CriticalDamage { get; private set; }
    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber HpRecovery { get; private set; }
    [ShowInInspector,BoxGroup("Stats")]
    public BigNumber DoubleShootChange { get; private set; }
    public BigNumber TripleShotChance { get; private set; }
    public BigNumber DamageReduction { get; private set; }
    public BigNumber DamageAmplify { get; private set; }
    public BigNumber CoolDownReduction { get; private set; }
    public BigNumber SkillCoolDownReduction { get; private set; }
    
    
    //TODO: Gia tri attack luc chua update attack
    private BigNumber m_Dps = new BigNumber(0);
    
    private float[] m_TimeDelayUpdateProperty = new float[Utilss.GetEnumCount<StatType>()];
    private const float TIME_DELAY_UPDATE_PROPERTY = 0.15f;
    
    protected override void OnAwake(){
        base.OnAwake();
        
    }
    public void Init()
    {
        InitProperties();
        InitSat();
        InitTime();
    }

    private void InitProperties()
    {
        Properties = new List<IProperty>();
        for(int i = 0; i < transform.childCount; i++){
            var property = transform.GetChild(i).GetComponent<IProperty>();
            if(property != null){
                Properties.Add(property);
            }
        }
    }
    private void InitSat()
    {
        UpdateAttack();
        UpdateHp();
        UpdateAttackSpeed();
        UpdateHpRegen();
        UpdateCriticalChance();
        UpdateCriticalDamage();
        
    }

    private void InitTime()
    {
        for (int i = 0; i < m_TimeDelayUpdateProperty.Length; i++)
        {
            m_TimeDelayUpdateProperty[i] = 0;
        }
    }
    #region UPDATE STAT

    private void OnUpDateProperty(StatType statType, BigNumber oldValue, Action<BigNumber> callBack)
    {
        if (m_TimeDelayUpdateProperty[(int)statType] <= Time.time)
        {
            m_TimeDelayUpdateProperty[(int)statType] = Time.time + TIME_DELAY_UPDATE_PROPERTY;

            StartCoroutine(CO_Update(statType, oldValue, callBack));
        }
        else
        {
            m_TimeDelayUpdateProperty[(int)statType] = Time.time + TIME_DELAY_UPDATE_PROPERTY;
        }
    }

    private IEnumerator CO_Update(StatType statType, BigNumber oldValue, Action<BigNumber> callBack)
    {
        while (m_TimeDelayUpdateProperty[(int)statType] >= Time.time)
        {
            yield return null;
        }

        callBack.Invoke(oldValue);
    }
    #endregion
    public void UpdateAttack()
    {
        OnUpDateProperty(StatType.ATTACK, TotalAttack,Calculate);

        void Calculate(BigNumber stat)
        {
            //Skill + WeaponGear + StickMan + MainStat
            BigNumber attack = GetTotalProperty<IAttack>(x => ((IAttack)x).GetAttack());
        
            //Get Owned Attack Effect
            //BigNumber ownedAttackEffect = FormulaManager.Ins.GetOwnedAttackEffect();
            BigNumber ownedAttackEffect = GetMultiplePropertiesValue<IAttackOwnerEffect>(x => ((IAttackOwnerEffect)x).GetAttackOwnerEffect());
            BigNumber equippedAttackEffect = GetMultiplePropertiesValue<IAttackEquippedEffect>(x => ((IAttackEquippedEffect)x).GetAttackEquippedEffect());

            TotalAttack = attack * ownedAttackEffect * equippedAttackEffect;
            //Debug.Log($"' Attack : {attack} _ {ownedAttackEffect} _ {equippedAttackEffect}");
        
            //Debug.Log($"Total Attack: {TotalAttack}");
            IngameManager.Ins.UpdateStat(MainStatType.Attack);
            UpdateDps();
            UIManager.Ins.GetUI<UIPanelPlayerStat>().UpdateUIStat(MainStatType.Attack);
        }
    }
    public void UpdateAttackSpeed()
    {
        BigNumber attackSpeed = GetTotalProperty<IAttackSpeed>(x => (x as IAttackSpeed).GetAttackSpeed());
        AttackSpeed = attackSpeed;
        IngameManager.Ins.UpdateStat(MainStatType.AttackSpeed);
        UpdateDps();
        UIManager.Ins.GetUI<UIPanelPlayerStat>().UpdateUIStat(MainStatType.AttackSpeed);
        Debug.Log($"Total Attack Speed: {attackSpeed}");
    }
    
    public void UpdateHp()
    {
        BigNumber hp = GetTotalProperty<IHp>(x => ((IHp)x).GetHp());
        
        BigNumber ownedHpEffect = GetMultiplePropertiesValue<IHpOwnerEffect>(x => ((IHpOwnerEffect)x).GetHpOwnerEffect());
        BigNumber equippedHpEffect = GetMultiplePropertiesValue<IHpEquippedEffect>(x => ((IHpEquippedEffect)x).GetHpEquippedEffect());
        TotalHp = hp * (ownedHpEffect + equippedHpEffect);
        Debug.Log($"{hp}--{ownedHpEffect}--{equippedHpEffect}");
        IngameManager.Ins.UpdateStat(MainStatType.HP);
        UIManager.Ins.GetUI<UIPanelPlayerStat>().UpdateUIStat(MainStatType.HP);
        
        Debug.Log($"Total Hp: {hp}");   
    }
    public void UpdateHpRegen()
    {
        BigNumber hpRegen = GetTotalProperty<IHpRegen>(x => (x as IHpRegen).GetHpRegen());
        HpRecovery = hpRegen;
        IngameManager.Ins.UpdateStat(MainStatType.HPRegen);
        UIManager.Ins.GetUI<UIPanelPlayerStat>().UpdateUIStat(MainStatType.HPRegen);
        Debug.Log($"Total Hp Regen: {hpRegen}");
    }

    public void UpdateCriticalChance()
    {
        BigNumber criticalChance = GetTotalProperty<ICriticalChance>(x => (x as ICriticalChance).GetCriticalChance());
        CriticalChange = criticalChance;
        
        IngameManager.Ins.UpdateStat(MainStatType.CriticalChance);
        UpdateDps();
        UIManager.Ins.GetUI<UIPanelPlayerStat>().UpdateUIStat(MainStatType.CriticalChance);
        
        Debug.Log($"Total Critical Chance: {criticalChance}");
    }

    public void UpdateCriticalDamage()
    {
        BigNumber criticalDamage = GetTotalProperty<ICriticalDamage>(x => (x as ICriticalDamage).GetCriticalDamage());
        CriticalDamage = criticalDamage;
        
        IngameManager.Ins.UpdateStat(MainStatType.CriticalDamage);
        UpdateDps();
        UIManager.Ins.GetUI<UIPanelPlayerStat>().UpdateUIStat(MainStatType.CriticalDamage);
    }
    
    private void UpdateDps()
    {
        if (m_Dps <= BigNumber.ZERO)
        {
            m_Dps = GetDps();
        }else{
            BigNumber newDps = GetDps();
            BigNumber changeValue = newDps - m_Dps;
            UIManager.Ins.GetUI<UIPanelIngame>().PanelPower.ChangePowerValue(newDps,changeValue);
        }
        
    }

    private BigNumber GetDps()
    {
        return (CriticalChange * 0.01f * CriticalDamage + 1) * TotalAttack * AttackSpeed;

    }
    public void UpdateHeroStat()
    {
        
    }
    public delegate BigNumber PropertyDelegate(IProperty property);
    private BigNumber GetTotalProperty<T>(PropertyDelegate action) where T : IProperty
    {

        BigNumber total = 0;
        for (int i = 0; i < Properties.Count; i++)
        {
            if (Properties[i] is T)
            {
                total += action.Invoke(Properties[i]);

            }
        }

        return total;
    }
    private BigNumber GetMultiplePropertiesValue<T>(PropertyDelegate _delegate) where T : IProperty
    {
        BigNumber value =1;
        for (int i = 0; i < Properties.Count; i++)
        {
            IProperty property = Properties[i];
            if(property is T){
                value *= _delegate.Invoke(property).ToRate();   
            }
        }
        return value;
    }
}
public static class Extend 
{
    public static BigNumber ToRate(this BigNumber t) 
    {
        return 1 + t * 0.01f;
    }
}