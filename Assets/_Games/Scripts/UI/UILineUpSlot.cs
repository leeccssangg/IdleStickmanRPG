using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISKillLineUpSlot:MonoBehaviour {

    public Image m_Icon;
    public Image m_RarityImage;
    public Text m_LevelText;

    public GameObject m_PanelLock;
    public GameObject m_PanelFreeSlot;
    public GameObject m_PanelInfo;

    public Button m_ButtonRemove;

    public int m_SlotID;
    private void Awake() {
        m_ButtonRemove.onClick.AddListener(OnRemove);
    }
    public void SetUp(Sprite iconSp,Sprite raritySp,int level) {
        m_Icon.sprite = iconSp;
        m_RarityImage.sprite = raritySp;
        m_LevelText.text = "LV"+level.ToString();
    }

    public void FreeSlot() {
        m_PanelFreeSlot.SetActive(true);
        m_PanelLock.SetActive(false);
        m_PanelInfo.SetActive(false);
    }
    public void Lock() {
        m_PanelLock.SetActive(true);
        m_PanelFreeSlot.SetActive(false);
        m_PanelInfo.SetActive(false);
    }

    public void OnRemove() {

    }
}
