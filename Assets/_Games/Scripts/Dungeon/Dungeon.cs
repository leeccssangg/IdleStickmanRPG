using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[GUIColor("@MyExtension.EditorExtension.GetColor(\"Dungeon\", (int)$value)")]
public enum DungeonType
{
    BOSS_RUSH = 0,
    GOLD_RUSH = 1,
    MONSTER_NEST = 3,
    CHALLENGE_KING = 4,
    NONE = 5,
}
public class Dungeon : MonoBehaviour
{
    [FormerlySerializedAs("m_DungeonType")] [SerializeField] private DungeonType m_DungeonDungeonType;
    [SerializeField] private DungeonConfig m_DungeonConfig;
    [SerializeField] private DungeonData m_DungeonData;
    [SerializeField] private int m_CurrentLevel;
    [SerializeField] private int m_KeyFreeCount;
    [SerializeField] private int m_KeyBonusCount;
    [SerializeField] private int m_KeyAdsCount;
    [SerializeField] private bool isOnDungeon;
    public DungeonType DungeonType{ get => m_DungeonDungeonType; set => m_DungeonDungeonType = value; }

#region Save & Load data
    public void InitData(DungeonConfig config, DungeonData data)
    {
        m_DungeonConfig = config;
        m_DungeonData = data;
        m_CurrentLevel = m_DungeonData.Level;
        DungeonType = m_DungeonData.DType;
        isOnDungeon = m_DungeonData.IsOnDungeon;
        EventManager.StartListening("Update" + DungeonType.ToString() + KeyType.FREE.ToString(), UpdateFreeKeyStatus);
        EventManager.StartListening("Update" + DungeonType.ToString() + KeyType.BONUS.ToString(), UpdateBonusKeyStatus);
        EventManager.StartListening("Update" + DungeonType.ToString() + KeyType.ADS.ToString(), UpdateAdsKeyStatus);
        UpdateAdsKeyStatus();
        UpdateBonusKeyStatus();
        UpdateFreeKeyStatus();
        if (isOnDungeon)
            EnterDungeon(m_CurrentLevel);
    }
    public DungeonData SaveData()
    {
        DungeonData dungeonData = new()
        {
            DType = DungeonType,
            Level = m_CurrentLevel,
            IsOnDungeon = isOnDungeon,
        };
        return dungeonData;
    }
    #endregion
    #region Key Function
    public void AddKeyOnNewDay()
    {
        int keyFreeNeeded = m_DungeonConfig.NumKeyADay - m_KeyFreeCount;
        if (keyFreeNeeded > 0)
            ProfileManager.PlayerData.AddKey(DungeonType, KeyType.FREE, keyFreeNeeded);
        int keyAdsNeeded = m_DungeonConfig.NumKeyADay - m_KeyAdsCount;
        if(keyAdsNeeded > 0)
            ProfileManager.PlayerData.AddKey(DungeonType, KeyType.ADS, keyAdsNeeded);
    }
    public void UpdateFreeKeyStatus()
    {
        m_KeyFreeCount = ProfileManager.PlayerData.GetKeyValue(DungeonType, KeyType.FREE);
    }
    public void UpdateBonusKeyStatus()
    {
        m_KeyBonusCount = ProfileManager.PlayerData.GetKeyValue(DungeonType, KeyType.BONUS);
    }
    public void UpdateAdsKeyStatus()
    {
        m_KeyAdsCount = ProfileManager.PlayerData.GetKeyValue(DungeonType, KeyType.ADS);
    }
    public KeyType KeyEnterDungeon()
    {
        KeyType key = KeyType.NONE;
        if (ProfileManager.PlayerData.IsEnoughKey(DungeonType, KeyType.FREE, 1))
            key = KeyType.FREE;
        else if (ProfileManager.PlayerData.IsEnoughKey(DungeonType, KeyType.BONUS, 1))
            key = KeyType.BONUS;
        else if (ProfileManager.PlayerData.IsEnoughKey(DungeonType, KeyType.ADS, 1))
            key = KeyType.ADS;
        return key;
    }
    #endregion
    #region Dungeon Function
    public DungeonConfig GetDungeonConfig()
    {
        return m_DungeonConfig;
    }
    public int GetLastLevel()
    {
        return m_DungeonConfig.GetLastLevel();
    }
    public string GetDungeonName()
    {
        return DungeonType.ToString();
    }
    public int GetCurrentLevel()
    {
        return m_CurrentLevel;
    }
    public int GetAllKeyCount()
    {
        return m_KeyAdsCount + m_KeyBonusCount + m_KeyFreeCount;
    }
    public int GetFreeKeyCount()
    {
        return m_KeyFreeCount;
    }
    public int GetFreeBonusKeyCount()
    {
        return m_KeyFreeCount + m_KeyBonusCount;
    }
    public int GetNumKeyADay()
    {
        return m_DungeonConfig.NumKeyADay;
    }
    public void CheckEnterDungeon(int level){
        DungeonManager.Ins.EnterDungeon(DungeonType, level);
        KeyType keyType = KeyEnterDungeon();
        // if (KeyEnterDungeon() != KeyType.NONE)
        //     DungeonComplete(level,keyType);
    }
    public void EnterDungeon(int level)
    {
        isOnDungeon = true;
        DungeonManager.Ins.SaveData();
    }
    public void DungeonComplete(int level,KeyType keyType)
    {
        ProfileManager.PlayerData.ConsumeKey(DungeonType, keyType, 1);
        ClaimReward(level);
        if (level == m_CurrentLevel)
            m_CurrentLevel++;
        isOnDungeon = false;
        DungeonManager.Ins.SaveData();
        EventManager.TriggerEvent("Dungeon" + DungeonType.ToString() + "NextLevel");
    }
    public void ClaimReward(int level)
    {
        ProfileManager.PlayerData.AddGameResource(m_DungeonConfig.LevelConfigs[level-1].Gift);
    }
    #endregion
    #region Editor
#if UNITY_EDITOR
    [Button]
    private void EnterDungeonEditor()
    {
        KeyType keyType = KeyEnterDungeon();
        if(KeyEnterDungeon() != KeyType.NONE)
            DungeonComplete(m_CurrentLevel,keyType);
    }
    [Button]
    private void DungeonCompleteFree()
    {
        DungeonComplete(m_CurrentLevel, KeyType.FREE);
    }
    [Button]
    private void DungeonCompleteBonus()
    {
        DungeonComplete(m_CurrentLevel, KeyType.BONUS);
    }
    [Button]
    private void DungeonCompleteAds()
    {
        DungeonComplete(m_CurrentLevel, KeyType.ADS);
    }
#endif
    #endregion
}
