using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BottonTabItem:MonoBehaviour {
    public int id;
    public RectTransform m_Target;
    //public ScrollSnap scroll;
    public Image m_ImageIcon;
    public Image m_ImageButton;
    public GameObject m_PanelLock;
    public Button m_ButtonSelect;
    public Button m_ButtonDeselect;
    public Sprite[] m_ButtonSprites;
    public Sprite[] m_ButtonBGSprites;
    public LayoutElement m_LayoutElement;
    public UIMainTab m_UIMainTab;
    private RectTransform m_RectTransform;
    private static float m_OnSelectedX;
    private static float m_OnDeselectedX;
    private Vector2 m_VectorSelect;
    private Vector2 m_VectorDeselect;
    private UnityAction<Vector2> m_OnDeselectOther;
    public bool m_OnSelected;
    public UnityAction<int,bool> m_ActionOpenUI;
    private void Awake() {
        m_RectTransform = GetComponent<RectTransform>();
    }
    public void SetupScale(UnityAction<Vector2> method) {
        SetupUI();
        m_ButtonSelect.onClick.AddListener(OnClick);
        //ClickEventManager.SetOnClick(btnSelect, OnSelect);
        m_OnDeselectOther = method;
    }
    public void SetupUIC(UnityAction<int,bool> action)
    {
        m_ActionOpenUI = action;
    }
    private void SetupUI() {
        float x = m_Target.rect.width / 5;
        float defaultX = 192;
        if(x > defaultX) {
            x = defaultX;
        }
        m_OnSelectedX = x * 1.25f;
        m_OnDeselectedX = (x * 5 - x * 1.25f) / 4;
        m_VectorSelect = new Vector2(m_OnSelectedX,182.8f);
        m_VectorDeselect = new Vector2(m_OnDeselectedX,182.8f);
    }
    public void OnClick() {
        m_OnSelected = !m_OnSelected;
        if(m_OnSelected) {
            OnSelect();
        } else {
            Deselect(m_VectorSelect);
        }
    }
    public void OnSelect() {
        m_OnDeselectOther(m_VectorSelect);
        m_ActionOpenUI.Invoke(id, true);
        ScaleSelf();
        //scroll.SetToPage(id);
        //m_UIMainTab.gameObject.SetActive(true);
        //m_UIMainTab.Setup();
        m_ImageIcon.sprite = m_ButtonSprites[1];
        m_ImageIcon.SetNativeSize();
        m_ImageButton.sprite = m_ButtonBGSprites[1];
        m_LayoutElement.flexibleWidth = 2;
        m_OnSelected = true;
        
    }
    private void ScaleSelf() {
        m_RectTransform.DOSizeDelta(m_VectorSelect,0.25f);
        m_ImageIcon.transform.DOScale(1.25f,0.25f);
        m_ImageIcon.transform.DOLocalMoveY(75f,0.25f);
    }
    public void Deselect(Vector2 scale) {
        //m_UIMainTab.gameObject.SetActive(false);
        m_ActionOpenUI.Invoke(id, false);
        m_RectTransform.DOSizeDelta(scale,0.25f);
        m_ImageIcon.transform.DOScale(1f,0.25f);
        m_ImageIcon.transform.DOLocalMoveY(0f,0.25f);
        m_ImageIcon.sprite = m_ButtonSprites[0];
        m_ImageIcon.SetNativeSize();
        m_ImageButton.sprite = m_ButtonBGSprites[0];
        m_LayoutElement.flexibleWidth = 1;
        m_OnSelected = false;
    }
    public void SetLock(bool isLock) {
        m_PanelLock.SetActive(isLock);
        m_ButtonSelect.interactable = !isLock;
    }
}
