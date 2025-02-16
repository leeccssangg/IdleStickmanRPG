using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIDailyGiftInfo : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_CollectableFx;
    [SerializeField] private Image m_ImgIcon;
    [SerializeField] private Image m_ImgReadyCollect;
    [SerializeField] private Image m_ImgTickCollected;
    [SerializeField] private Image m_ImgLock;
    [SerializeField] private TextMeshProUGUI m_TextAmount;
    [SerializeField] private TextMeshProUGUI m_TextDayNum;
    [SerializeField] private DailyGift m_Gift;
    [SerializeField] private Button m_BtnClaim;

    private void Awake()
    {
        m_BtnClaim.onClick.AddListener(OnClaim);
    }
    public void Setup(DailyGift gift)
    {
        m_Gift = gift;
        //switch (m_Gift.gift.giftType)
        //{
        //    case GiftType.RESOURCE:
        //        UpdateProcessGift(gift.resourceReward);
        //        break;
        //    case GiftType.KEY:
        //        UpdateProcessGift(gift.keyReward);
        //        break;
        //    default:
        //        break;
        //}
        m_TextDayNum.text = "Day " + (m_Gift.day +1).ToString();
        m_TextAmount.text = m_Gift.gift.amount.ToString();
    }
    private void UpdateProcessGift(ResourceRewardPackage reward)
    {
        //m_TextAmountCurrent.text = reward.amount.ToString();
    }
    private void UpdateProcessGift(KeyWallet reward)
    {
        //m_TextAmountCurrent.text = reward.amount.ToString();
    }
    public void SetEffect(int currentDay)
    {
        SetImgEffect(currentDay);
        SetBtnClaim(currentDay);
    }
    private void SetImgEffect(int currentDay)
    {
        m_ImgTickCollected.gameObject.SetActive(m_Gift.day < (currentDay % 7));
        m_ImgLock.gameObject.SetActive(m_Gift.day > (currentDay % 7) || 
            ((currentDay % 7) == m_Gift.day && !ProfileManager.Ins.m_DailyGiftManager.IsGoodToClaim()));
        m_ImgReadyCollect.gameObject.SetActive((currentDay % 7) == m_Gift.day && ProfileManager.Ins.m_DailyGiftManager.IsGoodToClaim());
        //if ((currentDay % 7) == m_Gift.day)
        //{
        //    m_ImgTickCollected.gameObject.SetActive(!ProfileManager.Ins.m_DailyGiftManager.IsGoodToClaim());
        //    //m_CollectableFx.Play();
        //}
    }
    private void SetBtnClaim(int currentDay)
    {
        m_BtnClaim.interactable = (((currentDay % 7) == m_Gift.day) && ProfileManager.Ins.m_DailyGiftManager.IsGoodToClaim());
    }
    public void OnClaim()
    {
        m_ImgTickCollected.gameObject.SetActive(true);
        ProfileManager.Ins.ClaimDailyGift();
        m_BtnClaim.interactable = false;
        m_ImgTickCollected.gameObject.SetActive(true);
        m_ImgReadyCollect.gameObject.SetActive(false);
        //m_CollectableFx.gameObject.SetActive(false);
    }
    public void OnReadyClaim()
    {
        //m_CollectableFx.Play();
    }
}
