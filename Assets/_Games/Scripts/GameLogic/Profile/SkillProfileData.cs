using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SkillData {
    /// <summary>
    /// skill ID
    /// </summary>
    public int sid;
    /// <summary>
    /// skill level
    /// </summary>
    public int lv;
    /// <summary>
    /// skill Piece
    /// </summary>
    public int p;
    public SkillData() {
    }
    public SkillData(int id) {
        sid = id;
        lv = 1;
        p = 0;
    }
}
[System.Serializable]
public class SkillSlotData {
    /// <summary>
    /// Slot ID
    /// </summary>
    public int i;
    /// <summary>
    /// Unlocked
    /// </summary>
    public int u;
    /// <summary>
    /// Skill ID
    /// </summary>
    public int si;

    public SkillSlotData() { }
    public SkillSlotData(int id) {
        i = id;
        u = 1;
        // if(id == 1) {
        //     u = 1;
        // } else
        //     u = 0;

        si = -1;
    }
}
[System.Serializable]
public class SkillsData {
    /// <summary>
    /// Auto Skill
    /// </summary>
    public int at;
    public List<SkillData> skd = new();
    public List<SkillSlotData> sld = new();

    public List<SkillData> SkillDataList { get => skd; set => skd = value; }
    public List<SkillSlotData> SlotsData { get => sld; set => sld = value; }

    public SkillsData() {
        SkillDataList = new();
        SlotsData = new();
        at = 0;
    }
}