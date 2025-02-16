using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Pextension;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;

public class DungeonManager : Pextension.Singleton<DungeonManager>
{
    private Pextension.MiniPool<Dungeon> dungeonsPool = new();
    [SerializeField] private Dungeon m_DungeonPrefabs;
    [SerializeField] private DungeonsData m_DungeonData;
    [SerializeField] private DungeonsConfig m_DungeonsConfig;
    [SerializeField] private List<Dungeon> m_ListDungeon;
    private DateTime m_NextDay;
    
    private DungeonType m_CurrentDungeon;

    #region Unity Functions
    private void Awake()
    {
        dungeonsPool.OnInit(m_DungeonPrefabs, 5, this.transform);
    }
    #endregion
    #region Load & Save Data
    public void LoadData(DungeonsData data)
    {
        m_DungeonData = data;
        m_DungeonsConfig = DungeonsConfig.Instance;
        for(int i = 0;i< m_DungeonsConfig.listDungeons.Count; i++)
        {
            DungeonConfig dungeonConfig = m_DungeonsConfig.GetDungeonConfigWithType(m_DungeonData.DungeonDatas[i].DType);
            SpawnDungeon(dungeonConfig, m_DungeonData.DungeonDatas[i]);
        }
        m_NextDay = Convert.ToDateTime(m_DungeonData.NextDay,CultureInfo.InvariantCulture);
        if (IsFree())
            AddFreeKeyOnNewDay();
        UpdateNextDay();
    }
    public void SaveData()
    {
        List<DungeonData> listDungeonData = new();
        for (int i = 0; i < m_ListDungeon.Count; i++)
            listDungeonData.Add(m_ListDungeon[i].SaveData());
        DungeonsData dungeonsData = new()
        {
            NextDay = m_NextDay.ToString(CultureInfo.InvariantCulture),
            DungeonDatas = listDungeonData,
        };
        ProfileManager.Ins.SaveDungeonsData(dungeonsData);
    }
    private void SpawnDungeon(DungeonConfig config, DungeonData data)
    {
        Dungeon dungeon = dungeonsPool.Spawn(transform.position, Quaternion.identity);
        dungeon.InitData(config, data);
        m_ListDungeon.Add(dungeon);
    }
    #endregion
    #region Manager functions
    public List<Dungeon> GetAllDungeon()
    {
        return m_ListDungeon;
    }
    private bool IsFree()
    {
        TimeSpan t = DateTime.Now - m_NextDay.Date;
        return t.TotalSeconds >= 0;
    }
    private void AddFreeKeyOnNewDay()
    {
        for (int i = 0; i < m_ListDungeon.Count; i++)
            m_ListDungeon[i].AddKeyOnNewDay();
    }
    public void UpdateNextDay()
    {
        m_NextDay = Static.GetNextDate();
    }
    public void EnterDungeon(DungeonType type, int level){
        m_CurrentDungeon = type;
        switch(type){
            case DungeonType.BOSS_RUSH:
                IngameManager.Ins.StartBossRush();
                break;
            case DungeonType.GOLD_RUSH:
                break;
            case DungeonType.MONSTER_NEST:
                break;
            case DungeonType.CHALLENGE_KING:
                break;
            case DungeonType.NONE:
                break;
            default:
                break;
        }
    }
    public void DungeonComplete(DungeonType type){
        switch(type){
            case DungeonType.BOSS_RUSH:
                break;
            case DungeonType.GOLD_RUSH:
                break;
            case DungeonType.MONSTER_NEST:
                break;
            case DungeonType.CHALLENGE_KING:
                break;
            case DungeonType.NONE:
                break;
            default:
                break;
        }
        UIManager.Ins.GetUI<UICDungeon>().DungeonComplete(GetCurrentDungeon());
    }
    public Dungeon GetDungeon(DungeonType type){
        return m_ListDungeon.FirstOrDefault(x => x.DungeonType == type);
    }
    public Dungeon GetCurrentDungeon()
    {
        return m_ListDungeon.FirstOrDefault(x => x.DungeonType == m_CurrentDungeon);
    }
    public void GetDungeonReward(DungeonType type, int level)
    {
        switch (type)
        {
            case DungeonType.BOSS_RUSH:
                break;
            case DungeonType.GOLD_RUSH:
                break;
            case DungeonType.MONSTER_NEST:
                break;
            case DungeonType.CHALLENGE_KING:
                break;
            case DungeonType.NONE:
                break;
            default:
                break;
        }
    }
    #endregion
    #region Editor
#if UNITY_EDITOR
    [Button]
    private void SaveDataEditor()
    {
        SaveData();
    }
#endif
    #endregion
}
[System.Serializable]
public class DungeonsData
{
    public List<DungeonData> dd;
    public string nD;
    public List<DungeonData> DungeonDatas { get => dd; set => dd = value; }
    public string NextDay { get => nD; set => nD = value; }


    public DungeonsData()
    {

    }
    public DungeonsData(List<DungeonData> list)
    {
        DungeonDatas = list;
        nD = DateTime.Now.ToString();
    }
}
[System.Serializable] 
public class DungeonData
{
    public DungeonType tp;
    public int lv;
    public bool oD;

    public DungeonType DType { get => tp; set => tp = value; }
    public int Level { get => lv; set => lv = value; }
    public bool IsOnDungeon { get => oD; set => oD = value; }

    public DungeonData()
    {

    }
    public DungeonData(DungeonType type)
    {
        DType = type;
        Level = 1;
        IsOnDungeon = false;
    }
}
