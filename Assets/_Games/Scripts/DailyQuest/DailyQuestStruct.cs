
public class DailyQuest_DefeatEnemy : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.DEFEAT_ENEMY) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.DEFEAT_ENEMY;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_ClearStage : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_STAGE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_STAGE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_ClearDungeonBossRush : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_DUNGEON_BOSS_RUSH) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_DUNGEON_BOSS_RUSH;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_ClearDungeonGoldRush : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_DUNGEON_GOLD_RUSH) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_DUNGEON_GOLD_RUSH;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_ClearDungeonMonsterNest : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_DUNGEON_MONSTER_NEST) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_DUNGEON_MONSTER_NEST;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_ClearDungeonChallengeKing : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_DUNGEON_CHALLENGE_KING) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_DUNGEON_CHALLENGE_KING;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_Summon : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.SUMMON) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.SUMMON;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_WatchAds : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.WATCH_ADS) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.WATCH_ADS;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_CompleteAllDailyQuest : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.COMPLETE_ALL_DAILY_QUEST) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.COMPLETE_ALL_DAILY_QUEST;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}

