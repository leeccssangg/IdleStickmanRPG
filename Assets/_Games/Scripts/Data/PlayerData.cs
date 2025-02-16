using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    [LabelText("Player Resource Unique List")]
    public PlayerResource pr;

    public DungeonsData dd;
    public DailyQuestData dqd;
    public TalentTreeSaveData td;
    public DailyGiftData dgd;
    public SpinWheelData swd;
    public MineResearchSaveData mrd;
    public RockieQuestData rqd;
    public ArtifactsData atd;
    public GearsDatas gd;
    public SkillsData sk;
    public StickmansData smd;
    public GatchaData gcd;
    public MonopolyJsonData mld;
    /// <summary>
    /// Stat Data
    /// </summary>
    public StatsData std;
    /// <summary>
    /// Level Data
    /// </summary>
    public InGameLevel lvd;

    public PlayerResource PlayerResourceD { get => pr; set => pr = value; }
    public DungeonsData DungeonsD { get => dd; set => dd = value; }
    public DailyQuestData DailyQuestD { get => dqd; set => dqd = value; }
    public DailyGiftData DailyGiftD { get => dgd; set => dgd = value; }
    public TalentTreeSaveData TalentTreeD { get => td; set => td = value; }
    public SpinWheelData SpinWheelD { get => swd; set => swd = value; }
    public MineResearchSaveData MineResearchD { get => mrd; set => mrd = value; }
    public RockieQuestData RockieQuestD { get => rqd; set => rqd = value; }
    public ArtifactsData ArtifactsD { get => atd; set => atd = value; }
    public StatsData StatData { get => std; set => std = value; }
    public InGameLevel LevelData { get => lvd; set => lvd = value; }
    public GearsDatas GearData { get => gd; set => gd = value; }
    public SkillsData SkillData { get => sk; set => sk = value; }
    public StickmansData StickmanData { get => smd; set => smd = value; }
    public GatchaData GatchaD { get => gcd; set => gcd = value; }
    public MonopolyJsonData MonopolyD { get => mld; set => mld = value; }

    /// <summary>
    /// Auto Skill
    /// </summary>
    public int ak = 0;
    /// <summary>
    /// Skill Datas
    /// </summary>
    public List<CollectorData> skd = new List<CollectorData>();
    /// <summary>
    /// StickmanData
    /// </summary>
    public List<CollectorData> stmd = new List<CollectorData>();
    /// <summary>
    /// Skill Line up
    /// </summary>
    public List<LineUp> skl = new List<LineUp>();
    /// <summary>
    /// Stickman Line up
    /// </summary>
    public List<LineUp> sml = new List<LineUp>();
    /// <summary>
    /// Stat Data
    /// </summary>
    private List<StatData> sd = new List<StatData>();

    #region Save & Load data
    public void CreateNewData() {
        PlayerResourceD = new();
        PlayerResourceD.CreateNewData();
        DungeonsD = new(new List<DungeonData>
        {
            new DungeonData(DungeonType.BOSS_RUSH),
            new DungeonData(DungeonType.GOLD_RUSH),
            new DungeonData(DungeonType.MONSTER_NEST),
            new DungeonData(DungeonType.CHALLENGE_KING),
        });
        DailyGiftD = new();
        DailyQuestD = new();
        TalentTreeD = new();
        SpinWheelD = new();
        MineResearchD = new();
        RockieQuestD = new();
        ArtifactsD = new();
        StatData = new();
        LevelData = new();
        GearData = new();
        SkillData = new();
        GatchaD = new();
        MonopolyD = new();
    }
    public void LoadData() {
        PlayerResourceD.LoadData();
    }
    public void SaveDungeonsData(DungeonsData data) {
        DungeonsD = data;
    }
    public void SaveDailyQuestData(DailyQuestData data) {
        DailyQuestD = data;
    }
    public void SaveTalentTreeData(TalentTreeSaveData data) {
        TalentTreeD = data;
    }
    public void SaveDailyGiftData(DailyGiftData data) {
        DailyGiftD = data;
    }
    public void SaveSpinWheelData(SpinWheelData data) {
        SpinWheelD = data;
    }
    public void SaveMineResearchData(MineResearchSaveData data) {
        MineResearchD = data;
    }
    public void SaveRockieQuestData(RockieQuestData data) {
        RockieQuestD = data;
    }
    public void SaveArtifactsData(ArtifactsData data) {
        ArtifactsD = data;
    }
    public void SaveStatsData(StatsData data) {
        StatData = data;
    }
    public void SaveLevelData(InGameLevel data) {
        LevelData = data;
    }
    public void SaveGearData(GearsDatas data) {
        GearData = data;
    }
    public void SaveSkillData(SkillsData data) {
        SkillData = data;
    }
    public void SaveStickmanData(StickmansData data) {
        StickmanData = data;
    }
    public void SaveGatchaData(GatchaData data)
    {
        GatchaD = data;
    }
    public void SaveMonopolyData(MonopolyJsonData data)
    {
        MonopolyD = data;
    }
    #endregion
    #region Game Resource Function
    public BigNumber GetResoureValue(ResourceData.ResourceType resourceType) {
        return PlayerResourceD.GetResourceValue(resourceType);
    }
    public void AddGameResource(ResourceRewardPackage rewardPackage) {
        PlayerResourceD.AddGameResource(rewardPackage.rewardType,rewardPackage.amount);
    }
    public void AddGameResource(ResourceData.ResourceType resourceType,BigNumber value) {
        PlayerResourceD.AddGameResource(resourceType,value);
    }
    public void AddGameResource(ResourceData resource) {
        PlayerResourceD.AddGameResource(resource);
    }
    public void AddGameResource(ResourceWallet resourceList) {
        PlayerResourceD.AddGameResource(resourceList);
    }
    public void ConsumeGameResource(ResourceData.ResourceType resourceType,BigNumber value) {
        PlayerResourceD.ConsumeGameResource(resourceType,value);
    }
    public void ConsumeGameResource(ResourceData resource) {
        PlayerResourceD.ConsumeGameResource(resource);
    }
    public void ConsumeGameResource(ResourceWallet resourceList) {
        PlayerResourceD.ConsumeGameResource(resourceList);
    }
    public bool IsEnoughGameResource(ResourceData.ResourceType resourceType,BigNumber value) {
        return PlayerResourceD.IsEnoughGameResource(resourceType,value);
    }
    public bool IsEnoughGameResource(ResourceData resource) {
        return PlayerResourceD.IsEnoughGameResource(resource);
    }
    public bool IsEnoughGameResource(ResourceWallet resourceList) {
        return PlayerResourceD.IsEnoughGameResource(resourceList);
    }
    #endregion
    #region Key Function
    public int GetKeyValue(DungeonType dungeonType,KeyType keyType) {
        return PlayerResourceD.GetKeyValue(dungeonType,keyType);
    }
    public void AddKey(DungeonType dungeonType,KeyType keyType,int value) {
        PlayerResourceD.AddKey(dungeonType,keyType,value);
    }
    public void AddKey(DungeonKeyData dungeonKeyData,KeyData keyData) {
        PlayerResourceD.AddKey(dungeonKeyData,keyData);
    }
    public void AddKey(KeyWallet keyList) {
        PlayerResourceD.AddKey(keyList);
    }
    public void ConsumeKey(DungeonType dungeonType,KeyType keyType,int value) {
        PlayerResourceD.ConsumeKey(dungeonType,keyType,value);
    }
    public void ConsumeKey(DungeonKeyData dungeonKeyData,KeyData keyData) {
        PlayerResourceD.ConsumeKey(dungeonKeyData,keyData);
    }
    public void ConsumeKey(KeyWallet keyList) {
        PlayerResourceD.ConsumeKey(keyList);
    }
    public bool IsEnoughKey(DungeonType dungeonType,KeyType keyType,int value) {
        return PlayerResourceD.IsEnoughKey(dungeonType,keyType,value);
    }
    public bool IsEnoughKey(DungeonKeyData dungeonKeyData,KeyData keyData) {
        return PlayerResourceD.IsEnoughKey(dungeonKeyData,keyData);
    }
    public bool IsEnoughKey(KeyWallet keyList) {
        return PlayerResourceD.IsEnoughKey(keyList);
    }
    #endregion

    // #region Skill
    // public CollectorData GetSkillData(int id) {
    //     for(int i = 0;i < skd.Count;i++) {
    //         CollectorData skillData = skd[i];
    //         if(skillData.i == id) {
    //             return skillData;
    //         }
    //     }
    //     return null;
    // }
    // public void AddSkillPiece(int skillID,int pieceAmount) {
    //     CollectorData skillData = GetSkillData(skillID);
    //     if(skillData == null) {
    //         skillData = new CollectorData();
    //         skillData.Init(skillID);
    //         skd.Add(skillData);
    //     }
    //     skillData.AddPiece(pieceAmount);
    // }
    // public void LevelUpSkill(int id) {
    //     CollectorData skillData = GetSkillData(id);
    //     int nextLevel = skillData.GetLevel() + 1;
    //     int requiredPiece = GameData.Ins.GetSkillDataConfig(id).GetLevelRequirePiece(nextLevel);
    //     skillData.ConsumePiece(requiredPiece);
    //     skillData.LevelUp();
    //     //SaveDataToLocal(false);
    // }
    // public void EquipSkill(int slotID,int skillID) {
    //     CollectorData skillData = GetSkillData(skillID);
    //     skillData.SetSlotID(slotID);
    //     LineUp skillLineUp = GetSkillLineUp(slotID);
    //     skillLineUp.id = skillID;
    //     //SaveDataToLocal(false);
    // }
    // public void RemoveSkill(int skillID) {
    //     CollectorData skillData = GetSkillData(skillID);
    //     FreeSkillLineUpSlot(skillData.GetSlotID());
    //     skillData.SetSlotID(0);
    //     //SaveDataToLocal(false);
    // }
    // public void SetSkillLineup(int slotID,int skillID,bool isChange = false) {
    //     LineUp skillLineUp = GetSkillLineUp(slotID);
    //     skillLineUp.id = skillID;
    //     CollectorData skillData = GetSkillData(skillID);
    //     if(skillData != null) {
    //         skillData.SetSlotID(slotID);
    //     }
    // }
    // public void FreeSkillLineUpSlot(int slotID) {
    //     LineUp skillLineUp = GetSkillLineUp(slotID);
    //     skillLineUp.SetFree();
    // }
    // public List<LineUp> GetSkillLineUp() {
    //     return skl;
    // }
    // public LineUp GetSkillLineUp(int slotID) {
    //     return skl[slotID - 1];
    // }
    // public LineUp GetSkillFreeSlot() {
    //     for(int i = 0;i < skl.Count;i++) {
    //         LineUp slot = skl[i];
    //         if(slot.IsFree()) {
    //             return slot;
    //         }
    //     }
    //     return null;
    // }
    // public void UnLockSlotSkill(int slotID) {
    //     GetSkillLineUp(slotID).Unlock();
    // }
    // public void SetAutoSkill(bool isAuto) {
    //     ak = isAuto ? 1 : 0;
    // }
    // #endregion
    // #region STICKMAN
    // public CollectorData GetStickmanData(int id) {
    //     for(int i = 0;i < stmd.Count;i++) {
    //         CollectorData smData = stmd[i];
    //         if(smData.i == id) {
    //             return smData;
    //         }
    //     }
    //     return null;
    // }
    // public void AddStickmanPiece(int smID,int pieceAmount) {
    //     CollectorData smData = GetStickmanData(smID);
    //     if(smData == null) {
    //         smData = new CollectorData();
    //         smData.Init(smID);
    //         stmd.Add(smData);
    //     }
    //     smData.AddPiece(pieceAmount);
    // }
    // public void LevelUpStickman(int id) {
    //     CollectorData smData = GetStickmanData(id);
    //     int nextLevel = smData.GetLevel() + 1;
    //     int requiredPiece = GameData.Ins.GetLeveupRequiredPiece(nextLevel);
    //     smData.ConsumePiece(requiredPiece);
    //     smData.LevelUp();
    //     //SaveDataToLocal(false);
    // }
    // public void EquipStickman(int slotID,int smID) {
    //     CollectorData stmData = GetStickmanData(smID);
    //     stmData.SetSlotID(slotID);
    //     LineUp smLineUp = GetStickmanLineUp(slotID);
    //     smLineUp.id = smID;
    //     //SaveDataToLocal(false);
    // }
    // public void RemoveStickman(int smID) {
    //     CollectorData smData = GetStickmanData(smID);
    //     FreeStickmanLineUpSlot(smData.GetSlotID());
    //     smData.SetSlotID(0);
    //     //SaveDataToLocal(false);
    // }
    // public void FreeStickmanLineUpSlot(int slotID) {
    //     LineUp smLineUp = GetStickmanLineUp(slotID);
    //     smLineUp.SetFree();
    // }
    // public List<LineUp> GetStickmanLineUp() {
    //     return sml;
    // }
    // public LineUp GetStickmanLineUp(int slotID) {
    //     return sml[slotID - 1];
    // }
    // public LineUp GetStickmanFreeSlot() {
    //     for(int i = 0;i < sml.Count;i++) {
    //         LineUp slot = sml[i];
    //         if(slot.IsFree()) {
    //             return slot;
    //         }
    //     }
    //     return null;
    // }
    // #endregion

    #region Data String
    public string ToJsonString() {
        return LitJson.JsonMapper.ToJson(this,true,false);
    }
    #endregion
}
