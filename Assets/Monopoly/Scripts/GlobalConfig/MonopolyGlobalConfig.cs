using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;
using TW.Utility.CustomType;


[CreateAssetMenu(fileName = "MonopolyGlobalConfig", menuName = "GlobalConfigs/MonopolyGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class MonopolyGlobalConfig : GlobalConfig<MonopolyGlobalConfig>
{
    [field: SerializeField] public int AmountResetDaily { get; private set; }

    [field: SerializeField, SuffixLabel("seconds ", true)]
    public int TimeDiceRefill { get; private set; }

    [field: SerializeField, SuffixLabel("seconds ", true)]
    public int TimeMagnetRefill { get; private set; }

    [field: SerializeField, SuffixLabel("seconds ", true)]
    public int TimeDice6Refill { get; private set; }

    [field: SerializeField] public BlockConfig[] NormalBlockConfigs { get; private set; }
    [field: SerializeField] public BlockConfig[] SpecialBlockConfigs { get; private set; }
    [field: SerializeField] public BlockConfig[] HighlightBlockConfigs { get; private set; }
    [field: SerializeField] public BlockConfig TrapBlockConfig { get; private set; }
    
    public BlockConfig GetHighLightBlockConfig(EBlockType blockType)
    {
        return HighlightBlockConfigs.FirstOrDefault(x => x.BlockType == blockType);
    }
//     [Button]
//     private void UpdateBlockId()
//     {
// #if UNITY_EDITOR
//         UnityEditor.EditorUtility.SetDirty(this);
//         for (int i = 0; i < NormalBlockConfigs.Length; i++)
//         {
//             NormalBlockConfigs[i].BlockId = $"N{i}";
//         }
//
//         for (int i = 0; i < SpecialBlockConfigs.Length; i++)
//         {
//             SpecialBlockConfigs[i].BlockId = $"S{i}";
//         }
//
//         for (int i = 0; i < HighlightBlockConfigs.Length; i++)
//         {
//             HighlightBlockConfigs[i].BlockId = $"H{i}";
//         }
//         TrapBlockConfig.BlockId = "T";
//         UnityEditor.AssetDatabase.SaveAssets();
//         UnityEditor.AssetDatabase.Refresh();
// #endif
//     }
}

public enum EBlockType
{
    Start = 0,
    Gold = 1,
    Gem = 2,
    Diamond = 3, // Use to upgrade skill tree
    Crystal = 4, // Use to upgrade character
    Key = 5,
    Empty = 6,
    UpStage = 7,
    Trap = 8,
    Dice6Only = 9,
    Magnet = 10,
    Dice = 11,
}

[System.Serializable]
public class BlockConfig
{
    [field: PreviewField(65), HorizontalGroup("Attribute", 70), HideLabel]
    [field: SerializeField]
    public Sprite BlockIcon { get; private set; }

    // [field: VerticalGroup("Attribute/Type")]
    // [field: SerializeField]
    // public string BlockId { get; set; }

    [field: VerticalGroup("Attribute/Type")]
    [field: SerializeField]
    public EBlockType BlockType { get; private set; }

    [field: VerticalGroup("Attribute/Type")]
    [field: SerializeField, BigNumberEditor]
    public BigNumber BaseValue { get; private set; }

    [field: VerticalGroup("Attribute/Type")]
    [field: SerializeField]
    public string Description { get; private set; }

    [field: VerticalGroup("Attribute/Type")]
    [field: SerializeField]
    public bool IsShowText { get; private set; }

    public BigNumber GetGoldScaleOnLevel(int level)
    {
        return level;
    }

    public BigNumber GetValue()
    {
        return BlockType switch
        {
            EBlockType.Gold => BaseValue * GetGoldScaleOnLevel(1),
            _ => BaseValue
        };
    }

    public string GetValueText()
    {
        return BlockType switch
        {
            EBlockType.Key => $"x{GetValue().ToString()}",
            _ => GetValue().ToString()
        };
    }
}