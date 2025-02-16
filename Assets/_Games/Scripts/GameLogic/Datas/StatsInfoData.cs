using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MainStatData {
    public MainStatType statType;
    public int level = 1;

    private StatsConfigData statsConfigData;

    public void Init(MainStatType stat) {
        statType = stat;
        level = 1;
    }
    public void LevelUp() {
        level++;
    }
    public StatsConfigData GetStatConfigData() {
        if(statsConfigData == null) {
            statsConfigData = GameData.Ins.GetStatDataConfig(statType);
        }
        return statsConfigData;
    }
    public BigNumber UpgradePrice => 100;
    public bool IsUnlocked() {
        return true;
    }
    public int GetLevel() {
        return level;
    }
    public BigNumber GetStatValue() {
        return 1 + (level - 1) * 10;
    }
    public BigNumber GetUpgradePrice() {
        return 100 + (level - 1) * 50;
    }
}
