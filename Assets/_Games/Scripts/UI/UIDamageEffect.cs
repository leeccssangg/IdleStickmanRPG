using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageEffect : MonoBehaviour
{
    public GameObject m_CriticalMark;
    public TextMeshPro m_TextDamage;
    public Canvas m_CanvasDamage;
    private int m_NormalSize = 300;
    private int m_BigSize = 350;
    private int m_HugeSize = 400;
    public void SetColor(Color c) {
        m_TextDamage.color = c;
    }
    public UIDamageEffect SetDamage(BigNumber damage) {
        m_TextDamage.text = damage.ToStringUI();
        return this;
    }
    public void SetTextSize(int size) {
        m_TextDamage.fontSize = size;
    }
    public void SetDamageType(DamageType dt) {
        m_CanvasDamage.sortingOrder = 0;
        switch(dt) {
            case DamageType.MISS:
            case DamageType.NORMAL:
                SetColor(Color.white);
                //m_CriticalMark.SetActive(false);
                SetTextSize(m_NormalSize);
                break;
            case DamageType.CRIT:
                SetColor(Color.red);
                //m_CriticalMark.SetActive(true);
                SetTextSize(m_HugeSize);
                m_CanvasDamage.sortingOrder = 3;
                break;
            case DamageType.CHAOS:
                SetColor(Color.yellow);
                //m_CriticalMark.SetActive(true);
                SetTextSize(m_BigSize);
                m_CanvasDamage.sortingOrder = 2;
                break;
            case DamageType.FIRE:
                SetColor(Color.red);
               // m_CriticalMark.SetActive(false);
                m_CanvasDamage.sortingOrder = 1;
                SetTextSize(m_BigSize);
                break;
            case DamageType.POISON:
                SetColor(Utilss.GetColor(144,0,255));
                //m_CriticalMark.SetActive(false);
                m_CanvasDamage.sortingOrder = 1;
                SetTextSize(m_BigSize);
                break;
            case DamageType.SKILL:
                SetColor(Utilss.GetColor("#FF5500"));
                //m_CriticalMark.SetActive(false);
                SetTextSize(m_BigSize);
                m_CanvasDamage.sortingOrder = 1;
                break;
            case DamageType.REFLECT:
                SetColor(Utilss.GetColor("#FF9800"));
               // m_CriticalMark.SetActive(false);
                SetTextSize(m_HugeSize);
                m_CanvasDamage.sortingOrder = 1;
                break;
        }
    }
    public void OnEffectDone() {
        SimplePool.Despawn(gameObject);
    }
}
