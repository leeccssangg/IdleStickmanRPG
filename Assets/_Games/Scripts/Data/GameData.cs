using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData:SingletonFree<GameData> {
    public StickmanDataConfigList StickmanDataConfigList = new StickmanDataConfigList();
    public SkillDataConfigList SkillDataConfigList = new SkillDataConfigList();
    public StatConfigDataList statDataConfigList = new StatConfigDataList();
    protected override void Awake() {
        base.Awake();
        Load();
    }
    [Button]
    public void Load() {
        //StickmanDataConfigList.Load();
        //SkillDataConfigList.Load();
       // statDataConfigList.Load();
    }
    public StickmanDataConfig GetStickmanDataConfig(int id) {
        return StickmanDataConfigList.GetStickmanDataConfig(id);
    }
    public List<StickmanDataConfig> GetStickmanDataConfigList() {
        return StickmanDataConfigList.stickmanDataConfigList;
    }
    public SkillDataConfig GetSkillDataConfig(int skillId) {
        return SkillDataConfigList.GetSkillDataConfig(skillId);
    }
    public List<SkillDataConfig> GetSkillDataConfigList() {
        return SkillDataConfigList.GetSkillDaDataCongfigList();
    }
    public StatsConfigData GetStatDataConfig(MainStatType statType) {
        return statDataConfigList.GetStatConfigData(statType);
    }
    public List<StatsConfigData> GetStatDataConfigList() {
        return statDataConfigList.statDatas;
    }
    public int GetLeveupRequiredPiece(int nextLevel) {
        int piece = 0;
        if(nextLevel <= 4) {
            piece = nextLevel;
        } else if(nextLevel <= 6) {
            piece = nextLevel + 1;
        }else if(nextLevel == 7) {
            piece = nextLevel + 2;
        } else if(nextLevel == 8) {
            piece = nextLevel + 3;
        } else if(nextLevel == 9) {
            piece = nextLevel + 4;
        } else{
            piece = 15;
        }
        return piece;
    }
}
