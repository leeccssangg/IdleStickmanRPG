using System.Collections.Generic;
using Sirenix.OdinInspector;

public class ProfileManager : ProfileManagerBase<ProfileManager>
{
    public DailyGiftManager m_DailyGiftManager = new();
    public override void Load()
    {
        base.Load();
        DungeonManager.Ins.LoadData(PlayerData.DungeonsD);
        DailyQuestManager.Ins.LoadData(PlayerData.DailyQuestD);
        TalentTreeManager.Ins.LoadData(PlayerData.TalentTreeD);
        m_DailyGiftManager.LoadData(PlayerData.DailyGiftD);
        SpinWheelManager.Ins.LoadData(PlayerData.SpinWheelD);
        MineResearchManager.Ins.LoadData(PlayerData.MineResearchD);
        RockieQuestManager.Ins.LoadData(PlayerData.RockieQuestD);
        ArtifactsManager.Ins.LoadData(PlayerData.ArtifactsD);
        GearManager.Ins.LoadData(PlayerData.GearData);
        StatManager.Ins.LoadData(PlayerData.StatData);
        SkillManager.Ins.LoadData(PlayerData.SkillData);
        StickManManager.Ins.LoadData(PlayerData.StickmanData);
        GatchaManager.Ins.LoadData(PlayerData.GatchaD);
        MonopolyManager.Ins.LoadData(PlayerData.MonopolyD);
        Save();
    }
    public void SavePlayerStatsTalentTreeData(TalentTreeSaveData data)
    {
        PlayerData.SaveTalentTreeData(data);
        Save();
    }
    public void SaveDungeonsData(DungeonsData data)
    {
        PlayerData.SaveDungeonsData(data);
        Save();
    }
    public void SaveDailyQuestData(DailyQuestData data)
    {
        PlayerData.SaveDailyQuestData(data);
        Save();
    }
    public void SaveDailyGiftData(DailyGiftData data)
    {
        PlayerData.SaveDailyGiftData(data);
        Save();
    }
    public void SaveSpinWheelData(SpinWheelData data)
    {
        PlayerData.SaveSpinWheelData(data);
        Save();
    }
    public void SaveMineResearchData(MineResearchSaveData data)
    {
        PlayerData.SaveMineResearchData(data);
        Save();
    }
    public void SaveRockieQuestData(RockieQuestData data)
    {
        PlayerData.SaveRockieQuestData(data);
        Save();
    }
    public void SaveArtifactsData(ArtifactsData data)
    {
        PlayerData.SaveArtifactsData(data);
        Save();
    }
    public void SaveStatData(StatsData data) {
        PlayerData.SaveStatsData(data);
        Save();
    }
    public void SaveLevelData(InGameLevel data) {
        PlayerData.SaveLevelData(data);
        Save();
    }
    public void SaveGearDatas(GearsDatas data) {
        PlayerData.SaveGearData(data);
        Save();
    }
    public void SaveSkillData(SkillsData data) {
        PlayerData.SaveSkillData(data);
        Save();
    }
    public void SaveStickmanData(StickmansData data) {
        PlayerData.SaveStickmanData(data);
        Save();
    }
    public void SaveGatchaData(GatchaData data)
    {
        PlayerData.SaveGatchaData(data);
        Save();
    }
    [Button]
    public override void Save()
    {
        base.Save();
    }
    public void ClaimDailyGift()
    {
        m_DailyGiftManager.Claim();
    }
    public bool IsGoodToClaim()
    {
        return m_DailyGiftManager.IsGoodToClaim();
    }
    public List<DailyGift> GetListGiftConfigs()
    {
        return m_DailyGiftManager.GetDailyGiftConfig().gifts;
    }
    public int GetMyCurrentDayID()
    {
        return m_DailyGiftManager.GetCurrentDayId();
    }

}
