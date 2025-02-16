using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStatInfo:MonoBehaviour {
    public MainStatType m_StatType; 

    public Image m_StatImage;
    public TextMeshProUGUI m_TextLevel;
    public TextMeshProUGUI m_TextStatType;
    public TextMeshProUGUI m_TextValue;
    public TextMeshProUGUI m_TextPrice;

    public ButtonUpgrade m_ButtonUpgrade;
    
    private BigNumber m_UpgradePrice;
    private MainStatConfig m_StatsConfig;
    private StatData m_StatsData;

    private void Start() {
        m_ButtonUpgrade.AddOnClickListener(OnUpgrade);
    }
    private void Update(){
        bool able = UpgradeAble();
        var color = able ? Color.white : Color.red;
        m_TextPrice.color = color;
        m_ButtonUpgrade.SetUpgradeAble(UpgradeAble());
    }
    public void SetUp(MainStatType statType) {
        m_StatType = statType;
        m_TextStatType.text = m_StatType.ToString();
        m_StatImage.sprite = SpriteManager.Ins.GetMainStatSprite(m_StatType);
        UpdateUI();
    }
    public void UpdateUI() {
        m_StatsConfig = StatManager.Ins.GetStatConfig(m_StatType);
        m_StatsData = StatManager.Ins.GetStatData(m_StatType);
        int level = 1;
        level = m_StatsData.Level;
        m_TextLevel.text = $"Lv.{level}";
        
        BigNumber value = GetStatValue();
        string valueText = m_StatType switch {
            MainStatType.Attack => value.ToString3(),
            MainStatType.AttackSpeed => Math.Round(value.ToDouble(),2) .ToString(),
            MainStatType.HP => value.ToString3(),
            MainStatType.HPRegen => value.ToString3(),
            MainStatType.CriticalChance => $"{Math.Round(value.ToDouble(),2) .ToString()}%",
            MainStatType.CriticalDamage =>$"{value.ToString3()}%",
            MainStatType.DoubleShotChance => $"{Math.Round(value.ToDouble(),2) .ToString()}%",
            MainStatType.TripleShotChance => $"{Math.Round(value.ToDouble(),2) .ToString()}%",
            MainStatType.CoolDownRedution => value.ToString3(),
            MainStatType.DamageAmplify => value.ToString3(),
            MainStatType.DamamgeReduction => value.ToString3(),
            _ => "0"
        };
        
        m_TextValue.text = valueText;
        UpdatePrice();
    }

    // private BigNumber GetStatValue() {
    //     return m_StatType switch {
    //         MainStatType.Attack => PropertiesManager.Ins.TotalAttack,
    //         MainStatType.AttackSpeed => PropertiesManager.Ins.AttackSpeed,
    //         MainStatType.HP => PropertiesManager.Ins.TotalHp,
    //         MainStatType.HPRegen => PropertiesManager.Ins.HpRecovery,
    //         MainStatType.CriticalChance => PropertiesManager.Ins.CriticalChange,
    //         MainStatType.CriticalDamage => PropertiesManager.Ins.CriticalDamage,
    //         MainStatType.DoubleShotChance => PropertiesManager.Ins.DoubleShootChange,
    //         MainStatType.TripleShotChance => PropertiesManager.Ins.TripleShotChance,
    //         MainStatType.CoolDownRedution => PropertiesManager.Ins.CoolDownReduction,
    //         MainStatType.DamageAmplify => PropertiesManager.Ins.DamageAmplify,
    //         MainStatType.DamamgeReduction => PropertiesManager.Ins.DamageReduction,
    //         _ => 0
    //     };
    // }
    private BigNumber GetStatValue() {
        return m_StatType switch {
            MainStatType.Attack => StatManager.Ins.GetAttack(),
            MainStatType.AttackSpeed => StatManager.Ins.GetAttackSpeed(),
            MainStatType.HP => StatManager.Ins.GetHp(),
            MainStatType.HPRegen => StatManager.Ins.GetHpRegen(),
            MainStatType.CriticalChance => StatManager.Ins.GetCriticalChance(),
            MainStatType.CriticalDamage => StatManager.Ins.GetCriticalDamage(),
            MainStatType.DoubleShotChance => PropertiesManager.Ins.DoubleShootChange,
            MainStatType.TripleShotChance => PropertiesManager.Ins.TripleShotChance,
            MainStatType.CoolDownRedution => PropertiesManager.Ins.CoolDownReduction,
            MainStatType.DamageAmplify => PropertiesManager.Ins.DamageAmplify,
            MainStatType.DamamgeReduction => PropertiesManager.Ins.DamageReduction,
            _ => 0
        };
    }
    public void UpdatePrice() {
        BigNumber price = StatManager.Ins.GetStatCost(m_StatsData);
        //price = 100;
        m_TextPrice.text = price.ToString3();
    }
    private void GetUpgradePrice() {

    }
    private void SetUpgradePriceText() {
        m_TextPrice.text = m_UpgradePrice.ToString3();
    }
    
    public void OnUpgrade() {
        if(!UpgradeAble()) return;
        StatManager.Ins.Upgrade(m_StatType);
        UpdateUI();
    }
    private bool UpgradeAble() {
        return StatManager.Ins.UpgradeAble(m_StatsData,m_StatsConfig);
    }
}
