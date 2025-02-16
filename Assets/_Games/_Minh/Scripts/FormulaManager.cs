using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;


public class FormulaManager : SingletonFree<FormulaManager> {

    // #region SINGLETON
    // private static FormulaManager Ins;
    // public static FormulaManager Ins {
    //     get
    //     {
    //         if(Ins == null)
    //         {
    //             Ins = GameObject.FindObjectOfType<FormulaManager>();
    //         }
    //         return Ins;
    //     }
    // } 
    // #endregion

    public StatCostConfig m_StatCostConfig;
    
    // private void Awake() {
    //     Ins = this;
    //     DontDestroyOnLoad(gameObject);
    // }

    #region FUNCTION
    public BigNumber GetProfitByLevel(int level) {
        CostConfig profitConfig = m_StatCostConfig.campaignStatConfig.campaignProfit;
        return CalculateByPerWithDownrate(level, profitConfig.baseStat, profitConfig.rate, profitConfig.minRate, profitConfig.prate, profitConfig.prate2);
    }
    public BigNumber GetEnemyHP(int level) {
        CostConfig config = m_StatCostConfig.campaignStatConfig.enemyHP;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    }
    public BigNumber GetEnemyAttack(int level) {
        CostConfig config = m_StatCostConfig.campaignStatConfig.enemyAttack;
        return CalculateByPerWithDownrate(level,config.baseStat,config.rate,config.minRate,config.prate,config.prate2);
    }
    #region VALUE COST
    //TODO: VALUE COST
    public BigNumber GetAttackCost(int level) {
        CostConfig config = m_StatCostConfig.playerStatCostConfig.attackCost;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    }
    public BigNumber GetAttackSpeedCost(int level) {
        CostConfig config = m_StatCostConfig.playerStatCostConfig.attackSpeedCost;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    }
    public BigNumber GetHpCost(int level) {
        CostConfig config = m_StatCostConfig.playerStatCostConfig.hpCost;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    }
    public BigNumber GetHpRegenCost(int level) {
        CostConfig config = m_StatCostConfig.playerStatCostConfig.hpRegenCost;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    }
    public BigNumber GetCriticalChanceCost(int level) {
        CostConfig config = m_StatCostConfig.playerStatCostConfig.criticalChanceCost;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    }
    public BigNumber GetCriticalDamageCost(int level) {
        CostConfig config = m_StatCostConfig.playerStatCostConfig.criticalDamageCost;
        return CalculateByPerWithDownrate(level, config.baseStat, config.rate, config.minRate, config.prate, config.prate2);
    } 
    #endregion

