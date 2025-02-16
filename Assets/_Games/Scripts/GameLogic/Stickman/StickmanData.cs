using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StickmansData {
    public List<StickmanData> std = new();
    public List<StickmanSlotData> sld = new();
    public StickmansData() {
        StickmanDataList = new();
        SlotsData = new();
    }
    public List<StickmanData> StickmanDataList { get => std; set => std = value; }
    public List<StickmanSlotData> SlotsData { get => sld; set => sld = value; }
}

[System.Serializable]
public class StickmanData {
    /// <summary>
    /// Stickman ID
    /// </summary>
    public int id = 1;
    /// <summary>
    /// Stickman rank
    /// </summary>
    public int lv = 1;
    /// <summary>
    /// Stickman Piece
    /// </summary>
    public int p = 0;
    public StickmanData() { }
    public StickmanData(int id) {
        this.id = id;
        this.lv = 1;
        this.p = 0;
    }
}
[System.Serializable]
public class StickmanSlotData {
    /// <summary>
    /// Slot ID
    /// </summary>
    public int i;
    /// <summary>
    /// Unlocked
    /// </summary>
    public int u;
    /// <summary>
    /// Stickman ID
    /// </summary>
    public int smi;

    public StickmanSlotData() { }
    public StickmanSlotData(int id) {
        i = id;
            u = 1;
        // if(id == 1) {
        // } else
        //     u = 0;
        smi = -1;
    }
}
