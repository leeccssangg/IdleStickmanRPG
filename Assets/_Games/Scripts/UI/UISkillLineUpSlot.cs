using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkillLineUpSlot:MonoBehaviour {
    public UISkilll m_UISkill;
    public SkillInfo m_SkillInfo;
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
    public void SetUp(SkillSlotData data,UnityAction<SkillInfo> selectCallback = null,UnityAction<SkillInfo> RemoveCallback = null) {
        bool islock = data.u == 0;
        bool isFree = !islock && data.si == -1;

        SetLock(islock);
        SetFreeSlot(isFree);
        if(!islock && !isFree) {
            m_UISkill.gameObject.SetActive(true);
            m_SkillInfo = SkillManager.Ins.GetSkillInfoByID(data.si);
            m_UISkill.Setup(m_SkillInfo,selectCallback,null,RemoveCallback);
        } else {
            m_UISkill.gameObject.SetActive(false);
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
