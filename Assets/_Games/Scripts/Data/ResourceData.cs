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

[System.Serializable]
public class ResourceData {
    [GUIColor("@MyExtension.EditorExtension.GetColor(\"Resource\", (int)$value)")]
    public enum ResourceType
    {
        NONE, 
        GOLD, 
        GEM,
        ARTIFACT_STONE,
        ITEM,
        MINE_STONE,
    }
    [field: HideLabel, HorizontalGroup("Type", 100), OnValueChanged("UpdateValueResolve")]
    [field: SerializeField]
    public ResourceType Type { get; set; }
    [field: HideLabel, HorizontalGroup("Type/Value"), SuffixLabel("@GetSuffixLabel()", true), OnValueChanged("UpdateValueResolve"), BigNumberEditor]
    [field: SerializeField]
    public BigNumber Value { get; set; }
    public string ActionCallbackName { get; set; }
    //[HorizontalGroup("Type/Resolve"), ReadOnly]
    [HideInInspector]
    public string vr;

    private readonly UnityEvent m_OnValueChangeCallback = new();

    private string GetSuffixLabel()
    {
        return "";
    }

    public void Add(BigNumber value)
    {
        this.Value += value;
        this.UpdateValueResolve();
        this.m_OnValueChangeCallback.Invoke();
    }
    public void Consume(BigNumber value)
    {
        this.Value -= value;
        this.UpdateValueResolve();
        this.m_OnValueChangeCallback.Invoke();
    }
    public bool IsEnough(BigNumber value)
    {
        float r1 = (value.ToFloat() / this.Value.ToFloat());
        return r1 <= 1;
    }
    public void AddOnValueChangeCallback(UnityAction action)
    {
        m_OnValueChangeCallback.AddListener(action);
    }
    private void UpdateValueResolve()
    {
        vr = $"{Type}::{Value}::{ActionCallbackName}";
    }

    public void LoadDataFromValueResolve()
    {
        string[] s = vr.Split("::");
        Type = Enum.Parse<ResourceType>(s[0]);
        Value = new BigNumber(s[1]);

        if (string.IsNullOrEmpty(s[2])) return;
        ActionCallbackName = s[2];
        AddOnValueChangeCallback(() => EventManager.TriggerEvent(ActionCallbackName));
    }

    public ResourceData()
    {
        UpdateValueResolve();
    }
    public ResourceData(ResourceType resourceType, BigNumber value, string actionCallbackName)
    {
        Type = resourceType;
        Value = value;
        ActionCallbackName = actionCallbackName;
        UpdateValueResolve();

        if (string.IsNullOrEmpty(ActionCallbackName)) return;
        AddOnValueChangeCallback(() => EventManager.TriggerEvent(ActionCallbackName));
    }
}
[System.Serializable]
public class ResourceWallet
{
    [ValueDropdown("CustomAddResourcesButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Resources")]
    [ListDrawerSettings(Expanded = true)]
    [LabelText("ResourceList")]
    public List<ResourceData> rl;

    private List<ResourceData> ResourceList { get => rl; set => rl = value; }
    public int Count => ResourceList.Count;

    public ResourceData this[int i]
    {
        get => ResourceList[i];
        set => ResourceList[i] = value;
    }

    public ResourceWallet()
    {
        ResourceList = new List<ResourceData>();
    }

    public ResourceWallet(List<ResourceData> resourceList)
    {
        ResourceList = resourceList;
    }

    public ResourceData GetResource(ResourceData.ResourceType resourceType, BigNumber defaultValue = default,string actionCallbackName = "")
    {
        if (ResourceList.All(x => x.Type != resourceType))
        {
            ResourceList.Add(new ResourceData(resourceType, defaultValue, actionCallbackName));
        }
        return ResourceList.FirstOrDefault(x => x.Type == resourceType);
    }
    public BigNumber GetResourceValue(ResourceData.ResourceType resourceType)
    {
        return GetResource(resourceType).Value;
    }
    public void AddGameResource(ResourceData.ResourceType resourceType, BigNumber value)
    {
        GetResource(resourceType).Add(value);
    }
    public void AddGameResource(ResourceData gameResource)
    {
        GetResource(gameResource.Type).Add(gameResource.Value);
    }
    public void ConsumeGameResource(ResourceData.ResourceType resourceType, BigNumber value)
    {
        GetResource(resourceType).Consume(value);
    }
    public void ConsumeGameResource(ResourceData gameResource)
    {
        GetResource(gameResource.Type).Consume(gameResource.Value);
    }
    public bool IsEnoughGameResource(ResourceData.ResourceType resourceType, BigNumber value)
    {
        return GetResource(resourceType).IsEnough(value);
    }
    public bool IsEnoughGameResource(ResourceData gameResource)
    {
        return GetResource(gameResource.Type).IsEnough(gameResource.Value);
    }
    public void AddOnValueChangeCallBack()
    {
        for(int i = 0;i< ResourceList.Count; i++)
        {
            UnityAction action = () => { EventManager.TriggerEvent("Update" + ResourceList[i].Type.ToString()); ; };
            ResourceList[i].AddOnValueChangeCallback(action) ;
        }
    }
#if UNITY_EDITOR
    private IEnumerable CustomAddResourcesButton()
    {
        return Enum.GetValues(typeof(ResourceData.ResourceType)).Cast<ResourceData.ResourceType>()
            .Except(ResourceList.Select(x => x.Type))
            .Select(x => new ResourceData() { Type = x })
            .AppendWith(ResourceList)
            .Select(x => new ValueDropdownItem(x.Type.ToString(), x));
    }
#endif
}


