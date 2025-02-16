using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStickmanLineUpSlot:MonoBehaviour {
    public UIStickman m_UIStickman;
    public StickmanInfo m_StickmanInfo;
    public GameObject m_FreeImage;
    public GameObject m_LockImage;

    public UnityAction<int> m_OnSelectCallback;
    public UnityAction<int> m_OnRemoveCallback;

    public Button m_ButtonRemove;
    public Button m_ButtonSelect;
    private void Awake() {
       // m_ButtonRemove.onClick.AddListener(OnRemove);
        //m_ButtonSelect.onClick.AddListener(OnSelect);
    }
    public void SetUp(StickmanSlotData data,UnityAction<StickmanInfo> selectCallback = null,UnityAction<StickmanInfo> RemoveCallback = null) {
        bool islock = data.u == 0;
        bool isFree = !islock && data.smi == -1;

        SetLock(islock);
        SetFreeSlot(isFree);
        if(!islock && !isFree) {
            m_UIStickman.gameObject.SetActive(true);
            m_StickmanInfo = StickManManager.Ins.GetStickmanInforByID(data.smi);
            m_UIStickman.Setup(m_StickmanInfo,selectCallback,null,RemoveCallback);
        } else {
            m_UIStickman.gameObject.SetActive(false);
        }
    }
    public void SetFreeSlot(bool isFree) {
        m_FreeImage.SetActive(isFree);
    }
    public void SetLock(bool isLock) {
        m_LockImage.SetActive(isLock);
    }
    public void OnSelect() {
    }
    public void OnRemove() {
    }
}
