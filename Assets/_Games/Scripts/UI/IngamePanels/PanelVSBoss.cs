using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class PanelVSBoss : MonoBehaviour{
    [SerializeField] private SkeletonGraphic m_PlayerSkeletonGraphic;
    [SerializeField] private SkeletonGraphic m_BossSkeletonGraphic;
    [SerializeField] private Transform m_VsTransform;
    [SerializeField] private RectTransform m_PlayerTransform;
    [SerializeField] private RectTransform m_BossTransform;
    [SerializeField] private float m_TargetPosX;
    [SerializeField] private CanvasGroup m_CanvasGroup;
    private Vector3 m_OriginPosX;
    private void Awake(){
        m_OriginPosX = m_BossTransform.anchoredPosition;
    }
    public void Setup(){
        m_CanvasGroup.alpha = 1;
        gameObject.SetActive((true));
        m_VsTransform.localScale = Vector3.zero;
        StartCoroutine((CO_ShowPanel()));   
    }
    private IEnumerator CO_ShowPanel(){
        m_PlayerTransform.DOAnchorPosX(-m_TargetPosX,0.5f).From(-m_OriginPosX);
        m_BossTransform.DOAnchorPosX(m_TargetPosX,0.5f).From(m_OriginPosX);;
        yield return Yielders.Get(0.5f);
        m_VsTransform.DOScale(1,0.15f).From(7).SetEase(Ease.OutBack);
        yield return Yielders.Get(1f);
        m_CanvasGroup.DOFade(0, 0.25f).From(1);
        yield return Yielders.Get(0.5f);
        gameObject.SetActive(false);
    }
}