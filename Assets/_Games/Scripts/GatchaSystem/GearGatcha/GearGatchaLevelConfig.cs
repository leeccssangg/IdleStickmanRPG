using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Pextension;

[System.Serializable]
public class GearGatchaLevelConfig
{
    public int level;
    public int upgradeNeeded;
    public Probability<Rarity> rarityConfig;

    public void Load(Dictionary<string, string> dic)
    {
        level = int.Parse(dic["Level"]);
        upgradeNeeded = int.Parse(dic["UpgradeNeeded"]);
        rarityConfig = new(new());
        ProbabilityValue<Rarity> common = new(Rarity.COMMON, int.Parse(dic["COMMON"]), () => rarityConfig.UpdateTotalValue());
        ProbabilityValue<Rarity> uncommon = new(Rarity.COMMON, int.Parse(dic["UNCOMMON"]), () => rarityConfig.UpdateTotalValue());
        ProbabilityValue<Rarity> rare = new(Rarity.COMMON, int.Parse(dic["RARE"]), () => rarityConfig.UpdateTotalValue());
        ProbabilityValue<Rarity> epic = new(Rarity.COMMON, int.Parse(dic["EPIC"]), () => rarityConfig.UpdateTotalValue());
        ProbabilityValue<Rarity> legend = new(Rarity.COMMON, int.Parse(dic["LEGEND"]), () => rarityConfig.UpdateTotalValue());
        ProbabilityValue<Rarity> immortal = new(Rarity.COMMON, int.Parse(dic["IMMORTAL"]), () => rarityConfig.UpdateTotalValue());
        ProbabilityValue<Rarity> ancient = new(Rarity.COMMON, int.Parse(dic["ANCIENT"]), () => rarityConfig.UpdateTotalValue());
        rarityConfig.m_ProbabilityValueList.Add(common);
        rarityConfig.m_ProbabilityValueList.Add(uncommon);
        rarityConfig.m_ProbabilityValueList.Add(rare);
        rarityConfig.m_ProbabilityValueList.Add(epic);
        rarityConfig.m_ProbabilityValueList.Add(legend);
        rarityConfig.m_ProbabilityValueList.Add(immortal);
        rarityConfig.m_ProbabilityValueList.Add(ancient);
    }
}
