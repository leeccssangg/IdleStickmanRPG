using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Unity.VisualScripting.FullSerializer;
using Sirenix.Utilities;
using Random = UnityEngine.Random;

public class ArtifactsManager:Pextension.Singleton<ArtifactsManager>,IHp,IAttack {
    [SerializeField] private ArtifactsData m_Data;
    [SerializeField] private List<Artifact> m_ArtifactsList;
    [SerializeField] private ArtifactSlotDic m_ArtifactSlotDic;
    [SerializeField] private ArtifactsGlobalConfig m_GlobalConfig;

    [SerializeField] private ArtifactInGame[] m_ArtifactPrefab;
    [ShowInInspector]
    private List<ArtifactInGame> m_ArtifactInGame;

    #region Save & Load data
    public void LoadData(ArtifactsData data)
    {
        m_ArtifactInGame = new(4);
        for (int i = 0; i < 4; i++)
        {
            m_ArtifactInGame.Add(null);
        }
        m_GlobalConfig = ArtifactsGlobalConfig.Instance;
        m_Data = data;
        if(m_Data.ArtifactsDataList.Count <= 0)
            CreateNewData();
        else
            LoadOldData();
    }
    private void CreateNewData() {
        m_ArtifactsList = new();
        m_ArtifactSlotDic = new();
        int slotCount = m_GlobalConfig.configs.Count;
        for(int i = 0;i < slotCount;i++) {
            ArtifactConfig config = m_GlobalConfig.configs[i];
            Artifact artifact = new();
            artifact.Setup(config,0);
            m_ArtifactsList.Add(artifact);
        }
        for(int i = 1;i <= m_GlobalConfig.GetTotalSlotId();i++) {
            m_ArtifactSlotDic.Add(i,null);
        }
        SaveData();
        
    }
    private void LoadOldData() {
        m_ArtifactsList = new();
        for(int i = 0;i < m_Data.ArtifactsDataList.Count;i++) {
            int slotId = m_Data.ArtifactsDataList[i].Id;
            int level = m_Data.ArtifactsDataList[i].Level;
            ArtifactConfig config = m_GlobalConfig.GetArtifactConfigById(slotId);
            Artifact artifact = new();
            artifact.Setup(config,level);
            m_ArtifactsList.Add(artifact);
        }
        for(int i = 1;i <= m_GlobalConfig.GetTotalSlotId();i++) {
            int artifactId = m_Data.EquipDic[i - 1];
            Artifact artifact = GetArtifactWithId(artifactId);
            m_ArtifactSlotDic.Add(i,artifact);
        }
    }
    public void SaveData() {
        List<ArtifactData> list = new();
        for(int i = 0;i < m_ArtifactsList.Count;i++) {
            ArtifactData artifactData = m_ArtifactsList[i].SaveData();
            list.Add(artifactData);
        }
        List<int> dic = new();
        for(int i = 0;i < m_ArtifactSlotDic.Count;i++) {
            int slotId = m_ArtifactSlotDic.ElementAt(i).Key;
            int artifactId = 0;
            if(m_ArtifactSlotDic.ElementAt(i).Value != null)
                artifactId = m_ArtifactSlotDic.ElementAt(i).Value.GetArtifactId();
            dic.Add(artifactId);
        }
        ArtifactsData data = new() {
            ArtifactsDataList = list,
            EquipDic = dic,
        };
        ProfileManager.Ins.SaveArtifactsData(data);
    }
    #endregion
    #region Manager functions

    private void Update()
    {
        for (int i = 0; i < m_ArtifactInGame.Count; i++)
        {
            var artifact = m_ArtifactInGame[i];
            if(artifact != null) artifact.OnUpdateArtifact();
        }
    }

