using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameHealthbar : MonoBehaviour
{
    public Slider m_UIHP;
    private float m_HPTargetProgress;

    public Vector3 offset = new Vector3(0, 0, 0.0f);
    public Character m_Owner;
    public Image m_HealthBar;
    public List<Sprite> m_Sprites = new List<Sprite>();
    private Transform m_Transform;
    public Transform Transform {
        get {
            if (m_Transform == null) {
                m_Transform = transform;
            }
            return m_Transform;
        }
    }
    public void Init() {

    }
    public void SetOwner(Character owner ) {
        m_Owner = owner;
        if (owner.GetTeam() == IngameTeam.Team1) {
            m_HealthBar.sprite = m_Sprites[0];
        } else {
            m_HealthBar.sprite = m_Sprites[1];
        }
    }

    private void LateUpdate() {
        m_HPTargetProgress = (m_Owner.GetCurrentHpPercentage()/100);
        m_UIHP.value = m_HPTargetProgress;
        Transform.position = m_Owner.m_HeadPoint.transform.position + offset;
    }
    public void Despawn() {
        SimplePool.Despawn(gameObject);
    }
}
