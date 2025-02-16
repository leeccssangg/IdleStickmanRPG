using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Artifact 
{
    [SerializeField] private ArtifactConfig m_Config;
    [SerializeField] private ArtifactData m_Data;
    [SerializeField] private int m_Level;
    [SerializeField] private int m_ArtifactId;
    [SerializeField] private int m_SlotId;

    public void Setup(ArtifactConfig config, int lv)
    {
        m_Config = config;
        m_Level = lv;
        m_SlotId = m_Config.slotId;
        m_ArtifactId = m_Config.artifactId;
    }
    public ArtifactData SaveData()
    {
        ArtifactData data = new()
        {
            Id = m_ArtifactId,
            Level = m_Level,
        };
        return data;
    }
    public bool IsUnlockAbleArtifact()
    {
        return m_Level < 1;
    }
    public void UnlockArtifact()
    {
        m_Level = 1;
    }
    public bool IsUpgradeAble()
    {
        BigNumber upgradeCost = new BigNumber(m_Config.baseCost * m_Level);
        Debug.Log(ProfileManager.PlayerData.GetResoureValue(ResourceData.ResourceType.ARTIFACT_STONE));
        Debug.Log(upgradeCost);
        Debug.Log(ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.ARTIFACT_STONE, upgradeCost));
        return m_Level >= 1 && m_Level < m_Config.maxLevel &&
             ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.ARTIFACT_STONE, upgradeCost);
    }
    public bool IsMaxLevel()
    {
        return m_Level == m_Config.maxLevel;
    }
    public void UpgradeArtifact()
    {
        ProfileManager.PlayerData.ConsumeGameResource(ResourceData.ResourceType.ARTIFACT_STONE, m_Config.baseCost * m_Level);
        m_Level++;
    }
    public int GetSlotId()
    {
        return m_SlotId;
    }
    public int GetLevel()
    {
        return m_Level;
    }
    public int GetArtifactId()
    {
        return m_ArtifactId;
    }
    public string GetCostUpgrade()
    {
        return (m_Config.baseCost * m_Level).ToString();
    }
    private BigNumber GetBuffValue(ArtifactBuffType type)
    {
        BigNumber value = 0;
        for (int i = 0; i < m_Config.buffs.Count; i++)
        {
            ArtifactBuff buff = m_Config.buffs[i];
            if(buff.buffType == type)
            {
                value = buff.baseStat + (m_Level - 1) * buff.increaseStat;
                break;
            }
        }

        return value;
    }
    public BigNumber GetAttack()
    {
        return GetBuffValue(ArtifactBuffType.ATK);
    }
    public BigNumber GetHP()
    {
        return GetBuffValue(ArtifactBuffType.HP);
    }

    public BigNumber GetSkillValue()
    {
        return GetBuffValue(ArtifactBuffType.SKILL);
    }
}
[System.Serializable]
public class ArtifactData
{
    public int id;
    public int lv;

    public int Id { get => id; set => id = value; }
    public int Level { get => lv; set => lv = value; }

}
