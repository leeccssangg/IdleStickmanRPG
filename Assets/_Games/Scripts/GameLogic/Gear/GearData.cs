using LitJson;
using MyExtension;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GearsDatas {
    [ListDrawerSettings()]
    [LabelText("DataList")]
    public List<GearsData> gds = new();
    public GearsDatas() {
        Gds = new();
    }
    public List<GearsData> Gds { get => gds; set => gds = value; }
    public GearsData GetDataTypeByType(GearType type) {
        GearsData data = Gds.FirstOrDefault(x => x.GearType == type);
        if (data != null) return data;
        data = new GearsData();
        data.Init(type);
        Gds.Add(data);
        return data;
    }
    
}
[System.Serializable]
public class GearsData {
    /// <summary>
    /// Unlock
    /// </summary>
    public int u;
    /// <summary>
    /// Gear Equipped
    /// </summary>
    [LabelText("Equipped ID")]
    [field: HideLabel]
    public int eq;
    /// <summary>
    /// Gear Type
    /// </summary>
    [field: HideLabel, HorizontalGroup("Type",100),ReadOnly]
    public GearType t;
    /// <summary>
    /// Gear Data List
    /// </summary>
    [LabelText("DataList")]
    [field: HideLabel, HorizontalGroup("Type/gd")]
    public List<GearData> gd;

    public List<GearData> GearDataList { get => gd; set => gd = value; }
    public GearType GearType { get => t; set => t = value; }
    public int Equipped { get => eq; set => eq = value; }

    public void Init(GearType type) {
        t = type;
        gd = new List<GearData>();
        eq = -1;
        // u = 0;
        //TODO: Cheat
        u = type is GearType.ARMOR or GearType.WEAPON ? 1 : 0;
    }
    public bool IsUnlocked() {
        return u == 1;
    }
    public bool HasEquipped() {
        return eq != -1;
    }
    public void Unlock() {
        u = 1;
    }
    public GearData GetDataByID(int id) {
        return GearDataList.FirstOrDefault(x => x.i == id);
    }
    public GearData GetGearDataEquipped() {
        if(eq == -1) return null;
        return GetDataByID(eq);
    }
    public void SetEquip(int id) {
        eq = id;
    }
}

[System.Serializable]
public class GearData {
    /// <summary>
    /// Gear ID
    /// </summary>
    public int i = 0;
    /// <summary>
    /// Gear Level
    /// </summary>
    public int l = 1;
    /// <summary>
    /// Gear Piece
    /// </summary>
    public int p = 0;
    /// <summary>
    /// Gear Type
    /// </summary>
    public void Init(int id) {
        i = id;
        l = 1;
    }
}
