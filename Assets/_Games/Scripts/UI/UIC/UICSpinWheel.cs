using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using EasyUI.PickerWheelUI;

public class UICSpinWheel : UICanvas
{
    private Pextension.MiniPool<PickerWheel> wheelPool = new();
    [SerializeField] private PickerWheel wheelPrefab;
    [SerializeField] private CanvasGroup m_ContentCanvasGroup;
    [SerializeField] private RectTransform m_ContentRectTransform;
    [SerializeField] private Image m_ImgBg;
    [SerializeField] private Image m_ImgProcess;
    [SerializeField] private TextMeshProUGUI m_TextCurrentUpgraded;
    [SerializeField] private TextMeshProUGUI m_TextLevel;
    [SerializeField] private TextMeshProUGUI m_TextCoolDown;
    [SerializeField] private Button m_BtnClose;
    [SerializeField] private Button m_BtnSpinWheelGem;
    [SerializeField] private Button m_BtnSpinWheelFree;
    [SerializeField] private Button m_BtnUpgrade;
    [SerializeField] private SpinWheelConfigs m_SpinWheelConfigs;
    [SerializeField] private PickerWheel m_SpinWheel;
    [SerializeField] private int m_WheelLevel;
    [SerializeField] private int m_CurrentUpgraded;
    [SerializeField] private Transform m_WheelParent;
    [SerializeField] private bool isFree;
    [SerializeField] private bool isSpin;

