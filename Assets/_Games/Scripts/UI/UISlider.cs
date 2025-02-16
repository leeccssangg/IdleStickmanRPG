using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider:MonoBehaviour {
    public Slider m_ProgressBar;
    public TextMeshProUGUI m_TextPregress;
    private float m_MaxProgress;
    private float m_CurrentProgress;
    private float m_TargetProgress;


    private void Update() {
        if(m_CurrentProgress < m_TargetProgress) {
            m_CurrentProgress += 2* Time.deltaTime;
        } else
            m_CurrentProgress = m_TargetProgress;

        UpdateProgressBar();
    }
    public void SetProgress(float p) {
        m_TargetProgress = p;
    }
    public void SetTextProgress(string text) {
        if(m_TextPregress) {
            m_TextPregress.text = text;
        }
    }
    private void UpdateProgressBar() {
        m_ProgressBar.value = m_CurrentProgress;
    }
}
