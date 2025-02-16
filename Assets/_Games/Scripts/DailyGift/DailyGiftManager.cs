#if UNITY_EDITOR
#endif
using System;
using UnityEngine;
using System.Globalization;

public class DailyGiftManager {
     private DailyGiftConfigs m_DailyGiftConfigs;
     private int m_CurrentDayID;
     private DateTime m_NextClaimDay;
     private DateTime m_NextDay;

    public void LoadData(DailyGiftData data) {
        m_DailyGiftConfigs = DailyGiftConfigs.Instance;
        //m_DailyGiftConfigs = Resources.Load<DailyGiftConfigs>("GlobalConfig/DailyGiftConfigs");
        ImportDataString(data.Id, data.Date);
        UpdateNextDay();
    }
    public void Save() {
        DailyGiftData data = new DailyGiftData
        {
            Id = m_CurrentDayID,
            Date = m_NextClaimDay.ToString(CultureInfo.InvariantCulture),
        };
        ProfileManager.Ins.SaveDailyGiftData(data);
    }
    public void ImportDataString(int id, string data) {
        m_CurrentDayID = id;
        m_NextClaimDay = Convert.ToDateTime(data, CultureInfo.InvariantCulture);
    }
    public string ToDataString() {
        string s = "";
        s += "" + m_CurrentDayID + "|" + m_NextClaimDay;
        return s;
    }
    public bool IsGoodToClaim() {
        TimeSpan ts = DateTime.Now - m_NextClaimDay;
        return ts.TotalSeconds >= 0;
    }
    public void Claim() {
        m_NextClaimDay = m_NextDay;
        //m_NextClaimDay = DateTime.Now;
        DailyGift gift = m_DailyGiftConfigs.gifts[m_CurrentDayID % 7];
        ClaimGift(gift.gift);
        if (IsGoodToCollectBonusGift())
            ClaimBonusGift();
        m_CurrentDayID++;
        Save();
    }
    public void ClaimBonusGift()
    {
        Gift gift = m_DailyGiftConfigs.fiveDayGift;
        ClaimGift(gift);
    }
    private void ClaimGift(Gift gift)
    {
        switch (gift.giftType)
        {
            case GiftType.GEM:
                ResourceData resourceGem = new(ResourceData.ResourceType.GEM, gift.amount,"");
                ProfileManager.PlayerData.AddGameResource(resourceGem);
                break;
            case GiftType.COIN:
                ResourceData resourceCoin = new(ResourceData.ResourceType.GOLD, gift.amount, "");
                ProfileManager.PlayerData.AddGameResource(resourceCoin);
                break;
            case GiftType.KEY_BOSS:
                ProfileManager.PlayerData.AddKey(DungeonType.BOSS_RUSH, KeyType.BONUS, gift.amount);
                break;
            case GiftType.KEY_GOLD:
                ProfileManager.PlayerData.AddKey(DungeonType.GOLD_RUSH, KeyType.BONUS, gift.amount);
                break;
            case GiftType.KEY_NEST:
                ProfileManager.PlayerData.AddKey(DungeonType.MONSTER_NEST, KeyType.BONUS, gift.amount);
                break;
            case GiftType.KEY_CHALLENGE:
                ProfileManager.PlayerData.AddKey(DungeonType.CHALLENGE_KING, KeyType.BONUS, gift.amount);
                break;
            default:
                break;
        }
        Debug.Log("ClaimGift");
    }
    private void UpdateNextDay() {
        m_NextDay = Static.GetNextDate();
    }
    public string GetTimeToNextDay() {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = m_NextDay - now;
        string time = Static.GetTimeStringFromSecond(timeSpan.TotalSeconds);
        return time;
    }

    public DailyGiftConfigs GetDailyGiftConfig()
    {
        return m_DailyGiftConfigs;
    }
    public int GetCurrentDayId()
    {
        return m_CurrentDayID;
    }
    public bool IsGoodToCollectBonusGift()
    {
        return m_CurrentDayID%7 == 5;
    }
}