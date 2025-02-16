using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
[System.Serializable]
public class StickmanDataConfig 
{
    public int id;
    public string name;
    public Class classType;
    public Race race;
    public Rarity rarity;
    public Platform platform;
    public BigNumber attack;
    public float increaseAttack = 5;
    public float attackSpeed;
    public float increaseATKSpeed = 0.05f;
    public string prefabName;
    public List<int> weapons = new List<int>();


    public BigNumber GetAttack(int level ) {
        return attack + increaseAttack * (level - 1);
    }
    public float GetAtackSpeed() {
        return attackSpeed;
    }
    public float GetAtackSpeed( int level ) {
        return attackSpeed + increaseATKSpeed * (level - 1);
    }
    public string GetStickmanRaceName() {
        switch (race) {
            case Race.HUMAN:
                return "HUMAN";
            case Race.DEMON:
                return "DEMON";
            case Race.ELF:
                return "ELF";
            case Race.ORC:
                return "ORC";
        }
        return "";
    }
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

        if(classType == Class.PRIEST || classType == Class.MAGE) {
            platform = Platform.FLY;
        } else {
            platform = Platform.GROUND;
        }
    }
}
[System.Serializable]
public class StickmanDataConfigList {
    public List<StickmanDataConfig> stickmanDataConfigList = new List<StickmanDataConfig>();
    public TextAsset textAsset;

    public void Load() {
        stickmanDataConfigList.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadDataFromString(textAsset.text);
        for(int i = 0;i < list.Count;i++) {
            Dictionary<string,string> dic = list[i];
            StickmanDataConfig data = new StickmanDataConfig();
            data.Load(dic);
            stickmanDataConfigList.Add(data);
        }
    }

    public StickmanDataConfig GetStickmanDataConfig(int id) {
        return stickmanDataConfigList.FirstOrDefault(x =>x.id == id);
    }
}