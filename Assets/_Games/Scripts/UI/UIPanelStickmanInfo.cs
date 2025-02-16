using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanelStickmanInfo:UICanvas {

    public SkeletonGraphic m_UISpine;

    public UIStickmanRace UIRace;
    public Image m_ClassImage;

    public TextMeshProUGUI m_TextStickmanName;
    public TextMeshProUGUI m_TextLevel;
    public TextMeshProUGUI m_TextRarity;
    public TextMeshProUGUI m_TextAttack;
    public TextMeshProUGUI m_TextAttackSpeed;
    public TextMeshProUGUI m_TextOwnedEffect;

    public UIButton m_ButtonClose;
    public UIButton m_ButtonEquip;
    public UIButton m_ButtonRemove;
    public UIButton m_ButtonEnhance;

    public UISlider m_UIProgressPiece;


    public UnityAction<StickmanInfo> OnUpgrageCallback;
    public UnityAction<StickmanInfo> OnEquipCallback;
    public UnityAction<StickmanInfo> OnRemoveCallback;

    private StickmanInfo m_StickmanInfo;

    private void Awake() {
        m_ButtonEnhance.onClick.AddListener(OnUpgrage);
        m_ButtonEquip.onClick.AddListener(OnEquip);
        m_ButtonRemove.onClick.AddListener(OnRemove);
        m_ButtonClose.onClick.AddListener(OnClose);
    }
    public void Setup(StickmanInfo stickman,UnityAction<StickmanInfo> upgradeCallback,UnityAction<StickmanInfo> equipCallback,UnityAction<StickmanInfo> removeCallback) {
        m_StickmanInfo = stickman;

        OnUpgrageCallback = upgradeCallback;
        OnEquipCallback = equipCallback;
        OnRemoveCallback = removeCallback;
        UpdateInfo();
        UpdateUI();
    }
    private void UpdateInfo() {
        Class classType = m_StickmanInfo.Class;
        Race race = m_StickmanInfo.Race;

        UIRace.Setup(SpriteManager.Ins.GetStickmanRaceSprite(race));
        m_ClassImage.sprite = SpriteManager.Ins.GetClassSprite(classType).sprite;
        m_TextStickmanName.text = m_StickmanInfo.Name;
        m_UISpine.SkeletonDataAsset = SpriteManager.Ins.GetStickmanSkeleton(classType,race);

        int skinID = Mathf.Clamp((int)m_StickmanInfo.Rarity,1,5);

        m_UISpine.initialSkinName = (skinID.ToString());
        m_UISpine.Initialize(true);
        //m_UISpine.Skeleton.SetSkin(skinID.ToString());
    }
    private void UpdateUI() {
        int level = m_StickmanInfo.Level;
        m_TextLevel.text = "LEVEL " + level;
        m_TextAttack.text = m_StickmanInfo.GetAttackStat().ToString3();
        m_TextAttackSpeed.text = m_StickmanInfo.GetAttackSpeed().ToString();
        m_TextOwnedEffect.text = $"ATK +{m_StickmanInfo.GetOwnerEffect().ToString3()}%";

        m_UIProgressPiece.SetProgress(m_StickmanInfo.GetLevelProgress());
        m_UIProgressPiece.SetTextProgress(m_StickmanInfo.GetLevelProgressString());

        m_ButtonEnhance.interactable = m_StickmanInfo.UpgradeAble();
        m_ButtonEquip.interactable = m_StickmanInfo.EquipAble();
        bool isEquiped = StickManManager.Ins.StickmanIsEquiped(m_StickmanInfo);
        m_ButtonRemove.gameObject.SetActive(isEquiped);
        m_ButtonEquip.gameObject.SetActive(!isEquiped);
    }
    public void OnUpgrage() {
        OnUpgrageCallback?.Invoke(m_StickmanInfo);
        UpdateUI();
    }
    public void OnEquip() {
        OnEquipCallback?.Invoke(m_StickmanInfo);
        OnClose();
    }
    public void OnRemove() {
        OnRemoveCallback?.Invoke(m_StickmanInfo);
        OnClose();
    }
    public void OnClose() {
        base.Close(0);
    }
}
