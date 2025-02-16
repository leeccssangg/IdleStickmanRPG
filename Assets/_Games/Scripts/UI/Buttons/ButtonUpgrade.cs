using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUpgrade : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private Image m_ButtonSprite;
    [SerializeField] private Sprite[] m_ButtonSprites;
    [SerializeField] private TextMeshProUGUI m_ButtonText ;
    [SerializeField] private Animator m_Animator;
    private float m_HoldDownStartTime;
    private bool m_IsPressing;
    private bool m_UpgradeAble;
    private float m_NextActionTime;
    private float m_Num;
    private UnityAction m_PressAction;
    
    public void AddOnClickListener(UnityAction pressAction){
        m_PressAction = pressAction;
    }
    public void OnPointerClick(PointerEventData eventData){
        DoPressAction();
    }
    public void OnPointerDown(PointerEventData eventData){
        m_Animator.SetBool("press",true);
        m_HoldDownStartTime = Time.time;
        m_Num = 0.5f;
        m_IsPressing = true;
    }
    public void OnPointerUp(PointerEventData eventData){
        m_Animator.SetBool("press",false);
        m_Animator.SetBool("hold",false);
        m_IsPressing = false;
    }
    private void Update(){
        if(!m_IsPressing) return;
        float holdTime = Time.time - m_HoldDownStartTime;
        if(holdTime > 0.15f && m_UpgradeAble){
            // m_Animator.SetBool("hold",true);
            if(Time.time > m_NextActionTime){
                m_Animator.SetBool("press",true);
                m_NextActionTime = Time.time + m_Num;
                DoPressAction();
                m_Num -= 0.1f;
                m_Num = Mathf.Clamp(m_Num, 0.1f, 0.5f);
            } else{
                m_Animator.SetBool("press",false);
            }
        } else{
            m_Animator.SetBool("press",true);
            //m_Animator.SetBool("hold",false);
        }
    }
    private void DoPressAction(){
        m_PressAction?.Invoke();
    }
    public void SetUpgradeAble(bool able){
        m_UpgradeAble = able;
        var color = able ? Color.white : Color.red;
        var index = able ? 0 : 1;
        m_ButtonText.color = color;
        m_ButtonSprite.sprite = m_ButtonSprites[index];
    }
}
