using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "DailyGiftConfigs", menuName = "GlobalConfigs/DailyGiftConfigs")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class DailyGiftConfigs : GlobalConfig<DailyGiftConfigs> {
    public List<DailyGift> gifts = new();
    public Gift fiveDayGift;
}