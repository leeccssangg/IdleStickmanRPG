using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonTab : MonoBehaviour
{
    public int m_Id;
    public Button m_Button;
    protected UnityAction<int> m_OnSelectCallback;
    public void Awake() {
        m_Button.onClick.AddListener(OnSelect);
    }
    public virtual void Setup(int id, UnityAction<int> onSelectCallback) {
        m_Id = id;  
        m_OnSelectCallback = onSelectCallback;
    }
    public virtual void Active(bool isActive) {
        m_Button.interactable = isActive;
    }
    public virtual void OnSelect() {
        m_OnSelectCallback?.Invoke(m_Id);
    }
}
