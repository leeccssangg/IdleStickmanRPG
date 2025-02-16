using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIItemResource : UIItemBase
{
    // [SerializeField] private ItemType m_ItemType = ItemType.RESOURCE;
    [SerializeField] private ResourceData m_ResourceData;

    public override void InitData(ResourceData resourceData)
    {
        base.InitData();
        m_ItemType = ItemType.RESOURCE;
        m_ResourceData = resourceData;
        m_IconType.gameObject.SetActive(false);
        m_TxtAmount.gameObject.SetActive(true);
        m_TxtAmount.text = m_ResourceData.Value.ToString();
        m_IconItem.sprite = SpriteManager.Ins.GetResourceIconSprite(m_ResourceData.Type);
        m_ImgBg.sprite = SpriteManager.Ins.GetResourceBackgroundSprite(m_ResourceData.Type);
    }
}
