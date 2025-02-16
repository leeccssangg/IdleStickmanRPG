using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ArmorGearManager : MonoBehaviour
{
    [SerializeField] private GearsData m_Data;
    [SerializeField] private List<ArmorGearConfig> m_Configs;
    [SerializeField] private List<ArmorGearInfo> m_InfoList;
    public List<ArmorGearInfo> InfoList => m_InfoList;
    private Dictionary<Rarity, List<GearConfig>> ClassifyArmorConfig { get; set; } = new();
    

    private BigNumber m_EquippedEffect;
    private BigNumber m_OwnedEffect;
    public void LoadData(GearsData data, List<ArmorGearConfig> configList )
    {
        m_Data = data;
        m_Configs = configList;

        LoadData();
    }
    private void LoadData()
    {
        m_InfoList = new List<ArmorGearInfo>();
        ClassifyArmorConfig = new Dictionary<Rarity, List<GearConfig>>();
        for (int i = 0; i < m_Configs.Count; i++){
            ArmorGearConfig config = m_Configs[i];
            GearData data = GetArmorDataByID(config.id);
            if (data == null)
            {
                Debug.Log("Create new data Armor");
                data = new GearData();
                data.Init(config.id);
                m_Data.GearDataList.Add(data);
            }
            Rarity rarity = config.rarity;
            if (!ClassifyArmorConfig.ContainsKey(rarity))
            {
                ClassifyArmorConfig.Add(rarity,new List<GearConfig>());
            }
            ClassifyArmorConfig[rarity].Add(config);
            var info = new ArmorGearInfo();
            info.Init(data,config);
            m_InfoList.Add(info);       
        }
    }
    public void EquipGear(ArmorGearInfo gear){
        m_Data.SetEquip(gear.Id);
        SaveData();
    }
    public void UpgradeGear(ArmorGearInfo gear){
        if(gear.UpgradeAble()){
            gear.Upgrade();
        }
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
        SaveData();
        return enhanceList;
    }
    public void AddPiece(int id,int amount){
        GetArmorDataByID(id).p+=amount;
        SaveData();
    }

#region GET
    public ArmorGearInfo GetEquippedArmorInfo()
    {
        return GetArmorInfoByID(m_Data.eq);
    }
    public ArmorGearInfo GetArmorInfoByID(int id)
    {
        return InfoList.FirstOrDefault(x => x.Id == id);
    }
    private GearData GetArmorDataByID(int id)
    {
        return m_Data.GetDataByID(id);
    }
    public ArmorGearInfo GetEquippedInfo()
    {
        return m_InfoList.FirstOrDefault(x => x.Id == m_Data.eq);
    }
    public IEnumerable<GearConfig> GetWeaponConfigByRarity(Rarity rarity)
    {
        return ClassifyArmorConfig[rarity];
    }
    public bool CheckGearEquippedAble(ArmorGearInfo gear){
        if(gear.IsLock()) return false;
        if (!m_Data.HasEquipped()) return true;
        return GetEquippedArmorInfo() != gear;
    }
    public bool IsUnLocked()
    {
        return true;
        return m_Data.IsUnlocked();
    }
    public bool HasEquipped()
    {
        return m_Data.eq != -1;
    }
    #endregion

#region STAT
   //Tong HP Owned Effect
    public BigNumber GetHpOwnedEffect()
    {
        m_OwnedEffect = 0;
        foreach (ArmorGearInfo armor in m_InfoList.Where(armor => armor.IsUnlocked()))
        {
            m_OwnedEffect += armor.GetOwnedEffect();
        }
        return m_OwnedEffect;
    }
    // HP Equipped Effect
    public BigNumber GetHpEquippedEffect()
    {
        ArmorGearInfo equip = GetEquippedInfo();
        m_EquippedEffect = equip?.GetEquippedEffect() ?? 0;
        return m_EquippedEffect;
    }
#endregion
    private void SaveData(){
        GearManager.Ins.SaveData();
    }
}
