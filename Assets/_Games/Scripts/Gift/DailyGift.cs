using Sirenix.OdinInspector;

public enum GiftType
{
    NONE,
    GEM,
    COIN,
    KEY_BOSS,
    KEY_GOLD,
    KEY_NEST,
    KEY_CHALLENGE,
    ARTIFACT,
}
[System.Serializable]
public class DailyGift{
    public int day;
    public Gift gift;
}
[System.Serializable]
public class ResourceRewardPackage
{
    public ResourceData.ResourceType rewardType;
    public int amount;
}
[System.Serializable]
public class Gift
{
    public GiftType giftType;
    public int amount;
}
