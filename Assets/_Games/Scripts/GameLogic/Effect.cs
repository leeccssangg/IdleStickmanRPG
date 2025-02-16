using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    protected float m_LifeTime = 4;
    protected Transform m_Transform;
    protected IngameObject m_Owner;
    protected bool m_IsFollow;
    private Vector3 m_Offset;
    public virtual void Setup( float lifeTime ) {
        m_LifeTime = lifeTime;
    }
    private void OnEnable() {
        m_LifeTime = 5;
    }
    private void Update() {
        OnRunning();
    }
    public virtual void SetColor( Color c ) {
    }
    public virtual void SetColor( int colorIndex ) {
    }
    public virtual void OnRunning() {
        if(m_IsFollow) {
            if(m_Owner != null) {
                Transform.position = m_Owner.Transform.position + m_Offset;
                if(m_Owner.IsDeActive()) {
                    Deactive();
                }
            }
        }
        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime <= 0) {
            Deactive();
        }
    }
    public void SetFollow(IngameObject owner) {
        m_Owner = owner;
        m_IsFollow = true;
        m_Offset = Vector3.zero;
    }
    public void SetFollow(IngameObject owner,Vector3 offset) {
        m_Owner = owner;
        m_IsFollow = true;
        m_Offset = offset;
    }
    public void SetFollow(IngameObject owner, Vector3 offset, float lifeTime) {
        m_Owner = owner;
        m_IsFollow = true;
        m_Offset = offset;
        m_LifeTime = lifeTime;
    }
    public Transform Transform {
        get {
            if (m_Transform == null) {
                m_Transform = transform;
            }
            return m_Transform;
        }
    }
    public void Deactive() {
        if (gameObject.activeInHierarchy) {
            PrefabManager.Instance.DespawnPool(gameObject);
        }
    }
}
