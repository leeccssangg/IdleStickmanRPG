using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RockieQuestSubject : Subject<MissionTarget>
{
    [SerializeField] private List<RockieQuest> m_RockieQuests = new();
    #region Load & Save Data
    public void LoadData(string jsonData = "")
    {
        if (!string.IsNullOrEmpty(jsonData))
            LoadOldData(jsonData);
        else
            LoadNewData();
    }
    private void LoadNewData()
    {
        m_RockieQuests = new();
        int questCount = RockieQuestManager.Ins.GetNumRockieQuestConfig();
        Debug.Log(questCount);
        for (int j = 1; j <= questCount; j++)
        {
            RockieQuestConfig rockieQuestConfig = RockieQuestManager.Ins.GetRockieQuestConfig(j);
            QuestConfig questConfig = rockieQuestConfig.questConfig;
            RockieQuest quest = GenerateRockieQuest(questConfig.missionTarget);
            quest.Init(questConfig.id);
            m_RockieQuests.Add(quest);
            AddObserver(quest);
        }
        SaveData();
    }
    private void LoadOldData(string jsonData)
    {
        m_RockieQuests = new();
        List<RockieQuest> rawList = JsonConvert.DeserializeObject<List<RockieQuest>>(jsonData);
        for (int i = 0; i < rawList.Count; i++)
        {
            RockieQuest quest = rawList[i];
            quest.InitQuestConfig();
            string rawString = JsonConvert.SerializeObject(quest);
            RockieQuest newQuest = DeserializeQuest(rawString, quest.GetMissionTarget());
            newQuest.InitQuestConfig();
            m_RockieQuests.Add(newQuest);
            AddObserver(newQuest);
        }
        SaveData();
    }
    public string SaveData()
    {
        return JsonConvert.SerializeObject(m_RockieQuests);
    }
    public RockieQuest GenerateRockieQuest(MissionTarget missionTarget)
    {
        RockieQuest newRockieQuest = null;
        switch (missionTarget)
        {
            case MissionTarget.DAILY_LOGIN:
                {
                    RockieQuest_DailyLogin newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.PASS_LEVEL:
                {
                    RockieQuest_PassLevel newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_ATK:
                {
                    RockieQuest_UpgradeATK newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_HP:
                {
                    RockieQuest_UpgradeHP newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_HP_REGEN:
                {
                    RockieQuest_UpgradeHPRegen newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_ASPD:
                {
                    RockieQuest_UpgradeASPD newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.SUMMON_EQUIPMENT:
                {
                    RockieQuest_SummonEquipment newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.SUMMON_SKILL:
                {
                    RockieQuest_SummonSkill newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.SUMMON_STICKMAN:
                {
                    RockieQuest_SummonStickMan newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.WATCH_ADS:
                {
                    RockieQuest_WatchAds newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.OPEN_SUPPLY_BOX:
                {
                    RockieQuest_OpenSupplyBox newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.USE_PICKAXE:
                {
                    RockieQuest_UsePickaxe newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.USE_DRILL:
                {
                    RockieQuest_UseDrill newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.USE_BOMB:
                {
                    RockieQuest_UseBomb newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.RESEARCH_COMPLETE:
                {
                    RockieQuest_ResearchComplete newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.MINE_DEEP:
                {
                    RockieQuest_MineDeep newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.OBTAIN_IDLE_REWARD:
                {
                    RockieQuest_ObtainIdleReward newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.USE_BOOSTER:
                {
                    RockieQuest_UseBooster newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_BOSS_RUSH:
                {
                    RockieQuest_ClearBossRush newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_GOLD_RUSH:
                {
                    RockieQuest_ClearGoldRush newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_STONE_RAID:
                {
                    RockieQuest_ClearStoneRaid newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_CHALLENGE_KING:
                {
                    RockieQuest_ClearMonsterKing newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.OBTAIN_HEXCORE:
                {
                    RockieQuest_ObtainHexCore newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_HEXCORE:
                {
                    RockieQuest_UpgradeHexCore newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.LEVELUP_HERO:
                {
                    RockieQuest_LevelUpHero newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_CRIT_CHANCE:
                {
                    RockieQuest_UpgradeCritChance newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.UPGRADE_CRIT_DMG:
                {
                    RockieQuest_UpgradeCritDamge newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.ENHANCE_RING:
                {
                    RockieQuest_EnhanceRing newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.DISMANTLE_RING:
                {
                    RockieQuest_DismantleRing newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_RING_RAID:
                {
                    RockieQuest_ClearRingRaid newQuest = new();
                    newRockieQuest = newQuest;
                }
                break;
        }
        return newRockieQuest;
    }
    public RockieQuest DeserializeQuest(string json, MissionTarget missionTarget)
    {
        RockieQuest rockieQuest = null;
        switch (missionTarget)
        {
            case MissionTarget.DAILY_LOGIN:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_DailyLogin>(json);
                }
                break;
            case MissionTarget.PASS_LEVEL:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_PassLevel>(json);
                }
                break;
            case MissionTarget.UPGRADE_ATK:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeATK>(json);
                }
                break;
            case MissionTarget.UPGRADE_HP:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeHP>(json);
                }
                break;
            case MissionTarget.UPGRADE_HP_REGEN:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeHPRegen>(json);
                }
                break;
            case MissionTarget.UPGRADE_ASPD:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeASPD>(json);
                }
                break;
            case MissionTarget.SUMMON_EQUIPMENT:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_SummonEquipment>(json);
                }
                break;
            case MissionTarget.SUMMON_SKILL:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_SummonSkill>(json);
                }
                break;
            case MissionTarget.SUMMON_STICKMAN:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_SummonStickMan>(json);
                }
                break;
            case MissionTarget.WATCH_ADS:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_WatchAds>(json);
                }
                break;
            case MissionTarget.OPEN_SUPPLY_BOX:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_OpenSupplyBox>(json);
                }
                break;
            case MissionTarget.USE_PICKAXE:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UsePickaxe>(json);
                }
                break;
            case MissionTarget.USE_DRILL:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UseDrill>(json);
                }
                break;
            case MissionTarget.USE_BOMB:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UseBomb>(json);
                }
                break;
            case MissionTarget.RESEARCH_COMPLETE:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ResearchComplete>(json);
                }
                break;
            case MissionTarget.MINE_DEEP:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_MineDeep>(json);
                }
                break;
            case MissionTarget.OBTAIN_IDLE_REWARD:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ObtainIdleReward>(json);
                }
                break;
            case MissionTarget.USE_BOOSTER:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UseBooster>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_BOSS_RUSH:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ClearBossRush>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_GOLD_RUSH:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ClearGoldRush>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_STONE_RAID:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ClearStoneRaid>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_CHALLENGE_KING:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ClearMonsterKing>(json);
                }
                break;
            case MissionTarget.OBTAIN_HEXCORE:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ObtainHexCore>(json);
                }
                break;
            case MissionTarget.UPGRADE_HEXCORE:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeHexCore>(json);
                }
                break;
            case MissionTarget.LEVELUP_HERO:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_LevelUpHero>(json);
                }
                break;
            case MissionTarget.UPGRADE_CRIT_CHANCE:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeCritChance>(json);
                }
                break;
            case MissionTarget.UPGRADE_CRIT_DMG:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_UpgradeCritDamge>(json);
                }
                break;
            case MissionTarget.ENHANCE_RING:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_EnhanceRing>(json);
                }
                break;
            case MissionTarget.DISMANTLE_RING:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_DismantleRing>(json);
                }
                break;
            case MissionTarget.CLEAR_DUNGEON_RING_RAID:
                {
                    rockieQuest = JsonConvert.DeserializeObject<RockieQuest_ClearRingRaid>(json);
                }
                break;
        }
        return rockieQuest;
    }
    #endregion
    #region Daily Quest Function
    public override void Notify(MissionTarget id, string info)
    {
        base.Notify(id, info);
        SaveData();
    }
    public void ClaimQuest(RockieQuest dailyQuest)
    {
        QuestConfig questConfig = dailyQuest.GetQuestConfig();
        AddReward(questConfig.reward);
        dailyQuest.OnClaim();
    }
    public void AddReward(ResourceRewardPackage itemPackage)
    {
        ProfileManager.PlayerData.AddGameResource(itemPackage);
    }
    public List<RockieQuest> GetListDailyQuestsByDay(int day)
    {
        List<RockieQuest> list = new();
        for(int i = 0; i < m_RockieQuests.Count; i++)
        {
            if (m_RockieQuests[i].day == day)
                list.Add(m_RockieQuests[i]);
        }
        return new List<RockieQuest>(list);
    }
    #endregion
}
