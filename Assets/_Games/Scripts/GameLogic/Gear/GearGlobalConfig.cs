using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GearGlobalConfig",menuName = "GlobalConfigs/GearGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class GearGlobalConfig : GlobalConfig<GearGlobalConfig> {
    
    public List<WeaponGearConfig> m_WeaponConfigs = new();
    public List<ArmorGearConfig> ArmorConfigs = new();
    public List<HelmetGearConfig> HelmetConfigs = new();
    public List<GlovesGearConfig> GlovesConfigs = new();
    
    public List<GearsConfig> allGearConfig = new();

    public List<WeaponGearConfig> WeaponConfigs => m_WeaponConfigs;

    public void LoadWeaponConfig(TextAsset testAsset)
    {
        WeaponConfigs.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(testAsset.text);
        for(int i = 0;i < list.Count;i++) {
            WeaponGearConfig config = new()
            {
                id = int.Parse(list[i]["ID"]),
                gearName = list[i]["Name"],
                // gearType = (GearType)System.Enum.Parse(typeof(GearType),list[i]["EquipmentType"]),
                gearType = GearType.WEAPON,
                rarity = (Rarity)System.Enum.Parse(typeof(Rarity),list[i]["Rarity"].ToUpper()),
                ownerStatType = (StatType)System.Enum.Parse(typeof(StatType),list[i]["OwnedType"].ToUpper()),
                equippedStatType = (StatType)System.Enum.Parse(typeof(StatType),list[i]["EquippedType"].ToUpper()),
                equippedBaseValue = new BigNumber(list[i]["Equipped_Base"])
            };
            WeaponConfigs.Add(config);
        }
    }
    public void LoadArmorConfig(TextAsset testAsset)
    {
        ArmorConfigs.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(testAsset.text);
        for(int i = 0;i < list.Count;i++) {
            ArmorGearConfig gear = new()
            {
                id = int.Parse(list[i]["ID"]),
                gearName = list[i]["Name"],
                gearType = (GearType.ARMOR),
                // gearType = (GearType)System.Enum.Parse(typeof(GearType),list[i]["EquipmentType"]),
                rarity = (Rarity)System.Enum.Parse(typeof(Rarity),list[i]["Rarity"].ToUpper()),
                ownerStatType = (StatType)System.Enum.Parse(typeof(StatType),list[i]["OwnedType"].ToUpper()),
                equippedStatType = (StatType)System.Enum.Parse(typeof(StatType),list[i]["EquippedType"].ToUpper()),
                equippedBaseValue = new BigNumber(list[i]["Equipped_Base"])
            };
            ArmorConfigs.Add(gear);
        }
    }
    public void LoadHelmetConfig(TextAsset testAsset)
    {
        HelmetConfigs.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(testAsset.text);
        for(int i = 0;i < list.Count;i++) {
            HelmetGearConfig gear = new()
            {
                equippedStatlist = new List<Stat>(),
                optionStatList = new List<Stat>()
            };
            string[] equippedStatString = list[i]["EquippedStat"].Split(',');
            for(int j = 0;j < equippedStatString.Length;j++) {
                string[] statString = equippedStatString[j].Split(':');
                Stat stat = new()
                {
                    StatType = (StatType)System.Enum.Parse(typeof(StatType),statString[0].ToUpper()),
                    Value = float.Parse(statString[1])
                };
                gear.equippedStatlist.Add(stat);
            }
            string[] optionStatString = list[i]["OptionStat"].Split(',');
            for(int j = 0;j < optionStatString.Length;j++) {
                string[] statString = optionStatString[j].Split(':');
                Stat stat = new()
                {
                    StatType = (StatType)System.Enum.Parse(typeof(StatType),statString[0].ToUpper()),
                    Value = float.Parse(statString[1])
                };
                gear.optionStatList.Add(stat);
            }
            HelmetConfigs.Add(gear);
        }
    }
    public void Load(TextAsset testAsset) {
        allGearConfig.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(testAsset.text);
        for(int i = 0;i < list.Count;i++) {
            GearType type = (GearType)System.Enum.Parse(typeof(GearType),list[i]["EquipmentType"]);
            GearsConfig gcf = GetGearsConfigByType(type);
            gcf.Load(list[i]);
        }
    }
    public GearsConfig GetGearsConfigByType(GearType type) {
        GearsConfig config =  allGearConfig.FirstOrDefault(x => x.gearType == type);
        if(config == null) {
            config = new GearsConfig();
            config.gearType = type;
            allGearConfig.Add(config);
        }
        return config;
    }
    public GearConfig GetGearConfigByTypeAndID(GearType type,int id)
    {
        GearsConfig config = GetGearsConfigByType(type);
        GearConfig gearConfig = config.gearConfigList.FirstOrDefault(x => x.id == id);
        return gearConfig;
    }
}


[System.Serializable]
public class GearsConfig {
    public GearType gearType;
    public List<GearConfig> gearConfigList = new();

    public void Load(Dictionary<string,string> dic) {
        GearConfig gear = new()
        {
            id = int.Parse(dic["ID"]),
            gearName = dic["Name"],
            gearType = (GearType)System.Enum.Parse(typeof(GearType),dic["EquipmentType"]),
            rarity = (Rarity)System.Enum.Parse(typeof(Rarity),dic["Rarity"].ToUpper()),
            //ownerStatType = (StatType)System.Enum.Parse(typeof(StatType),dic["OwnedType"].ToUpper())
        };

        string stat1String = "Stat_1";
        if (dic.ContainsKey(stat1String))
        {
            Stat stat1 = new Stat()
            {
                StatType = (StatType)System.Enum.Parse(typeof(StatType),dic[stat1String].ToUpper()),
                Value = float.Parse(dic["Value_1"])
            };
            //gear.equippedStatList.Add(stat1);
        }
        
        gearConfigList.Add(gear);
    }
}
[System.Serializable]
public class GearConfig {
    public int id;
    public string gearName;
    public GearType gearType;
    public Rarity rarity;
}
[System.Serializable]
public class WeaponGearConfig : GearConfig
{
    public StatType ownerStatType;
    public StatType equippedStatType;
    public BigNumber equippedBaseValue;
}
[System.Serializable]
public class ArmorGearConfig : GearConfig
{
    public StatType ownerStatType;
    public StatType equippedStatType;
    public BigNumber equippedBaseValue;
}
[System.Serializable]
public class HelmetGearConfig
{
    public List<Stat> equippedStatlist;
    public List<Stat> optionStatList;
}
[System.Serializable]
public class GlovesGearConfig
{
    public List<Stat> equippedStatlist;
    public List<Stat> optionStatList;
}