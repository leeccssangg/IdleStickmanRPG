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

public enum ItemType
{
    NONE,
    GATCHA,
    RESOURCE,
}
public class UIItemBase : MonoBehaviour
{
    [SerializeField] protected ItemType m_ItemType;
    public Image m_IconType;
    public Image m_IconItem;
    public Image m_ImgBg;
    public GameObject m_EffectGlow;
    public TextMeshProUGUI m_TxtAmount;


    public virtual void InitData()
    {

    }
    public virtual void InitData(GatchaConfigInfo gatchaConfig)
    {
        
    }
    public virtual void InitData(ResourceData resourceData)
    {
        
    }
}
