using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkillSlider:MonoBehaviour {

    public Slider m_Slider;
    public UnityAction m_EndCallback;

    private float m_CurrentValue;
    private float m_MaxSliderValue;
    private void Update() {
        //if(m_CurrentValue > 0) {
        //    m_CurrentValue -= Time.deltaTime;
        //} else {
        //    m_CurrentValue = 0;
        //    OnFinish();
        //}
    }
    public void SetupSlider(float maxValue) {
        m_MaxSliderValue = maxValue;
        UpdateSlider(m_MaxSliderValue);
    }
    public void UpdateSlider(float value) {
        m_CurrentValue = value;
        float t = m_CurrentValue / m_MaxSliderValue;
        m_Slider.value = t;
        if(m_CurrentValue <= 0) {
            OnFinish();
        }
    }
    public void StartRuning(float time) {
        m_CurrentValue = time;
        m_MaxSliderValue = time;
        SetActive(true);
    }
    public void OnFinish() {
        SetActive(false);
       // m_EndCallback?.Invoke();
    }
    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);
    } 
}
