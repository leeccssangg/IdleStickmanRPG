using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsGlobalConfig",menuName = "GlobalConfigs/StatsGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class StatGlobalConfig:GlobalConfig<StatGlobalConfig> {
    public List<MainStatConfig> mainStatsConfigs = new();

    public void Load(TextAsset textAset) {
        mainStatsConfigs.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(textAset.text);
        for(int i = 0;i < list.Count;i++) {
            MainStatConfig st = new();
            st.Load(list[i]);
            mainStatsConfigs.Add(st);
        }
    }
    public MainStatConfig GetStatConfig(int id) {
        for(int i = 0;i < mainStatsConfigs.Count;i++) {
            if(mainStatsConfigs[i].statID == id) {
                return mainStatsConfigs[i];
            }
        }
        return null;
    }
    public MainStatConfig GetStatConfig(MainStatType statType) {
        for(int i = 0;i < mainStatsConfigs.Count;i++) {
            if(mainStatsConfigs[i].statType == statType) {
                return mainStatsConfigs[i];
            }
        }
        return null;
    }
}
[System.Serializable]
public class MainStatConfig {
    public int statID;
    public MainStatType statType;
    public BigNumber baseValue;
    public float increaseValue;
    public int maxLevel;
    public void Load(Dictionary<string,string> dic) {
        statType = (MainStatType)(int.Parse(dic["ID"]));
        maxLevel = int.Parse(dic["MaxLevel"]);
        baseValue = float.Parse(dic["BaseVale"]);
        increaseValue = float.Parse(dic["Increase"]);
    }

    public BigNumber GetValue(int level) {
        BigNumber value = baseValue + increaseValue * (level - 1);
        return value;
    }
}
