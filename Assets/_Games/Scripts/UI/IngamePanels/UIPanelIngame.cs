using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIPanelIngame: UICanvas {
    [SerializeField] private CanvasGroup m_BlackPanel;
    
    [SerializeField] private UIPanelLevel m_PanelLevel;
    [SerializeField] private UIPanelBossRush m_PanelLevelBossRush;
    [FormerlySerializedAs("M_PanelContent")] [FormerlySerializedAs("n_PanelContent")] [SerializeField] private UIPanelTopButton m_PanelContent;
    
    [SerializeField] private PanelVSBoss m_PanelVSBoss;
    [SerializeField] private UIPanelGold m_PanelGold;
    [SerializeField] private UIPanelGem m_PanelGem;
    [SerializeField] private UIPanelPower m_PanelPower;
    
    [SerializeField] private GameObject m_WinImage;
    public UIPanelGold PanelGold{ get => m_PanelGold; set => m_PanelGold = value; }
    public UIPanelGem PanelGem{ get => m_PanelGem; set => m_PanelGem = value; }

    public UIPanelPower PanelPower => m_PanelPower;

    private void OnEnable() {
        m_BlackPanel.alpha = 1;
        m_PanelVSBoss.gameObject.SetActive(false);
        m_WinImage.SetActive(false);
    }
    public void SetupStartBattle(GameMode gameMode){
        switch(gameMode){
            case GameMode.NONE:
                break;
            case GameMode.CAMPAIGN:
                m_PanelLevel.SetActive(true);
                m_PanelLevelBossRush.SetActive(false);
                m_PanelContent.gameObject.SetActive(true);
                break;
            case GameMode.BOSS_RUSH:
                m_PanelLevel.SetActive(false);
                m_PanelContent.gameObject.SetActive(false);
                m_PanelLevelBossRush.Setup(IngameManager.Ins.CurrentGamePlayMode as BossRushMode);
                break;
            case GameMode.GOLD_RUSH:
                break;
            case GameMode.MONSTER_NEST:
                break;
            case GameMode.CHALLENGE_KING:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
        }
    }
    public void SetupUILevel(){
        var campaignLevel = ProfileManager.PlayerData.LevelData;
        m_PanelLevel.Setup(campaignLevel);
    }
    public void DarkPanelEffect(float time){
        m_BlackPanel.DOKill();
        const float fadeOutTime = 0.5f;
        const float fadeInTime = 0.5f;
        var delayTime = time - fadeOutTime - fadeInTime;
        m_BlackPanel.DOFade(1,fadeInTime).OnComplete(() => {
            m_BlackPanel.DOFade(0,fadeOutTime).SetDelay(time);
        });
    }
    public void ShowBossPanel(){
        m_PanelVSBoss.Setup();
    }
    
    public void ShowWinImage(bool isShow){
        m_WinImage.SetActive(isShow);
        DOVirtual.DelayedCall(4,()=>{m_WinImage.SetActive(false);} );
    }
}