    #region Get set functions
    public int GetNumArtifactUnlocked()
    {
        int num = 0;
        for (int i = 0; i < m_ArtifactsList.Count; i++)
        {
            if (m_ArtifactsList[i].GetLevel() > 0)
                num++;
        }
        return num;
    }
    public Artifact GetArtifactWithId(int id)
    {
        for (int i = 0; i < m_ArtifactsList.Count; i++)
        {
            if (m_ArtifactsList[i].GetArtifactId() == id)
                return m_ArtifactsList[i];
        }
        return null;
    }
    public List<Artifact> GetListArtifactWithSlotId(int slotId)
    {
        List<Artifact> list = new();
        for (int i = 0; i < m_ArtifactsList.Count; i++)
        {
            if (m_ArtifactsList[i].GetSlotId() == slotId)
                list.Add(m_ArtifactsList[i]);
        }
        return list;
    }
    public BigNumber GetCostUnlock()
    {
        return new BigNumber(m_GlobalConfig.GetCost(GetNumArtifactUnlocked()));
    }
    public Artifact GetArtifactEquipped(int id)
    {
        foreach (int slot in m_ArtifactSlotDic.Keys)
        {
            if(m_ArtifactSlotDic[slot] == null) continue;
            if (m_ArtifactSlotDic[slot].GetArtifactId() == id)
                return m_ArtifactSlotDic[slot];
        }

        return null;
    }
    public ArtifactInGame GetArtifactInGame(int id)
    {
        return m_ArtifactPrefab.FirstOrDefault(x => x.Id == id);
    }
    #endregion
    #region Gatcha functions
    public void GatchaAritfact()
    {
        bool tmp = false;
        while (!tmp)
        {
            int i = Random.Range(0, m_ArtifactsList.Count);
            Artifact artifact = m_ArtifactsList[i];
            if (!IsUnlockAbleArtifact(artifact))
                continue;
            UnLockArtifact(artifact);
            tmp = true;
            Debug.Log("Unlock Artifact");
        }
    }
    public void UnLockArtifact(Artifact artifact)
    {
        if (!m_ArtifactsList.Contains(artifact))
            return;
        if (!IsUnlockAbleArtifact(artifact))
            return;
        ProfileManager.PlayerData.ConsumeGameResource(ResourceData.ResourceType.ARTIFACT_STONE, m_GlobalConfig.GetCost(GetNumArtifactUnlocked()));
        artifact.UnlockArtifact();
        SaveData();
        DoUpdateStat();
        EventManager.TriggerEvent("UpdateArtifact");
    }
    public bool IsUnlockAllArtifact()
    {
        return GetNumArtifactUnlocked() >= m_ArtifactsList.Count;
    }
    public bool IsHaveEnoughResource()
    {
        return ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.ARTIFACT_STONE, new BigNumber(m_GlobalConfig.GetCost(GetNumArtifactUnlocked())));
    }
    public bool IsUnlockThreeOfLastSlotArtifact(int curSlotId)
    {
        if (curSlotId == 1)
            return true;
        List<Artifact> list = GetListArtifactWithSlotId(curSlotId - 1);
        int unlockCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetLevel() > 0)
                unlockCount++;
        }
        return unlockCount >= 3;
    }
    public bool IsUnlockAllLastSlotArtifact(int curSlotId)
    {
        if (curSlotId == 1)
            return true;
        for (int i = 0; i < m_ArtifactsList.Count; i++)
        {
            if (m_ArtifactsList[i].GetSlotId() == curSlotId - 1)
                if (m_ArtifactsList[i].GetLevel() < 1)
                    return false;
        }
        return true;
    }

    private bool IsUnlockAbleArtifact(Artifact artifact)
    {
        if (IsUnlockAllArtifact())
            return false;
        if (!m_ArtifactsList.Contains(artifact))
            return false;
        if (!IsUnlockThreeOfLastSlotArtifact(artifact.GetSlotId()))
            return false;
        return artifact.IsUnlockAbleArtifact();
    }
    #endregion
    #region Equip functions

    private bool IsEquipAbleArtifact(Artifact artifact)
    {
        int slotId = artifact.GetSlotId();
        return artifact.GetLevel() > 0 && m_ArtifactSlotDic[slotId] != artifact;
    }
    public void EquipArtifact(Artifact artifact)
    {
        if (!IsEquipAbleArtifact(artifact))
            return;
        int slotId = artifact.GetSlotId();
        if (m_ArtifactSlotDic.TryGetValue(slotId, out Artifact art))
            m_ArtifactSlotDic[slotId] = artifact;
        
        if(m_ArtifactInGame[slotId-1] != null) m_ArtifactInGame[slotId-1].OnRelease();
        
        
        if(GetArtifactInGame(artifact.GetArtifactId()))
        {
            ArtifactInGame artifactInGame = Instantiate(GetArtifactInGame(artifact.GetArtifactId()),transform);
            artifactInGame.OnInit(artifact);
            m_ArtifactInGame[slotId-1] = artifactInGame;
        }
        
        SaveData();
        EventManager.TriggerEvent("UpdateArtifact");
    }

    private bool IsUnEquipAbleArtifact(Artifact artifact)
    {
        int slotId = artifact.GetSlotId();
        return m_ArtifactSlotDic[slotId] == artifact;
    }
    public void UnEquipArtifact(Artifact artifact)
    {
        if (!IsUnEquipAbleArtifact(artifact))
            return;
        int slotId = artifact.GetSlotId();
        if (m_ArtifactSlotDic.TryGetValue(slotId, out Artifact art))
            m_ArtifactSlotDic[slotId] = null;
        if(m_ArtifactInGame[slotId-1] != null) m_ArtifactInGame[slotId-1].OnRelease();
        SaveData();
        EventManager.TriggerEvent("UpdateArtifact");
    }
    public bool IsEquipedArtifact(Artifact artifact)
    {
        int slotId = artifact.GetSlotId();
        return m_ArtifactSlotDic[slotId] == artifact;
    }

    #endregion
    #region Upgrade functions
    public void UpgradeArtifact(Artifact artifact)
    {
        if (!m_ArtifactsList.Contains(artifact))
            return;
        if (!IsUpgradeAbleArtifact(artifact))
            return;
        artifact.UpgradeArtifact();
        DoUpdateStat();
        SaveData();
        EventManager.TriggerEvent("UpdateArtifact");
    }
    public bool IsUpgradeAbleArtifact(Artifact artifact)
    {
        if (!m_ArtifactsList.Contains(artifact))
            return false;
        return artifact.IsUpgradeAble();
    }
    #endregion
    #endregion
    #region Editor
