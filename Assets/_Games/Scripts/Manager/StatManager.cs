using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class StatManager:SingletonFree<StatManager>,IAttack,IAttackSpeed,ICriticalChance,ICriticalDamage,IHp,IHpRegen {

    
    [SerializeField]private List<StatData> m_StatDataList = new List<StatData>();
    [SerializeField]private StatGlobalConfig m_StatGlobalConfig;
    public StatsData m_StatsData;


    public List<StatData> StatDataList { get => m_StatDataList; set => m_StatDataList = value; }
    public StatGlobalConfig StatGlobalConfig { get => m_StatGlobalConfig; set => m_StatGlobalConfig = value; }

    #region SaveLoadData
    public void LoadData(StatsData data) {
        m_StatsData = data;
        if(m_StatsData.StatDataList.Count <= 0) {
            CreateNewData();
        } else {
            LoadOldData();
        }
    }
    private void CreateNewData() {
        int count  = StatGlobalConfig.mainStatsConfigs.Count;
        for(int i = 0;i < count;i++) {
            MainStatConfig stc = StatGlobalConfig.mainStatsConfigs[i];
            StatData data  = new();
            data.Init(stc.statID,stc.statType);
            m_StatDataList.Add(data);
        }
        SaveData();
    }
    private void LoadOldData() {
        m_StatDataList.Clear();
        for(int i = 0;i < m_StatsData.StatDataList.Count;i++) {
            StatData data = m_StatsData.StatDataList[i];
            m_StatDataList.Add(data);  
        }
    }
    public void SaveData() {
        List<StatData> list = new();
        for(int i = 0;i < m_StatDataList.Count;i++) {
            StatData data = m_StatDataList[i];
            list.Add(data);
        }
        StatsData dataSave = new StatsData() {
            StatDataList = list
        };
        ProfileManager.Ins.SaveStatData(dataSave);
    }
    #endregion
    #region Execute
    public StatData GetStatData(MainStatType statType) {
        for(int i = 0;i < m_StatDataList.Count;i++) {
            StatData std = m_StatDataList[i];
            if(std.st == statType) {
                return std;
            }
        }
        return null;
    }
    public bool Upgrade(MainStatType st){
        StatData stat = GetStatData(st);
        MainStatConfig stc = GetStatConfig(st);
        var price = GetStatCost(stat);
        if(UpgradeAble(stat,stc)){
            stat.levelUp();
            IngameManager.Ins.UpdateStat(st);
            ProfileManager.PlayerData.ConsumeGameResource(ResourceData.ResourceType.GOLD,price);
            DoUpdateStat(st);
            SaveData();
            return true;
        }
        return false;
    }
    public bool UpgradeAble(StatData data,MainStatConfig stc){
        var cost = GetStatCost(data);
        var isEnough = ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.GOLD, new BigNumber(cost));
        
        var maxlevel = stc.maxLevel;
        var isMaxLevel = maxlevel >= 0 && data.Level >= stc.maxLevel ;
        return isEnough && !isMaxLevel;
        // return true;
    }
    private void DoUpdateStat(MainStatType st) {
        switch(st) {
            case MainStatType.none:
                break;
            case MainStatType.Attack:
                PropertiesManager.Ins.UpdateAttack();
                break;
            case MainStatType.AttackSpeed:
                PropertiesManager.Ins.UpdateAttackSpeed();
                break;
            case MainStatType.HP:
                PropertiesManager.Ins.UpdateHp();
                break;
            case MainStatType.HPRegen:
                PropertiesManager.Ins.UpdateHpRegen();
                break;
            case MainStatType.CriticalChance:
                PropertiesManager.Ins.UpdateCriticalChance();
                break;
            case MainStatType.CriticalDamage:
                PropertiesManager.Ins.UpdateCriticalDamage();
                break;
            case MainStatType.DoubleShotChance:
                break;
            case MainStatType.TripleShotChance:
                break;
            case MainStatType.DamamgeReduction:
                break;
            case MainStatType.DamageAmplify:
                break;
            case MainStatType.CoolDownRedution:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(st), st, null);
        }
    }
