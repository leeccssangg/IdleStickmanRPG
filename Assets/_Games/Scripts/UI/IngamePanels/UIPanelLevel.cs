using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelLevel:MonoBehaviour {
    public Text m_TextLevel;
    public UIPanelWave m_PanelWave;
    public GameObject m_BossActive;

    public Button m_ButtonBossFight;
    private void OnEnable(){
        Setup(ProfileManager.PlayerData.LevelData);
    }
    public void Setup(InGameLevel inGameLevel) {
        string diff = GameManager.Ins.GetTextDifficulty(inGameLevel.d);
        string levelText = $"{diff} {inGameLevel.l}-{inGameLevel.s}";
        m_TextLevel.text = levelText;
        m_PanelWave.Setup(inGameLevel.w);
        m_ButtonBossFight.interactable = (inGameLevel.IsBossWaitting());
        m_BossActive.SetActive(inGameLevel.IsBossWaitting());
    }
    public void OnBossFight() {
        IngameManager.Ins.Campaign.OnBossFight();
        m_BossActive.SetActive(false);
    }
    public void SetActive(bool isActive){
        gameObject.SetActive(isActive);
    }
}
