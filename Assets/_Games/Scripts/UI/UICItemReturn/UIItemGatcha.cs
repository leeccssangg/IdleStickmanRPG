using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIItemGatcha : UIItemBase
{
    // [SerializeField] private ItemType m_ItemType = ItemType.GATCHA;
    [SerializeField] private GatchaConfigInfo m_GatchaConfigInfo;
    [SerializeField] private GameObject m_GearImage;
    [SerializeField] private Transform m_Content;
    [SerializeField] private ParticleImage m_Effect;
    public Rarity ItemRarity{ get; set; }

    public override void InitData(GatchaConfigInfo gatchaConfig)
    {
        base.InitData(gatchaConfig);
        m_ItemType = ItemType.GATCHA;
        m_GatchaConfigInfo = gatchaConfig;
        switch (m_GatchaConfigInfo.gatchaType)
        {
            case GatchaType.Stickman:
                StickmanInfo stickmanInfo = StickManManager.Ins.GetStickmanInforByID(m_GatchaConfigInfo.id);
                Setup(stickmanInfo);
                break;
            case GatchaType.Skill:
                SkillInfo skillInfo = SkillManager.Ins.GetSkillInfoByID(m_GatchaConfigInfo.id);
                Setup(skillInfo);
                break;
            case GatchaType.Gear:
                ItemGearInfo gearInfo = GearManager.Ins.GetGearInfoByTypeAndID(m_GatchaConfigInfo.gearType, m_GatchaConfigInfo.id);
                Setup(gearInfo);
                break;
            default:
                break;
        }

        m_Content.DOKill();
        m_Content.localScale = Vector3.one * 2.2f;
        m_Content.gameObject.SetActive(false);
        //Setup(gatchaConfig.IconItem, gatchaConfig.IsShowEffectGlow, gatchaConfig.Amount, gatchaConfig.IconType, gatchaConfig.ImgRarity);
    }
    private void Setup(StickmanInfo stickmanInfo)
    {
        if(m_GearImage) m_GearImage.SetActive(false);
        m_TxtAmount.gameObject.SetActive(false);
        m_IconItem.sprite = stickmanInfo.GetIcon();
        m_ImgBg.sprite = SpriteManager.Ins.GetGachaRaritySprite(stickmanInfo.Rarity);
        m_EffectGlow.SetActive((int)stickmanInfo.Rarity >= (int)Rarity.EPIC);
        ItemRarity = stickmanInfo.Rarity;
    }
    private void Setup(SkillInfo skillInfo)
    {
        if(m_GearImage) m_GearImage.SetActive(false);
        m_TxtAmount.gameObject.SetActive(false);
        m_IconItem.sprite = skillInfo.GetIcon();
        m_ImgBg.sprite = SpriteManager.Ins.GetGachaRaritySprite(skillInfo.Rarity);
        m_EffectGlow.SetActive((int)skillInfo.Rarity >= (int)Rarity.EPIC);
        ItemRarity = skillInfo.Rarity;
    }
    private void Setup(ItemGearInfo gearInfo)
    {
        m_TxtAmount.gameObject.SetActive(false);
        if(m_GearImage) m_GearImage.SetActive(true);
        m_IconType.sprite = gearInfo.GetIconType();
        m_IconItem.sprite = gearInfo.GetIcon();
        m_ImgBg.sprite = SpriteManager.Ins.GetGachaRaritySprite(gearInfo.Rarity);
        m_EffectGlow.SetActive((int)gearInfo.Rarity >= (int)Rarity.EPIC);
        ItemRarity = gearInfo.Rarity;
    }
    public void Show(){
        m_Effect.Play();
        m_Content.gameObject.SetActive(true);
        m_Content.DOScale(1, 0.25f).SetEase(Ease.OutBack);
    }
}
