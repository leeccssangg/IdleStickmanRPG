using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SkillGlobalConfig",menuName = "GlobalConfigs/SkillGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class SkillGlobalConfig :GlobalConfig <SkillGlobalConfig>{
    public List<SkillConfig> configList = new List<SkillConfig>();  

    public void Load(TextAsset textAsset) {
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(textAsset.text);
        configList.Clear();
        for(int i = 0;i < list.Count;i++) {
            Dictionary<string,string> dic = list[i];
            SkillConfig config = new();
            config.id = int.Parse(dic["ID"]);
            config.rarity = (Rarity)System.Enum.Parse(typeof(Rarity),dic["Rarity"].ToUpper());
            config.name = dic["Name"];
            config.cooldownTime = float.Parse(dic["Cooldown"]);
            config.dpsRate = float.Parse(dic["DPSRate"]);
            config.dpsIncrease = float.Parse(dic["DPSIncrease"]);
            config.dmgRate = float.Parse(dic["DmgRate"]);
            config.prefabName = "Skill_" + int.Parse(dic["ID"]);
            configList.Add(config);
        }
       
    }
}
[System.Serializable]
public class SkillConfig {
    public int id;
    public string name;
    public Rarity rarity;
    public float cooldownTime;
    public float dpsRate;
    public float dpsIncrease;
    public float dmgRate;
    public string description;
    public string prefabName;

    public SkillConfig Clone()
    {
        return (SkillConfig)this.MemberwiseClone();
    }
}
