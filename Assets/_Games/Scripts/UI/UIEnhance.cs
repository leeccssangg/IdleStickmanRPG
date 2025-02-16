using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnhance : MonoBehaviour
{
    [SerializeField] private Transform m_Content;
    [SerializeField] private Transform m_LevelTrans;
    [SerializeField] private Image m_ItemIcon;
    [SerializeField] private Image m_ImageRarity;
    [SerializeField] private TextMeshProUGUI m_TextOldLevel;
    [SerializeField] private TextMeshProUGUI m_TextNewLevel;
    
    public void Setup(ItemInfoEnhance info){
        m_Content.gameObject.SetActive(false);
        m_Content.DOKill();
        m_LevelTrans.DOKill();
        m_LevelTrans.localScale = Vector3.zero;
        m_Content.localScale = Vector3.one * 1.5f;
        
        m_ItemIcon.sprite = info.itemIcon;
        Debug.Log(info.rarity);
        m_ImageRarity.sprite = SpriteManager.Ins.GetGachaRaritySprite(info.rarity);
        m_TextOldLevel.text = info.oldLevel.ToString();
        m_TextNewLevel.text = info.newLevel.ToString();
    }
    public void Show(){
        m_Content.gameObject.SetActive(true);
        m_Content.DOScale(1, 0.15f).SetEase(Ease.OutBack);
        m_LevelTrans.DOScale(1,0.15f).SetEase(Ease.OutBack).SetDelay(0.25f);
    }
}
