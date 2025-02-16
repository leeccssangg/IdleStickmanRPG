using System;
using UnityEngine.Events;

[Serializable]
public class DataSavingMachine {
    private float m_AutoSavingCooldown = 0;
    public float m_MaxAutoSavingTime = 5;

    public UnityEvent m_OnSaveEvent;
    public void Update(float dt) {
        m_AutoSavingCooldown += dt;
        if (m_AutoSavingCooldown >= m_MaxAutoSavingTime) {
            ForceSave();
        }
    }
    public void Save() {
        if (IsGoodToSave()) {
            m_OnSaveEvent.Invoke();
            Reset();
        }
    }
    public void ForceSave() {
        m_OnSaveEvent.Invoke();
        Reset();
    }
    public void Reset() {
        m_AutoSavingCooldown = 0;
    }
    public bool IsGoodToSave() {
        return m_AutoSavingCooldown >= m_MaxAutoSavingTime;
    }
}