    #region OWNED EFFECT
    //TODO: OWNED EFFECT
    public BigNumber GetTurretOwnedEffect(int level, Rarity rarity) {
        CostConfig config = m_StatCostConfig.GetStickmanOwnedEffect(rarity);
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetSkillOwnedEffect(int level, Rarity rarity) {
        CostConfig config = m_StatCostConfig.GetSkillOwnedEffect(rarity);
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetEquipmentOwnedEffect(int level, Rarity rarity) {
        CostConfig config = m_StatCostConfig.GetEquipmentOwnedEffect(rarity);
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetStickmanOwnedEffect(int level, Rarity rarity) {
        CostConfig config = m_StatCostConfig.GetStickmanOwnedEffect(rarity);
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    #endregion
    #region EQUIP EFFECT
    //TODO:  EQUIP EFFECT
    public BigNumber GetEquipmentEquipEffect(int level, BigNumber baseStat)
    {
        CostConfig config = m_StatCostConfig.EquipmentEquipEffect;
        return CalculateByPercentage(level, baseStat, config.rate);
    }
    #endregion
    #region PLAYER STAT
    public BigNumber GetPlayerHP(int level)
    {
        CostConfig config = m_StatCostConfig.playerStat.playerHP;
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetPlayerHPRegen(int level)
    {
        BigNumber num = GetPlayerHP(level);
        return num * 0.08f;
    }
    public BigNumber GetPlayerAttack(int level)
    {
        CostConfig config = m_StatCostConfig.playerStat.playerAttack;
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetPlayerAttackSpeed(int level)
    {
        CostConfig config = m_StatCostConfig.playerStat.playerAttackSpeed;
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetPlayerCriticalChance(int level)
    {
        CostConfig config = m_StatCostConfig.playerStat.playerCriticalChance;
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    public BigNumber GetPlayerCriticalDamage(int level)
    {
        CostConfig config = m_StatCostConfig.playerStat.playerCriticalDamage;
        return CalculateByPercentage(level, config.baseStat, config.rate);
    }
    #endregion

    #region LEVEL
    //TODO: LEVEL
    public int GetRequiredAmountItem(int Level)
    {
        int value = m_StatCostConfig.GetLevelConfligItem(Level).RequiredAmount;
        return value;
    }
    #endregion

    #region FORMULA

    //TODO: FORMULA
    public static BigNumber CalculateByPercentage(int level, BigNumber baseNum, float rate) {
        float num = (level - 1) * rate / 100f;
        BigNumber num1 = baseNum + baseNum * num;
        return num1;
    }
    public static BigNumber CalculateByPercentageWithMilestones(int level, BigNumber baseNum, float rate, float powerRate) {
        int num0 = (level / 10);
        powerRate = Mathf.Pow(powerRate, num0);
        rate = rate * powerRate;
        float num = (level - 1) * rate / 100f;
        BigNumber num1 = baseNum + baseNum * num;
        return num1;
    }
    public static BigNumber CalculateByPerWithDownrate(int level, BigNumber baseNum, float rate, float minRate, float powerRate, float decreasePowerRate = 0) {
        BigNumber result = baseNum;
        minRate = minRate < 0.5f ? 0.5f : minRate;
        for (int i = 0; i < level; i++) {
            result = result + result * rate / 100f;
            {
                rate = rate - rate * powerRate / 100;
                rate = rate < minRate ? minRate : rate;
            }
            {
                powerRate = powerRate - powerRate * decreasePowerRate / 100;
                powerRate = powerRate < 0.2f ? 0.2f : powerRate;
            }
        }
        return result;
    }
    public static BigNumber CalculateByProgression(int level, BigNumber baseNum, float rate, float rate2) {
        BigNumber result = baseNum;
        for (int i = 1; i < level; i++) {
            result += rate;
            rate += rate2;
        }
        return result;
    }
    public static BigNumber CalculateStatByPow(int level, float baseCost, float costRate = 50, float powerRate = 1.15f) {
        BigNumber result = ((baseCost + Mathf.Pow(((level - 1) * costRate), powerRate)) / 100) * 100;
        return result;
    }
    public static BigNumber CalculateByPowMultiple(int level, float baseCost, float costRate = 50, float powerRate = 1.15f, float minRate = 100) {
        float num = level / 4.8f + powerRate;
        float num1 = Mathf.Pow(((level - 1) * costRate), num);
        float min = (level - 1) * minRate;
        if (num1 < min && level > 1) num1 = min;
        BigNumber gold = ((baseCost + num1) / 100) * 100;
        return gold;
    }
    #endregion
    #endregion


#if UNITY_EDITOR
    [Header("UNITY_EDITOR")]
    public int level;
    #region Profit
    [BoxGroup("Profit")]
    public float baseProfit;
    [BoxGroup("Profit")]
    public float profitRate;
    [BoxGroup("Profit")]
    public float profitPowerRate;
    [BoxGroup("Profit")]
    public float profitPowerRate2;
    #endregion

    #region Cost
    [BoxGroup("Cost")]
    public float baseCost;
    [BoxGroup("Cost")]
    public float costRate;
    [BoxGroup("Cost")]
    public float costPowerRate;
    [BoxGroup("Cost")]
    public float costPowerRate2;
    #endregion

    #region HP
    [BoxGroup("HP")]
    public float baseHP;
    [BoxGroup("HP")]
    public float hpRate;
    [BoxGroup("HP")]
    public float hpPowerRate;
    [BoxGroup("HP")]
    public float hpPowerRate2;
    #endregion

    #region DPS
    [BoxGroup("DPS")]
    public float baseAtk;
    [BoxGroup("DPS")]
    public float atkRate;
    [BoxGroup("DPS")]
    public float atkPowerRate;
    [BoxGroup("DPS")]
    public float atkPowerRate2;
    #endregion

    [Button]
    public void Test() {
        ClearLog();
        string s = "Level,Cost,TotalCost,Profit,TotalProfit,HP,DPS" + "\n";
        BigNumber total = 0;
        BigNumber totalProfit = 0;
        string s2 = "";

        for (int i = 1; i <= level; i++) {
            float cProfitRate = profitRate;
            {
                BigNumber num = GetAttackCost(i);
                total += num;
                s += i + "," + num.ToDouble().ToString("F0") + "," + total.ToDouble().ToString("F0");
            }
            {
                BigNumber profit = GetProfitByLevel(i);
                totalProfit += profit;
                s += "," + profit.ToDouble().ToString("F0") + "," + totalProfit.ToDouble().ToString("F0");
            }

            {
                float cRate = hpRate;
                BigNumber HP = CalculateByPerWithDownrate(i, baseHP, cRate, 2, hpPowerRate);
                s += "," + HP.ToDouble().ToString("F0");
            }
            {
                float cRate = atkRate;
                BigNumber DPS = CalculateByProgression(i, baseAtk, cRate,atkPowerRate);
                s += "," + DPS.ToDouble().ToString("F0") + "\n";
                s2 += DPS.ToDouble().ToString("F0") + "\n";
            }
        }
        Debug.Log(s2);
        string path = "Assets/_Game/_Minh/Test1.csv";
        File.WriteAllText(path, s);

    }
    public void ClearLog() //you can copy/paste this code to the bottom of your script
    {
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
    
#endif
}