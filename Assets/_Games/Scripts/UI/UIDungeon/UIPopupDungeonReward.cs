using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class UIPopupDungeonReward : UICanvas
{
    //TEXT
    public TextMeshProUGUI m_TextTitle;
    public TextMeshProUGUI m_TextReward;
    public TextMeshProUGUI m_TextRewardValue;
    
    //Button
    public UIButton m_ButtonClaim;
    private void Awake(){
        SetupText();
        m_ButtonClaim.onClick.AddListener(OnClaim);
    }
    private void OnEnable(){
        Setup();    
    }
    public override void Setup(){
        m_Content.DOScale(1, 0.25f).SetEase(Ease.OutBack).From(0);
    }
    private void SetupText(){
        var s = $"Received a reward for clearing a dungeon!";
        m_TextReward.text = s;
        var s2 = $"CHALLENGE REWARD".ToUpper();
        m_TextTitle.text = s2;
    }

    public void OnClaim(){
        UIManager.Ins.PlayEarnResourceEffect(ResourceData.ResourceType.GEM, m_ButtonClaim.transform.position);
        Close();
    }
    public override void Close(float delayTime = 0){
        if(m_Content){
            m_Content.DOScale(0,0.15f).SetEase(Ease.InBack).From(1).OnComplete(() => {
                base.Close(delayTime);
            });
        }
    }
}