    void Awake()
    {
        wheelPool.OnInit(wheelPrefab, 5, m_WheelParent);
        m_BtnClose.onClick.AddListener(OnClose);
        m_BtnSpinWheelGem.onClick.AddListener(SpinWheel);
        m_BtnSpinWheelFree.onClick.AddListener(SpinFree);
        m_BtnUpgrade.onClick.AddListener(UpgradeWheel);
        SetupData(SpinWheelManager.Ins.GetSpinWheelLevel(),
            SpinWheelManager.Ins.GetCurrentUpgraded()
            , SpinWheelManager.Ins.GetSpinWheelConfigs());
    }
    public void SetupData(int level, int upgraded, SpinWheelConfigs spinWheelConfigs)
    {
        m_SpinWheelConfigs = spinWheelConfigs;
        m_WheelLevel = level;
        m_CurrentUpgraded = upgraded;
    }
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            if (!isSpin)
            {
                m_BtnSpinWheelFree.interactable = SpinWheelManager.Ins.IsFree();
                isFree = SpinWheelManager.Ins.IsFree();
            }
            if (!isFree)
            {
                m_TextCoolDown.text = SpinWheelManager.Ins.GetTimeToNextDay();
            }

        }
    }
    private void OnEnable()
    {
        if (m_SpinWheel == null)
        {
            SpawnNewWheel(true);
            m_ContentRectTransform.DOScale(0, 0);
        }
        m_ImgBg.DOFade(1f, 0.25f);
        m_ContentCanvasGroup.DOFade(1, 0.15f).OnComplete(() => {
            m_ContentRectTransform.DOScale(1.05f, 0.15f).OnComplete(() => {
                m_ContentRectTransform.DOScale(1, 0.15f).OnComplete(() => {
                });
            });
        });
        //m_ContentRectTransform.DOAnchorPosY(120f, 0.5f);
        SetInteracableButton(m_CurrentUpgraded >= m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_UpgradeRequire);
        CheckButtonSpinGem();
        UpdateProcess();
    }
    private void OnClose()
    {
        m_ImgBg.DOFade(0f, 0.25f);
        m_ContentRectTransform.DOScale(0, 0.25f).OnComplete(() =>
        {
            m_ContentCanvasGroup.DOFade(0, 0.25f);
            //UIManager.Instance.OpenUI<UICGamePlay>();
            UIManager.Ins.CloseUI<UICSpinWheel>();
        });
        //m_ContentRectTransform.DOAnchorPosY(0f, 0.25f).OnComplete(() => {
        //    UIGameplay.Instance.SwitchAdsBonus(false);
        //    UIGame.Ins.OpenUI<UIGameplay>();
        //    UIGame.Ins.CloseUI<UISpinWheel>();
        //});

    }
    private void CheckButtonSpinGem()
    {
        int cost = m_SpinWheel.GetSpinCost();
        m_BtnSpinWheelGem.interactable = ProfileManager.PlayerData.IsEnoughGameResource(ResourceData.ResourceType.GEM, new BigNumber(cost));
    }
    private void SpinWheel()
    {
        ProfileManager.PlayerData.ConsumeGameResource(ResourceData.ResourceType.GEM, m_SpinWheel.GetSpinCost());
        m_SpinWheel.OnSpinStart(() =>
        {
            isSpin = true;
            m_BtnSpinWheelFree.interactable = false;
            m_BtnSpinWheelGem.interactable = false;
            m_BtnClose.interactable = false;
        });
        m_SpinWheel.OnSpinEnd(wheelPiece =>
        {
            if (isFree)
                m_BtnSpinWheelFree.interactable = true;
            CheckButtonSpinGem();
            m_BtnClose.interactable = true;
            m_CurrentUpgraded++;
            if (m_WheelLevel < m_SpinWheelConfigs.SpinWheelDataConfigs.Count - 1)
                SetInteracableButton(m_CurrentUpgraded >= m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_UpgradeRequire);
            UpdateProcess();
            SpinWheelManager.Ins.ClaimReward(wheelPiece.m_WheelDataConfigs, false);
            isSpin = false;
        });
        m_SpinWheel.Spin();
    }
    private void SpinFree()
    {
        m_SpinWheel.OnSpinStart(() =>
        {
            isSpin = true;
            isFree = false;
            m_BtnSpinWheelFree.interactable = false;
            m_BtnSpinWheelGem.interactable = false;
            m_BtnClose.interactable = false;
        });
        m_SpinWheel.OnSpinEnd(wheelPiece =>
        {
            m_BtnSpinWheelFree.interactable = false;
            CheckButtonSpinGem();
            m_BtnClose.interactable = true;
            m_CurrentUpgraded++;
            if (m_WheelLevel < m_SpinWheelConfigs.SpinWheelDataConfigs.Count - 1)
                SetInteracableButton(m_CurrentUpgraded >= m_SpinWheelConfigs.SpinWheelDataConfigs[m_WheelLevel].m_UpgradeRequire);
            UpdateProcess();
            SpinWheelManager.Ins.ClaimReward(wheelPiece.m_WheelDataConfigs, true);
            isSpin = false;
            SpinWheelManager.Ins.SaveData();
        });
        m_SpinWheel.Spin();
    }
    private void UpgradeWheel()
    {
        m_BtnUpgrade.gameObject.SetActive(false);
        m_BtnClose.interactable = false;
        SpinWheelManager.Ins.Upgrade();
        m_CurrentUpgraded = 0;
        m_WheelLevel++;
        PickerWheel oldWheel = m_SpinWheel;
        oldWheel.GetComponent<RectTransform>().DOScale(0f, 0.5f).OnComplete(() =>
        {
            SpawnNewWheel(false);
            SetInteracableButton(false);
            m_BtnClose.interactable = true;
            Destroy(oldWheel);
        });
        UpdateProcess();
        CheckButtonSpinGem();
        SpinWheelManager.Ins.SaveData();

    }
    public void SetWheel(PickerWheel wheel)
    {
        m_SpinWheel = wheel;
    }
    public void SpawnNewWheel(bool isFirstTime)
    {
        PickerWheel newWheel = wheelPool.Spawn(Vector3.zero, Quaternion.identity);
        newWheel.GetComponent<CanvasGroup>().alpha = 0;
        newWheel.GetComponent<RectTransform>().DOScale(0f, 0f);
        Transform transform1 = newWheel.transform;
        transform1.parent = m_WheelParent;
        transform1.localPosition = Vector3.zero;
        SetWheel(newWheel);
        m_SpinWheel.SetupData(m_WheelLevel, m_SpinWheelConfigs);
        m_SpinWheel.StartInGame();
        if (!isFirstTime)
        {
            m_SpinWheel.GetComponent<CanvasGroup>().DOFade(1f, 0.25f);
            m_SpinWheel.GetComponent<RectTransform>().DOScale(1.5f, 0.125f).OnComplete(() => {
                m_SpinWheel.GetComponent<RectTransform>().DOScale(1.2f, 0.125f);
            });
        }
        else
        {
            m_SpinWheel.GetComponent<CanvasGroup>().DOFade(1f, 0);
            m_SpinWheel.GetComponent<RectTransform>().DOScale(1.2f, 0);
        }
    }
    public void SpawnNewWheel(PickerWheel lastPickerWheel)
    {
        PickerWheel newWheel = wheelPool.Spawn(Vector3.zero, Quaternion.identity);
        Transform transform1 = newWheel.transform;
        transform1.parent = m_WheelParent;
        transform1.localPosition = Vector3.zero;
        SetWheel(newWheel);
        m_SpinWheel.SetupData(m_WheelLevel, m_SpinWheelConfigs);
        m_SpinWheel.StartInGame();
    }
    private void SetInteracableButton(bool isTrue)
    {
        m_BtnSpinWheelGem.gameObject.SetActive(!isTrue);
        m_BtnSpinWheelFree.gameObject.SetActive(!isTrue);
        m_BtnUpgrade.gameObject.SetActive(isTrue);
    }
    private void UpdateProcess()
    {
        if (m_WheelLevel < m_SpinWheelConfigs.SpinWheelDataConfigs.Count - 1)
        {
            m_ImgProcess.fillAmount = SpinWheelManager.Ins.GetProcessUpgraded(m_CurrentUpgraded);
            if (SpinWheelManager.Ins.GetProcessUpgraded(m_CurrentUpgraded) < 1)
            {
                m_TextCurrentUpgraded.text = SpinWheelManager.Ins.GetCurrentUpgraded().ToString()
                    + "/" +
                    SpinWheelManager.Ins.GetNumUpgradedNeed().ToString();
            }
            else
            {
                m_TextCurrentUpgraded.text = "Ready upgraded!";
            }
        }
        else
        {
            m_ImgProcess.fillAmount = 1;
            m_TextCurrentUpgraded.text = "Max level!";
        }
        m_TextLevel.text = "Tier " + (m_WheelLevel + 1).ToString();
    }
}
