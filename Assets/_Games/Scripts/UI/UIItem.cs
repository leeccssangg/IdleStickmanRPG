using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public Image m_Icon;
    public Image m_RarityBorder;
    public Image m_RarityBackground;
    
    public UISlider m_PieceProgress;
    
    public TextMeshProUGUI m_TextLevel;

    public GameObject m_PanelLock;
    public GameObject m_PanelEquipped;
    public GameObject m_ArrowUpgrade;
    
    public GameObject m_Notification;
    
    public void UpdateImageIcon(Sprite sprite)
    {
        if(m_Icon) m_Icon.sprite = sprite;
    }
    public void UpdateImageRarity(Rarity rarity)
    {
        //if(m_RarityBorder) m_RarityBorder.sprite = SpriteManager.Ins.GetItemRaritySprite(rarity).border;
        if(m_RarityBackground) m_RarityBackground.sprite = SpriteManager.Ins.GetItemRaritySprite(rarity).background;
    }
    public void UpdateTextLevel(int level){
        if(m_TextLevel) m_TextLevel.text = $"LV.{level}";
    }
    public void UpdatePanelLock(bool isLock)
    {
        if(m_PanelLock) m_PanelLock.SetActive(isLock);
    }
    public void UpdatePanelEquipped(bool isEquipped)
    {
        if(m_PanelEquipped) m_PanelEquipped.SetActive(isEquipped);
    }
    public void UpdateArrowUpgrade(bool isUpgrade)
    {
        if(m_ArrowUpgrade) m_ArrowUpgrade.SetActive(isUpgrade);
    }
    public void UpdateNotification(bool isEnableNotification)
    {
        if(m_Notification) m_Notification.SetActive(isEnableNotification);
    }
    public void UpdateUIPiece(float currentPiece, string pieceText){
         if(!m_PieceProgress || !m_PieceProgress.gameObject.activeSelf) return;
         m_PieceProgress.SetProgress(currentPiece);
         m_PieceProgress.SetTextProgress(pieceText);
    }
}
