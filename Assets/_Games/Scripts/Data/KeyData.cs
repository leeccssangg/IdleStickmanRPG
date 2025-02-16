using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyExtension;
using Sirenix.Utilities;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine.Events;
using UnityEngine.Serialization;

[GUIColor("@MyExtension.EditorExtension.GetColor(\"KeyDungeon\", (int)$value)")]
public enum KeyType
{
    NONE, FREE, ADS, BONUS,
}
[System.Serializable]
public class KeyData
{
    [LabelText("KeyType")]
    [SerializeField]
    public KeyType kt;
    [LabelText("Value")]
    [SerializeField]
    public int vl;
    [LabelText("ActionString")]
    [SerializeField]
    public string acs;


    public KeyType KeyType { get => kt; set => kt = value; }
    public int Value { get => vl; set=> vl= value;}
    public string ActionString { get => acs; set => acs = value; }

    private readonly UnityEvent m_OnValueChangeCallback = new();

    public void AddKey(int value)
    {
        this.Value += value;
        this.m_OnValueChangeCallback.Invoke();
    }
    public void ConsumeKey(int value)
    {
        this.Value -= value;
        this.m_OnValueChangeCallback.Invoke();
    }
    public bool IsEnoughKey(int value)
    {
        return value <= this.Value;
    }
    public void AddOnValueChangeCallback(UnityAction action)
    {
        m_OnValueChangeCallback.AddListener(action);
    }
    public KeyData()
    {
    
    }
    public KeyData(KeyType keyType, int value,string actionString)
    {
        KeyType = keyType;
        Value = value;
        ActionString = actionString;
        if (String.IsNullOrEmpty(actionString))
            return;
        AddOnValueChangeCallback(() => { EventManager.TriggerEvent(ActionString); });
    }

}
[System.Serializable]
public class DungeonKeyData
{
    [LabelText("DungeonType")]
    [SerializeField]
    public DungeonType dt;
    [LabelText("KeyList")]
    [SerializeField]
    public List<KeyData> kl;
    public DungeonType DungeonType { get => dt; set => dt = value; }
    public List<KeyData> KeyList { get => kl; set => kl = value; }
    public int Count => KeyList.Count;


    public KeyData this[int i]
    {
        get => KeyList[i];
        set => KeyList[i] = value;
    }
    public DungeonKeyData()
    {
        KeyList = new List<KeyData>();
    }

    public DungeonKeyData(List<KeyData> keyList)
    {
        KeyList = keyList;
    }
    public DungeonKeyData(DungeonType dungeonType, List<KeyData> keyList)
    {
        DungeonType = dungeonType;
        KeyList = keyList;
    }
    public KeyData GetKey(KeyType keyType, int defaultValue = default, string actionString ="")
    {
        if (KeyList.All(x => x.KeyType != keyType))
        {
            KeyList.Add(new KeyData(keyType, defaultValue, actionString));
        }
        return KeyList.FirstOrDefault(x => x.KeyType == keyType);
    }
    public int GetKeyValue(KeyType keyType)
    {
        return GetKey(keyType).Value;
    }
    public void AddKey(KeyType keyType, int value)
    {
        GetKey(keyType).AddKey(value);
    }
    public void AddKey(KeyData key)
    {
        GetKey(key.KeyType).AddKey(key.Value);
    }
    public void ConsumeKey(KeyType keyType, int value)
    {
        GetKey(keyType).ConsumeKey(value);
    }
    public void ConsumeKey(KeyData key)
    {
        GetKey(key.KeyType).ConsumeKey(key.Value);
    }
    public bool IsEnoughKey(KeyType keyType, int value)
    {
        return GetKey(keyType).IsEnoughKey(value);
    }
    public bool IsEnoughKey(KeyData key)
    {
        return GetKey(key.KeyType).IsEnoughKey(key.Value);
    }
    //#if UNITY_EDITOR
    //    private IEnumerable CustomAddResourcesButton()
    //    {
    //        return Enum.GetValues(typeof(DungeonType)).Cast<DungeonType>()
    //            .Except(KeyList.Select(x => x.KeyType))
    //            .Select(x => new KeyData() { KeyType = x })
    //            .AppendWith(KeyList)
    //            .Select(x => new ValueDropdownItem(x.KeyType.ToString(), x));
    //    }
    //#endif

}
[System.Serializable]
public class KeyWallet
{
    [ValueDropdown("CustomAddResourcesButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Resources")]
    [ListDrawerSettings(Expanded = true)]
    [LabelText("DungeonKeyList")]
    public List<DungeonKeyData> dkl;

