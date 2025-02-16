using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GearInfo {
    //[field: HideLabel, HorizontalGroup("m_Type",100)]
    [SerializeField]private GearType m_Type;
    // [field: HideLabel, HorizontalGroup("m_Data")]
    // [SerializeField,BoxGroup("stat") ]private BigNumber m_OwnerEffect;
    [LabelText("Data")]
    [SerializeField] private GearData m_Data;
    [SerializeField] private GearConfig m_Config;

    private int m_RequirePieceAmount;

    public GearType Type { get => m_Type; set => m_Type = value; }
    public GearData Data { get => m_Data; set => m_Data = value; }
    public int Level { get => m_Data.l; set => m_Data.l = value; }
    public int ID { get => m_Data.i; set => m_Data.i = value; }
    public string Name { get => m_Config.gearName;}
    public int Piece { get => m_Data.p; private set => m_Data.p = value; }
    public GearConfig Config { get => m_Config; set => m_Config = value; }
    public Rarity Rarity {get => Config.rarity; }

    private Sprite m_IconType;
    private Sprite m_Icon;
    public void Init(GearType type,GearData data,GearConfig config) {
        Type = type;
        m_Data = data;
        m_Config = config;
    }
    public bool IsUnlocked(){
        return Piece > 0 || Level > 1;
    }
    public bool IsLock() {
        return Piece == 0 && Level == 1;
    }
    public void Upgrade() {
        Piece -= GetRequirePieceAmount();
        Level++;
    }
    public void UpgradeMaxLevel(){
        while(UpgradeAble()){
            Upgrade();
        }
    }
    public bool UpgradeAble() {
        return Level < 100 && m_Data.p >= GetRequirePieceAmount();
    }
    public void AddPiece(int amount) {
        Piece += amount;
    }
    public int GetRequirePieceAmount() {
        return GameData.Ins.GetLeveupRequiredPiece(Level + 1);
    }
    public float GetLevelProgress() {
        return (float)Piece/GetRequirePieceAmount();
    }
    public string GetLevelProgressString() {
        return $"{Piece}/{GetRequirePieceAmount()}";
    }
    public Sprite GetIcon()
    {
        if(m_Icon == null) m_Icon = SpriteManager.Ins.GetGearSprite(Type,ID);
        return m_Icon;
    }
    public Sprite GetIconType()
    {
        if(m_IconType == null) m_IconType = SpriteManager.Ins.GetGearTypeSprite(Type);
        return m_IconType;
    }

#region STAT
    private Stat m_OwnerEffect = new Stat();
    public Stat GetOwnerEffect()
    {
       // m_OwnerEffect.StatType = Config.ownerStatType;
        m_OwnerEffect.Value = FormulaManager.Ins.GetEquipmentOwnedEffect(Level,Rarity);;
        return m_OwnerEffect;
    }
    // public BigNumber GetOwnerEffect(){
    //    // m_OwnerEffect = FormulaManager.Instance.GetEquipmentOwnedEffect(Level,Rarity);
    //     return FormulaManager.Ins.GetEquipmentOwnedEffect(Level,Rarity);
    // }
    
    private Stat m_EquippedEffect = new Stat();
    public Stat GetEquippedEffect()
    {
        //Stat baseStat = Config.equippedStatList[0];
        // m_EquippedEffect.StatType = baseStat.StatType;
        // m_EquippedEffect.Value = FormulaManager.Ins.GetEquipmentEquipEffect(Level,baseStat.Value);
        return m_EquippedEffect;
    }
#endregion
}
[System.Serializable]
public class Gears {
    public List<GearInfo> gd;
    public List<GearInfo> GearList { get => gd; set => gd = value; }
}