#region  GET
     public MainStatConfig GetStatConfig(MainStatType st) {
        return StatGlobalConfig.GetStatConfig(st);
    }
    public BigNumber GetStatValue(StatData stat) {
        return GetStatConfig(stat.st).GetValue(stat.Level);
    }

    private BigNumber GetStatValue(MainStatType stat) {
        
        var statData = GetStatData(stat);
        var level = statData.Level;
        switch (stat)
        {
            case MainStatType.none:
                break;
            case MainStatType.Attack:
                return FormulaManager.Ins.GetPlayerAttack(level);
            case MainStatType.AttackSpeed: 
                return FormulaManager.Ins.GetPlayerAttackSpeed(level)/100;
                // return GetStatConfig(stat).GetValue(level);
            case MainStatType.HP:
                return FormulaManager.Ins.GetPlayerHP(level);
            case MainStatType.HPRegen:
                return FormulaManager.Ins.GetPlayerHPRegen(level);
            case MainStatType.CriticalChance:
                return FormulaManager.Ins.GetPlayerCriticalChance(level);
            case MainStatType.CriticalDamage:
                return FormulaManager.Ins.GetPlayerCriticalDamage(level);
            case MainStatType.DoubleShotChance:
                break;
            case MainStatType.TripleShotChance:
                break;
            case MainStatType.DamamgeReduction:
                break;
            case MainStatType.DamageAmplify:
                break;
            case MainStatType.CoolDownRedution:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }

        return 0;
    }
    public BigNumber GetStatValue2(StatData stat) {
        int level = stat.Level;
        BigNumber value = 0;
        switch(stat.st) {
            case MainStatType.none:
                break;
            case MainStatType.Attack:
                return FormulaManager.Ins.GetPlayerAttack(level);
            case MainStatType.AttackSpeed:
                //value = FormulaManager.Instance.GetPlayerAttackSpeed(level);
                value = GetStatConfig(stat.st).GetValue(level);
                break;
            case MainStatType.HP:
                value = FormulaManager.Ins.GetPlayerHP(level);
                break;
            case MainStatType.HPRegen:
                value = FormulaManager.Ins.GetPlayerHPRegen(level);
                break;
            case MainStatType.CriticalChance:
                value = GetStatValue(stat);
                break;
            case MainStatType.CriticalDamage:
                value = GetStatValue(stat);
                break;
            case MainStatType.DoubleShotChance:
                value = GetStatConfig(stat.st).GetValue(level);
                break;
            case MainStatType.TripleShotChance:
                value = GetStatConfig(stat.st).GetValue(level);
                break;
            case MainStatType.DamamgeReduction:
                value = GetStatConfig(stat.st).GetValue(level);
                break;
            case MainStatType.DamageAmplify:
                value = GetStatConfig(stat.st).GetValue(level);
                break;
            case MainStatType.CoolDownRedution:
                value = GetStatConfig(stat.st).GetValue(level);
                break;
            default:
                break;
        }
        return value;
    }
    public BigNumber GetStatCost(StatData stat) {
        int level = stat.Level;
        BigNumber value = 0;
        switch(stat.st) {
            case MainStatType.none:
                break;
            case MainStatType.Attack:
                return FormulaManager.Ins.GetAttackCost(level);
            case MainStatType.AttackSpeed:
                value = FormulaManager.Ins.GetAttackSpeedCost(level);
                break;
            case MainStatType.HP:
                value = FormulaManager.Ins.GetHpCost(level);
                break;
            case MainStatType.HPRegen:
                value = FormulaManager.Ins.GetHpRegenCost(level);
                break;
            case MainStatType.CriticalChance:
                value = FormulaManager.Ins.GetCriticalChanceCost(level);
                break;
            case MainStatType.CriticalDamage:
                value = FormulaManager.Ins.GetCriticalDamageCost(level);
                break;
            case MainStatType.DoubleShotChance:
                value = 100;
                break;
            case MainStatType.TripleShotChance:
                value = 100;
                break;
            case MainStatType.DamamgeReduction:
                value = 100;
                break;
            case MainStatType.DamageAmplify:
                value = 100;
                break;
            case MainStatType.CoolDownRedution:
                value = 100;
                break;
            default:
                break;
        }
        return value;
    }
#endregion
    
    #endregion
    public BigNumber GetAttack()
    {
        return GetStatValue(MainStatType.Attack);
    }

    public BigNumber GetAttackSpeed()
    {
        return GetStatValue(MainStatType.AttackSpeed);
    }

    public BigNumber GetCriticalChance()
    {
        return GetStatValue(MainStatType.CriticalChance);
    }

    public BigNumber GetCriticalDamage()
    {
        return GetStatValue(MainStatType.CriticalDamage);
    }

    public BigNumber GetHp()
    {
        return GetStatValue(MainStatType.HP);
    }

    public BigNumber GetHpRegen()
    {
        return GetStatValue(MainStatType.HPRegen);
    }
    #region Editor
    public TextAsset m_TextCSV;
#if UNITY_EDITOR
    [Button]
    public void ImportCSVData() {
        ImportStatCofigCSV();
        EditorUtility.SetDirty(gameObject);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }

    private void ImportStatCofigCSV() {
        StatGlobalConfig = StatGlobalConfig.Instance;
        EditorUtility.SetDirty(StatGlobalConfig);
        StatGlobalConfig.Load(m_TextCSV);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
    #endregion

   
}
[System.Serializable]
public class StatsData {
    public List<StatData> std = new();
    public List<StatData> StatDataList { get => std; set => std = value; }
}
[System.Serializable]
public class Stat{
   public StatType m_StatType;
   public BigNumber m_Value;
   
   public StatType StatType { get => m_StatType;  set => m_StatType = value; }
   public BigNumber Value { get => m_Value;  set => m_Value = value; }

   public Stat() { }
   public Stat(StatType statType,BigNumber value) {
       StatType = statType;
       Value = value;
   }
   
   public string GetStatString() {
       string str = "";
       switch (StatType)
       {
           case StatType.NONE:
               break;
           case StatType.ATTACK:
               str = "ATK";
               break;
           case StatType.ATTACK_SPEED:
                str = "ATK SPD";
               break;
           case StatType.HP:
                str = "HP";
               break;
           case StatType.HP_REGEN:
                str = "HP REGEN";
               break;
           case StatType.CRITICAL_CHANCE:
                str = "CRIT CHANCE";
               break;
           case StatType.CRITICAL_DAMAGE:
                str = "CRIT DMG";
               break;
           default:
               throw new ArgumentOutOfRangeException();
       }

       return str;
   }
}
public enum StatType {
    NONE = 0,
    ATTACK = 1,
    ATTACK_SPEED = 2,
    HP = 3,
    HP_REGEN = 4,
    CRITICAL_CHANCE = 5,
    CRITICAL_DAMAGE = 6,
}
public enum MainStatType {
    none = 0,
    Attack = 1,
    AttackSpeed = 2,
    HP = 3,
    HPRegen = 4,
    CriticalChance = 5,
    CriticalDamage = 6,
    DoubleShotChance = 7,
    TripleShotChance = 8,
    DamamgeReduction = 9,
    DamageAmplify = 10,
    CoolDownRedution = 11,
}
