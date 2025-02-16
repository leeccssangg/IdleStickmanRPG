
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIButton : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
{
    [SerializeField]
    [OnValueChanged("UpdateSpriteSprite")]
    private bool m_Interactable = true;
    [FormerlySerializedAs("m_QuikClickable")]
    [SerializeField]
    private bool m_QuickClickable;
    [SerializeField,BoxGroup("Sprites")]
    private Sprite m_DisableSprite;

    [SerializeField] private Animator m_Animator;
    [Serializable]
    /// <summary>
    /// Function definition for a button click event.
    /// </summary>
    public class ButtonClickedEvent : UnityEvent{}
   
    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    
    private bool m_IsPressing;
    private bool m_IsClicked;

    private Image m_ButtonImage;
    private Sprite m_OriginalSprite;

    private Transform m_Transform;
    
    public ButtonClickedEvent onClick
    {
        get => m_OnClick;
        set => m_OnClick = value;
    }

    private void Awake(){
        m_ButtonImage = GetComponent<Image>();
        m_Transform = GetComponent<Transform>();
        m_OriginalSprite = m_ButtonImage.sprite;
    }
    public bool interactable{
        get => m_Interactable;
        set{
            m_Interactable = value; 
            UpdateSpriteSprite();
        }
    }

    private void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData){
        if (!isActiveAndEnabled() || !interactable) return;
        //SoundManager.Instance.PlaySound(SoundManager.Instance.m_ButtonClick);
        if(m_QuickClickable){
            m_OnClick.Invoke();
            return;
        }
        m_IsClicked = true;
    }
    public void OnClick(){
        if(m_IsClicked){
            m_OnClick.Invoke();
            m_IsClicked = false;
        }
    }
    
    private bool isActiveAndEnabled() => gameObject.activeInHierarchy && enabled;
    public void OnPointerDown(PointerEventData eventData){
        if (!isActiveAndEnabled())
            return;
        m_IsPressing = true;
        m_Animator?.SetTrigger("down");
    }
    public void OnPointerUp(PointerEventData eventData){
        if(!m_IsPressing) return;
        m_IsPressing = false;
        m_Animator?.SetTrigger("up");
    }
    public void OnPointerExit(PointerEventData eventData){
        if(!m_IsPressing) return;
        m_IsPressing = false;
        m_Animator?.SetTrigger("up");
    }

#region SPRITE
    public void UpdateSpriteSprite(){
        var sprite = !m_Interactable && m_DisableSprite ? m_DisableSprite : m_OriginalSprite;
        if(!m_ButtonImage){
            m_ButtonImage = GetComponent<Image>();
        }
        m_ButtonImage.sprite = sprite;
    }
    
#endregion
    
    public void SetScaleZero(){
        m_Animator.enabled = false;
        m_Transform.localScale = Vector3.zero;
    }
    public void DoScale(float scale,Ease ease = Ease.Linear,float delay = 0){
        m_Transform.DOScale(scale,0.15f).SetEase(ease).SetDelay(delay).OnComplete(()=>m_Animator.enabled = true);
    }

}
