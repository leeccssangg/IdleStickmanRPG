using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
[System.Serializable]
public class PlayerProfileChien {
    /// <summary>
    /// Auto Skill
    /// </summary>
    public int ak = 0;
    /// <summary>
    /// Skill Datas
    /// </summary>
    public List<CollectorData> skd = new List<CollectorData>();
    /// <summary>
    /// StickmanData
    /// </summary>
    public List<CollectorData> smd = new List<CollectorData>();
    /// <summary>
    /// Skill Line up
    /// </summary>
    public List<LineUp> skl = new List<LineUp>();
    /// <summary>
    /// Stickman Line up
    /// </summary>
    public List<LineUp> sml = new List<LineUp>();
    /// <summary>
    /// Stat Data
    /// </summary>
    public List<StatData> sd = new List<StatData>();

    private float m_SaveCooldownTime = 0;
    public void CreateNewPlayer() {
        for(int i = 0;i < 6;i++) {
            skl.Add(new LineUp() { sid = i + 1,ul = 0 });
        }
        for(int i = 0;i < 5;i++) {
            sml.Add(new LineUp() { sid = i + 1,ul = 1 });
        }

        UnLockSlotSkill(1);
        UnLockSlotSkill(2);
        UnLockSlotSkill(3);
        AddSkillPiece(1,1);
        AddSkillPiece(2,1);
        AddSkillPiece(3,1);
        AddSkillPiece(4,1);
        AddSkillPiece(5,1);
        AddSkillPiece(6,1);
        AddSkillPiece(19,1);
        AddSkillPiece(20,1);
        AddSkillPiece(22,1);

        for(int i = 0;i < 112;i++) {
            AddStickmanPiece(i + 1,15);
        }
        
    }
    public void LoadLocalProfile() {
       
    }
    #region Skill
    public CollectorData GetSkillData(int id) {
        for(int i = 0;i < skd.Count;i++) {
            CollectorData skillData = skd[i];
            if(skillData.i == id) {
                return skillData;
            }
        }
        return null;
    }
    public void AddSkillPiece(int skillID,int pieceAmount) {
        CollectorData skillData = GetSkillData(skillID);
        if(skillData == null) {
            skillData = new CollectorData();
            skillData.Init(skillID);
            skd.Add(skillData);
        }
        skillData.AddPiece(pieceAmount);
    }
    public void LevelUpSkill(int id) {
        CollectorData skillData = GetSkillData(id);
        int nextLevel = skillData.GetLevel() + 1;
        int requiredPiece = GameData.Ins.GetSkillDataConfig(id).GetLevelRequirePiece(nextLevel);
        skillData.ConsumePiece(requiredPiece);
        skillData.LevelUp();
        SaveDataToLocal(false);
    }
    public void EquipSkill(int slotID,int skillID) {
        CollectorData skillData = GetSkillData(skillID);
        skillData.SetSlotID(slotID);
        LineUp skillLineUp = GetSkillLineUp(slotID);
        skillLineUp.id = skillID;
        SaveDataToLocal(false);
    }
    public void RemoveSkill(int skillID) {
        CollectorData skillData = GetSkillData(skillID);
        FreeSkillLineUpSlot(skillData.GetSlotID());
        skillData.SetSlotID(0);
        SaveDataToLocal(false);
    }
    public void SetSkillLineup(int slotID,int skillID,bool isChange = false) {
        LineUp skillLineUp = GetSkillLineUp(slotID);
        skillLineUp.id = skillID;
        CollectorData skillData = GetSkillData(skillID);
        if(skillData != null) {
            skillData.SetSlotID(slotID);
        }
    }
    public void FreeSkillLineUpSlot(int slotID) {
        LineUp skillLineUp = GetSkillLineUp(slotID);
        skillLineUp.SetFree();
    }
    public List<LineUp> GetSkillLineUp() {
        return skl;
    }
    public LineUp GetSkillLineUp(int slotID) {
        return skl[slotID - 1];
    }
    public LineUp GetSkillFreeSlot() {
        for(int i = 0;i < skl.Count;i++) {
            LineUp slot = skl[i];
            if(slot.IsFree()) {
                return slot;
            }
        }
        return null;
    }
    public void UnLockSlotSkill(int slotID) {
        GetSkillLineUp(slotID).Unlock();
    }
    public void SetAutoSkill(bool isAuto) {
        ak = isAuto ? 1 : 0;
    }
    #endregion
    #region STICKMAN
    public CollectorData GetStickmanData(int id) {
        for(int i = 0;i < smd.Count;i++) {
            CollectorData smData = smd[i];
            if(smData.i == id) {
                return smData;
            }
        }
        return null;
    }
    public void AddStickmanPiece(int smID,int pieceAmount) {
        CollectorData smData = GetStickmanData(smID);
        if(smData == null) {
            smData = new CollectorData();
            smData.Init(smID);
            smd.Add(smData);
        }
        smData.AddPiece(pieceAmount);
    }
    public void LevelUpStickman(int id) {
        CollectorData smData = GetStickmanData(id);
        int nextLevel = smData.GetLevel() + 1;
        int requiredPiece = GameData.Ins.GetLeveupRequiredPiece(nextLevel);
        smData.ConsumePiece(requiredPiece);
        smData.LevelUp();
        SaveDataToLocal(false);
    }
    public void EquipStickman(int slotID,int smID) {
        CollectorData stmData = GetStickmanData(smID);
        stmData.SetSlotID(slotID);
        LineUp smLineUp = GetStickmanLineUp(slotID);
        smLineUp.id = smID;
        SaveDataToLocal(false);
    }
    public void RemoveStickman(int smID) {
        CollectorData smData = GetStickmanData(smID);
        FreeStickmanLineUpSlot(smData.GetSlotID());
        smData.SetSlotID(0);
        SaveDataToLocal(false);
    }
    public void FreeStickmanLineUpSlot(int slotID) {
        LineUp smLineUp = GetStickmanLineUp(slotID);
        smLineUp.SetFree();
    }
    public List<LineUp> GetStickmanLineUp() {
        return sml;
    }
    public LineUp GetStickmanLineUp(int slotID) {
        return sml[slotID - 1];
    }
    public LineUp GetStickmanFreeSlot() {
        for(int i = 0;i < sml.Count;i++) {
            LineUp slot = sml[i];
            if(slot.IsFree()) {
                return slot;
            }
        }
        return null;
    }
    #endregion
    #region Stat
    public void LevelupStat(MainStatType statType) {
        StatData stat =  GetStatData(statType);
        stat.levelUp();
        SaveDataToLocal(true);
    }
    public List<StatData> GetListStat()=>sd;
    public StatData GetStatData(MainStatType statType) {
        for(int i = 0;i < sd.Count;i++) {
            StatData std = sd[i];
            if(std.st == statType) {
                return std;
            }
        }
        return null;
    }
    #endregion
    #region SAVE DATA
    public void SaveDataToLocal(bool checkCooldown = true) {
        if(checkCooldown) {
            if(m_SaveCooldownTime > 0)
                return;
        }
        m_SaveCooldownTime = 5f;
        //PrepareDataToSave();
        //Debug.Log("Save Time " + m_sCurrentOnlineTime);
        string piJson = this.ToJsonString();
        //Debug.Log("Save Data " + piJson);
        ProfileManager_Chien.Ins.SaveDataText(piJson);

    }
    public string ToJsonString() {
        return LitJson.JsonMapper.ToJson(this);
    }
    #endregion

}
[System.Serializable]
public class LineUp {
    public int sid;
    public int id;
    public int ul;
    public LineUp() {
        id = -1;
    }
    public void Unlock() {
        if(ul > 0)
            return;
        ul = 1;
    }
    public bool IsUnlocked() {
        return ul == 1;
    }
    public void SetFree() {
        id = -1;
    }
    public bool IsFree() {
        return id == -1 && ul == 1;
    }
}
[System.Serializable]
public class StatData {
    public int i;
    public MainStatType st;
    public int l;
    public int Level { get => l; set => l = value; }

    public void Init(int id,MainStatType stat) {
        i = id;
        l = 1;
        st = stat;
    }
    public int GetLevel() => l;
    public void levelUp() {
        l++;
    }
}