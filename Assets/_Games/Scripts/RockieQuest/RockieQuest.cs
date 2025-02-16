using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockieQuest : Quest<MissionTarget>
{
    protected QuestConfig questConfig;
    public int day;
    public void Init(int id)
    {
        this.id = id;
        InitQuestConfig();
        this.mt = (int)questConfig.missionTarget;
        this.qt = (int)questConfig.type;
        this.cl = 0;
        this.tgm = questConfig.targetAmount;
        this.icd = 0;
    }
    public virtual void InitQuestConfig()
    {
        questConfig = RockieQuestManager.Ins.GetRockieQuestConfig(this.id).questConfig;
        this.day = RockieQuestManager.Ins.GetRockieQuestConfig(this.id).day;
    }
    public override void OnNotify(MissionTarget id, string info)
    {
        if (day <= RockieQuestManager.Ins.GetDayNum() && id == questConfig.missionTarget)
        {
            base.OnNotify(id, info);
        }
    }
    public virtual MissionTarget GetMissionTarget()
    {
        return (MissionTarget)this.mt;
    }
    public void SetMissionTarget(MissionTarget missionTarget)
    {
        this.mt = (int)missionTarget;
    }
    public virtual void SetTargetAmount(int amount)
    {
        this.tgm = amount;
    }
    public void SetQuestConfig(QuestConfig questConfig)
    {
        this.questConfig = questConfig;
    }
    public QuestConfig GetQuestConfig()
    {
        return questConfig;
    }
    public override string GetDescription()
    {
        return questConfig.GetDescription();
    }
    public ResourceRewardPackage GetReward()
    {
        return questConfig.reward;
    }
}
