using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelBossRush : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextBossRush;
    [SerializeField] private TextMeshProUGUI m_TextBossRushLevel;
    [SerializeField] private TextMeshProUGUI m_TextBossRushProcess;
    [SerializeField] private TextMeshProUGUI m_TextBossRushTime;

    private BossRushMode m_GameMode;
    private int m_MaxWave;
    private void Awake(){
        string s = $"Boss Rush";
        m_TextBossRush.text = s;
    }
    public void Setup(GameModeBase gameMode){
        m_GameMode = gameMode as BossRushMode;
        SetActive(true);
        UpdateCurrentLevel();
    }
    private void UpdateCurrentLevel(){
        string s = $"{m_GameMode.GetCurLevel()}";
        m_TextBossRushLevel.text = s;
        
        m_MaxWave = m_GameMode.GetMaxProcess();
    }
    private void Update(){
        if(m_GameMode == null) return;
        UpdateUIBossRush();
        UpdateTime();
    }
    private void UpdateUIBossRush(){
        int curWave = m_GameMode.CurrentWave;
        m_TextBossRushProcess.text = $"{curWave}/{m_MaxWave}";
    }
    private void UpdateTime(){
        TimeSpan timeSpan = TimeSpan.FromSeconds(m_GameMode.TimeLeft);
        string s = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        m_TextBossRushTime.text = s;
    }
    public void SetActive(bool isActive){
        gameObject.SetActive(isActive);
    }
}
