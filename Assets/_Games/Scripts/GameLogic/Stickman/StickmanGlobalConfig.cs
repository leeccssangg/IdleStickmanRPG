using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StickmanGlobalConfig",menuName = "GlobalConfigs/StickmanGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class StickmanGlobalConfig : GlobalConfig<StickmanGlobalConfig> {
    public List<StickmanConfig> configList = new();

    public void Load(TextAsset textAsset) {
        configList.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadDataFromString(textAsset.text);
        for(int i = 0;i < list.Count;i++) {
            Dictionary<string,string> dic = list[i];
            StickmanConfig config = new();
            config.Load(dic);
            configList.Add(config);
        }
    }
}
[System.Serializable]
public class StickmanConfig {
    public int id;
    public string name;
    public Rarity rarity;
    public Race race;
    public Class classType;
    public Platform platform;
    public BigNumber attack;
    public float increaseAttack = 5;
    public float attackSpeed;
    public string prefabName;

    public void Load(Dictionary<string,string> dic) {
        id = int.Parse(dic["ID"]);
        name = dic["Name"];

        string classString = dic["Class"].ToUpper();
        classType = (Class)Enum.Parse(typeof(Class),classString);

        string raceString = dic["Race"].ToUpper();
        race = (Race)Enum.Parse(typeof(Race),raceString);

        string rarityString = dic["Rarity"].ToUpper();
        rarity = (Rarity)Enum.Parse(typeof(Rarity),rarityString);

        attack = float.Parse(dic["Attack"]);

        attackSpeed = float.Parse(dic["AttackSpeed"]);

        prefabName = $"{dic["Class"]}_{dic["Race"]}";

        platform = classType == Class.PRIEST ? Platform.FLY : classType == Class.PRIEST ? Platform.FLY : Platform.GROUND;
    }
}
