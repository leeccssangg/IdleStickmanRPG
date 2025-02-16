using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponGearManager : MonoBehaviour
{
    [SerializeField] private GearsData m_Data;
    [SerializeField] private List<WeaponGearConfig> m_Configs;
    [SerializeField] private List<WeaponGearInfo> m_InfoList;

    public List<WeaponGearInfo> InfoList => m_InfoList;

    public void LoadData(GearsData data, List<WeaponGearConfig> configList )
    {
        m_Data = data;
        m_Configs = configList;

        LoadData();
    }
    private void LoadData(){
        m_InfoList = new List<WeaponGearInfo>();
        foreach(var config in m_Configs){
            GearData data = GetWeaponDataByID(config.id);
            if (data == null)
            {
                data = new GearData();
                data.Init(config.id);
                m_Data.GearDataList.Add(data);
            }
            // else{
            //     Debug.Log("Load data Weapon");  
            //     data.Init(config.id);
            // }
            var info = new WeaponGearInfo();
            info.Init(data,config);
            m_InfoList.Add(info);
        }
    }
    public void UpgradeGear(WeaponGearInfo gear){
        if(gear.UpgradeAble()){
            gear.Upgrade();
        }

        DoUpdateStat();
        SaveData();
    }
    public void EquipGear(WeaponGearInfo gear){
        m_Data.SetEquip(gear.Id);
        DoUpdateStat();
        SaveData();
    }
    public List<ItemInfoEnhance> EnhanceAllGear(){
        List<ItemInfoEnhance> enhanceList = new();
        foreach(var gear in m_InfoList){
            if (!gear.UpgradeAble()) continue;
            ItemInfoEnhance itemEnhance = new()
            {
                itemIcon = gear.GetIcon(),
                rarity = gear.Rarity,
                oldLevel = gear.Level
            };
            gear.UpgradeMaxLevel();
            itemEnhance.newLevel = gear.Level;
            enhanceList.Add(itemEnhance);
        }
        DoUpdateStat();
        SaveData();
        return enhanceList;
    }
    public void AddPiece(int id,int amount){
        GetWeaponDataByID(id).p+=amount;
        DoUpdateStat();
        SaveData();
    }
    #region GET
    private GearData GetWeaponDataByID(int id)
    {
        return m_Data.GetDataByID(id);
    }
    public WeaponGearInfo GetEquippedWeaponInfo()
    {
        return GetWeaponInfoByID(m_Data.eq);
    }
    public WeaponGearInfo GetWeaponInfoByID(int id)
    {
        return InfoList.FirstOrDefault(x => x.Id == id);
    }
    public List<GearConfig> GetWeaponConfigByRarity(Rarity rarity)
    {
        return m_Configs.Where(t => t.rarity == rarity).Cast<GearConfig>().ToList();
    }
    public bool CheckGearEquippedAble(WeaponGearInfo gear){
        if(gear.IsLock()) return false;
        if (!m_Data.HasEquipped()) return true;
        return GetEquippedWeaponInfo() != gear;
    }
    public bool HasEquipped()
    {
        return m_Data.eq != -1;
    }
    public bool IsUnLocked()
    {
        return m_Data.IsUnlocked();
    }
    #endregion
    #region STAT
    // Attack % Owned Effect
    public BigNumber GetAttackOwnedEffect()
    {
        BigNumber ownedEffect = 0;
        foreach (WeaponGearInfo weapon in m_InfoList.Where(armor => armor.IsUnlocked()))
        {
            ownedEffect += weapon.GetOwnedEffect();
        }
        return ownedEffect;
    }
    // Attack % Equipped Effect
    public BigNumber GetAttackEquippedEffect()
    {
        WeaponGearInfo equip = GetEquippedWeaponInfo();
        return equip?.GetEquippedEffect() ?? 0;;
    }
    #endregion

    private static void DoUpdateStat()
    {
        PropertiesManager.Ins.UpdateAttack();
    }
    private void SaveData()
    {
        GearManager.Ins.SaveData();
    }
}
