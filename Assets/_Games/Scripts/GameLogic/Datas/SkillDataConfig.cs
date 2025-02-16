using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class SkillDataConfig {
    public int id;
    public string name = "Hehehe" +
        "" +
        "";
    public Rarity rarity;
    public float coolDownTime;
    public BigNumber damage;
    public float cooldownTime;
    public float ownedEffect;
    public float value;
    public void LoadData(Dictionary<string,string> dic) {
        id = int.Parse(dic["ID"]);

        string rst = dic["Rarity"].ToUpper();
        rarity = (Rarity)Enum.Parse(typeof(Rarity),rst);
        coolDownTime = int.Parse(dic["Cooldown"]);
        value = float.Parse(dic["Value"]);
    }

    public int GetLevelRequirePiece(int nextLevel) {
        return 2 +  2 * (nextLevel - 2);
    }
    public float GetAttackTime() {
        return 1;
    }
    public float GetCooldowntime() {
        return cooldownTime;
    }
}
[System.Serializable]
public class SkillDataConfigList {
    public TextAsset textAsset;
    public List<SkillDataConfig> m_DataList = new List<SkillDataConfig>();

    public void Load() {
        m_DataList.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(textAsset.text);
        for(int i = 0; i < list.Count; i++) {
            Dictionary<string,string> dic = list[i];
            SkillDataConfig skill = new SkillDataConfig();
            skill.LoadData(dic);
            m_DataList.Add(skill);
        }
    }

    public SkillDataConfig GetSkillDataConfig(int id) => m_DataList.FirstOrDefault(x => x.id == id);
    public List<SkillDataConfig> GetSkillDaDataCongfigList() => m_DataList;
}