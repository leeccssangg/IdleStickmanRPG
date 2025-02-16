using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UICDailyGift : UICanvas
{
    [SerializeField] private CanvasGroup m_ContentCanvasGroup;
    [SerializeField] private RectTransform m_ContentRectTransform;
    [SerializeField] private Image m_ImgBg;
    [SerializeField] private Button m_BtnClose;
    [SerializeField] private List<UIDailyGiftInfo> m_ListUIDailyGiftInfo;
    [SerializeField] private List<DailyGift> m_ListGiftsConfigs;
    [SerializeField] private int m_CurrentDayID;
    [SerializeField] private DailyGift m_CurrentDayGift;
    [SerializeField] private Image m_ImgProcess;
    [SerializeField] private TextMeshProUGUI m_TextProcess;
    void Awake()
    {
        InitData();
        m_BtnClose.onClick.AddListener(OnClose);
    }
    private void OnEnable()
    {
        //m_ImgBg.DOFade(0.3f, 0.25f);
        //m_ContentCanvasGroup.DOFade(1, 0.25f);
        //m_ContentRectTransform.DOAnchorPosY(160f, 0.5f);
        m_ContentRectTransform.DOScale(0, 0);
        m_ImgBg.DOFade(1f, 0.25f);
        m_ContentCanvasGroup.DOFade(1, 0.15f).OnComplete(() => {
            m_ContentRectTransform.DOScale(1.05f, 0.15f).OnComplete(() => {
                m_ContentRectTransform.DOScale(1, 0.15f).OnComplete(() => {
                });
            });
        });
    }
    private void OnClose()
    {
        m_ImgBg.DOFade(0f, 0.25f);
        m_ContentRectTransform.DOScale(0, 0.25f).OnComplete(() =>
        {
            m_ContentCanvasGroup.DOFade(0, 0.25f);
            //UIManager.Instance.OpenUI<UICGamePlay>();
            UIManager.Ins.CloseUI<UICDailyGift>();
        });
    }
    public void InitData()
    {
        m_ListGiftsConfigs = ProfileManager.Ins.GetListGiftConfigs();
        m_CurrentDayID = ProfileManager.Ins.GetMyCurrentDayID() % 7;
        m_CurrentDayGift = m_ListGiftsConfigs[m_CurrentDayID];
        for (int i = 0; i< m_ListUIDailyGiftInfo.Count; i++)
        {
            DailyGift gift = m_ListGiftsConfigs[i];
            m_ListUIDailyGiftInfo[i].Setup(gift);
            m_ListUIDailyGiftInfo[i].SetEffect(m_CurrentDayID);
        }
        if(m_CurrentDayID == 5)
        {
            m_TextProcess.text = "5/5";
            m_ImgProcess.fillAmount = 1;
        }
        else
        {
            m_TextProcess.text = (m_CurrentDayID % 5).ToString() + "/5";
            m_ImgProcess.fillAmount = (float)(m_CurrentDayID % 5) / 5;
        }
        
    }
}