    private List<DungeonKeyData> DungeonKeyList { get => dkl; set => dkl = value; }
    public int Count => DungeonKeyList.Count;
    public DungeonKeyData this[int i]
    {
        get => DungeonKeyList[i];
        set => DungeonKeyList[i] = value;
    }

    public KeyWallet()
    {
        DungeonKeyList = new List<DungeonKeyData>();
    }

    public KeyWallet(List<DungeonKeyData> keyList)
    {
        DungeonKeyList = keyList;
    }
    public DungeonKeyData GetDungeonKey(DungeonType dungeonType)
    {
        if (DungeonKeyList.All(x => x.DungeonType != dungeonType))
        {
            DungeonKeyList.Add(new DungeonKeyData()
            {
                DungeonType = dungeonType,
                KeyList = new(),
            });
        }
        return DungeonKeyList.FirstOrDefault(x => x.DungeonType == dungeonType);
    }
    public int GetKeyValue(DungeonType dungeonType, KeyType keyType)
    {
        return GetDungeonKey(dungeonType).GetKeyValue(keyType);
    }
    public void AddKey(DungeonType dungeonType,KeyType keyType, int value)
    {
        GetDungeonKey(dungeonType).GetKey(keyType).AddKey(value);
    }
    public void AddKey(DungeonKeyData dungeonKeyData,KeyData keyData)
    {
        GetDungeonKey(dungeonKeyData.DungeonType).GetKey(keyData.KeyType).AddKey(keyData.Value);
    }
    public void ConsumeKey(DungeonType dungeonType, KeyType keyType, int value)
    {
        GetDungeonKey(dungeonType).GetKey(keyType).ConsumeKey(value);
    }
    public void ConsumeKey(DungeonKeyData dungeonKeyData, KeyData keyData)
    {
        GetDungeonKey(dungeonKeyData.DungeonType).GetKey(keyData.KeyType).ConsumeKey(keyData.Value);
    }
    public bool IsEnoughKey(DungeonType dungeonType, KeyType keyType, int value)
    {
        return GetDungeonKey(dungeonType).GetKey(keyType).IsEnoughKey(value);
    }
    public bool IsEnoughKey(DungeonKeyData dungeonKeyData, KeyData keyData)
    {
        return GetDungeonKey(dungeonKeyData.DungeonType).GetKey(keyData.KeyType).IsEnoughKey(keyData.Value);
    }
    public void AddAllKeyValueChangeCallback()
    {
        for(int i = 0;i< DungeonKeyList.Count; i++)
        {
            DungeonKeyData curDungeon = DungeonKeyList[i];
            for (int j = 0; j < curDungeon.Count; j++)
            {
                KeyData curKey = curDungeon[j];
                if (String.IsNullOrEmpty(curKey.ActionString))
                    return;
                curKey.AddOnValueChangeCallback(
                    () => { EventManager.TriggerEvent(curKey.ActionString); });
            }
        }
    }
#if UNITY_EDITOR
    private IEnumerable CustomAddResourcesButton()
    {
        return Enum.GetValues(typeof(DungeonType)).Cast<DungeonType>()
            .Except(DungeonKeyList.Select(x => x.DungeonType))
            .Select(x => new DungeonKeyData { DungeonType = x })
            .AppendWith(DungeonKeyList)
            .Select(x => new ValueDropdownItem(x.DungeonType.ToString(), x));
    }
#endif
}