

public class RockieQuest_DailyLogin : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.DAILY_LOGIN) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.DAILY_LOGIN;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_PassLevel : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.PASS_LEVEL) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.PASS_LEVEL;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeATK : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_ATK) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_ATK;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeHP : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_HP) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_HP;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeHPRegen : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_HP_REGEN) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_HP_REGEN;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeASPD : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_ASPD) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_ASPD;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_SummonEquipment : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.SUMMON_EQUIPMENT) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.SUMMON_EQUIPMENT;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_SummonSkill : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.SUMMON_SKILL) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.SUMMON_SKILL;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_SummonStickMan : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.SUMMON_STICKMAN) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.SUMMON_STICKMAN;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_WatchAds : RockieQuest
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
public class RockieQuest_OpenSupplyBox : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.OPEN_SUPPLY_BOX) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.OPEN_SUPPLY_BOX;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UsePickaxe : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.USE_PICKAXE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.USE_PICKAXE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UseDrill : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.USE_DRILL) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.USE_DRILL;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UseBomb : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.USE_BOMB) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.USE_BOMB;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_ResearchComplete : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.RESEARCH_COMPLETE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.RESEARCH_COMPLETE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_MineDeep : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.MINE_DEEP) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.MINE_DEEP;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_ObtainIdleReward : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.OBTAIN_IDLE_REWARD) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.OBTAIN_IDLE_REWARD;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UseBooster : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.USE_BOOSTER) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.USE_BOOSTER;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_ClearBossRush : RockieQuest
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
public class RockieQuest_ClearGoldRush : RockieQuest
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
public class RockieQuest_ClearStoneRaid : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_DUNGEON_STONE_RAID) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_DUNGEON_STONE_RAID;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_ClearMonsterKing : RockieQuest
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
public class RockieQuest_ObtainHexCore: RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.OBTAIN_HEXCORE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.OBTAIN_HEXCORE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeHexCore : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_HEXCORE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_HEXCORE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_LevelUpHero : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.LEVELUP_HERO) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.LEVELUP_HERO;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeCritChance : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_CRIT_CHANCE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_CRIT_CHANCE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_UpgradeCritDamge : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UPGRADE_CRIT_DMG) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UPGRADE_CRIT_DMG;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_EnhanceRing : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.ENHANCE_RING) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.ENHANCE_RING;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_DismantleRing : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.DISMANTLE_RING) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.DISMANTLE_RING;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class RockieQuest_ClearRingRaid : RockieQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.CLEAR_DUNGEON_RING_RAID) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.CLEAR_DUNGEON_RING_RAID;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}