#if UNITY_EDITOR
    [Button]
    public void RandomUnlockArtifact() {
        GatchaAritfact();
        Debug.Log("Unlock Artifact");
    }
    [Button]
    public void RandomUpgradeArtifact() {
        int i = Random.Range(0,m_ArtifactsList.Count);
        Artifact artifact = m_ArtifactsList[i];
        if(!IsUpgradeAbleArtifact(artifact))
            return;
        UpgradeArtifact(artifact);
        Debug.Log("Upgrade Artifact");
    }
    [Button]
    public void RandomEquipArtifact() {
        int i = Random.Range(0,m_ArtifactsList.Count);
        Artifact artifact = m_ArtifactsList[i];
        if(!IsEquipAbleArtifact(artifact))
            return;
        EquipArtifact(artifact);
        Debug.Log("Equip Artifact");
    }
    [Button]
    public void RandomUnequipArtifact() {


    }
#endif
    #endregion

    #region STAT
    public BigNumber GetHp()
    {
        BigNumber totalHp = 0;
        for (int i = 0; i < m_ArtifactsList.Count; i++)
        {  
            if(m_ArtifactsList[i].GetLevel() > 0) totalHp += m_ArtifactsList[i].GetHP();
        }
        Debug.Log($"Get Artifacts Hp: {totalHp}");
        return totalHp;
    }

    public BigNumber GetAttack()
    {
        BigNumber totalAttack = 0;
        for (int i = 0; i < m_ArtifactsList.Count; i++)
        {  
            if(m_ArtifactsList[i].GetLevel() > 0) totalAttack += m_ArtifactsList[i].GetAttack();
        }
        return totalAttack;
    }
    private static void DoUpdateStat()
    {
        PropertiesManager.Ins.UpdateAttack();
        PropertiesManager.Ins.UpdateHp();
    }
    #endregion
    
}
[System.Serializable]
public class ArtifactsData {
    public List<ArtifactData> ad;
    public List<int> eqd;

    public List<ArtifactData> ArtifactsDataList { get => ad; set => ad = value; }
    public List<int> EquipDic { get => eqd; set => eqd = value; }

    public ArtifactsData() {
        ArtifactsDataList = new();
        EquipDic = new();
    }
}
[System.Serializable]
public class ArtifactSlotDic:UnitySerializedDictionary<int,Artifact> {

}
