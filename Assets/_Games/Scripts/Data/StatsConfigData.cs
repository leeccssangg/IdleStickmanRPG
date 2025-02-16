using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class StatsConfigData {
    public MainStatType statType;
    public BigNumber baseValue;
    public float increaseValue;
    public int maxLevel;
    public void Load(Dictionary<string,string> dic) {
        statType = (MainStatType)(int.Parse(dic["ID"]) - 1);
        maxLevel = int.Parse(dic["MaxLevel"]);
        baseValue = float.Parse(dic["BaseVale"]);
        increaseValue = float.Parse(dic["Increase"]);
    }
    public BigNumber GetValue(int  level) {
        BigNumber value = baseValue + increaseValue * (level -1 );
        return value;
    }
}
[System.Serializable]
public class StatConfigDataList {
    public TextAsset textAset;
    public List<StatsConfigData> statDatas = new List<StatsConfigData>();
    public void Load() {
        statDatas.Clear();
        List<Dictionary<string,string>> list = CSVReader.ReadStringData(textAset.text);
        for(int i = 0;i < list.Count;i++) {
            StatsConfigData st = new StatsConfigData();
            st.Load(list[i]);
            statDatas.Add(st);  
        }
    }

    public StatsConfigData GetStatConfigData(MainStatType statType) {
        return statDatas.FirstOrDefault(x => x.statType == statType);
    }
}

