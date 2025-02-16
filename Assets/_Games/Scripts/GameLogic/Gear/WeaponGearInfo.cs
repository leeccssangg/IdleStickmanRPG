using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponGearInfo : ItemGearInfo
{
    private WeaponGearConfig m_Config;

    public WeaponGearConfig Config
    {
        get => m_Config;
        private set => m_Config = value;
    }

    public void Init(GearData data, WeaponGearConfig config)
    {
        m_Data = data;
        Config = config;
        
        m_Id = m_Data.i;
        
        Rarity = Config.rarity;
        m_ItemName = Config.gearName;
        Type = Config.gearType;
    }
    public bool IsEquipped()
    {
        WeaponGearInfo equippedWeapon = GearManager.Ins.WeaponGearManager.GetEquippedWeaponInfo();
        if(equippedWeapon == null) return false;
        return Id == equippedWeapon.Id;
    }
    public BigNumber GetEquippedEffect()
    {
        
        return FormulaManager.Ins.GetEquipmentEquipEffect(Level,Config.equippedBaseValue);
    }
    public BigNumber GetOwnedEffect()
    {
        return FormulaManager.Ins.GetEquipmentOwnedEffect(Level,Rarity);
    }
}
[System.Serializable]
public class ArmorGearInfo : ItemGearInfo
{
    private ArmorGearConfig m_Config;
    public ArmorGearConfig Config{ get => m_Config;private set => m_Config = value; }
    public void Init(GearData data, ArmorGearConfig config)
    {
        m_Data = data;
        Config = config;
        
        m_Id = m_Data.i;
        
        Rarity = Config.rarity;
        m_ItemName = Config.gearName;
        Type = Config.gearType;
        
    }
    public bool IsEquipped()
    {
        ArmorGearInfo equippedWeapon = GearManager.Ins.ArmorGearManager.GetEquippedInfo();
        if(equippedWeapon == null) return false;
        return Id == equippedWeapon.Id;
    }
    public BigNumber GetEquippedEffect()
    {
        return FormulaManager.Ins.GetEquipmentEquipEffect(Level,Config.equippedBaseValue);
    }
    public BigNumber GetOwnedEffect()
    {
        return FormulaManager.Ins.GetEquipmentOwnedEffect(Level,Rarity);
    }
}

