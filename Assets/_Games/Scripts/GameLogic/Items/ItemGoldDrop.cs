using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoldDrop : MonoBehaviour {
    public Rigidbody2D m_Rigidbody;
    private float xForce = 0;
    private BigNumber m_Amount;
    private float m_TimeToAutoCollect = 5;
    private bool IsCollected = false;
    private Transform m_Transform;
    public Transform Transform {
        get {
            if(m_Transform == null) {
                m_Transform = transform;
            }
            return m_Transform;
        }
    }
    private void Start() {
        EventManager.StartListening("CollectGold", () => {
            if (gameObject.activeInHierarchy) {
                PickUp();
            }
        });
    }
    private void Update() {
        if (!IsCollected) {
            m_TimeToAutoCollect -= Time.deltaTime;
            if (m_TimeToAutoCollect < 0) {
                PickUp();
            }
        }
    }
    private void OnEnable() {
        xForce = Random.Range(-50, 50);
        m_Rigidbody.AddForce(new Vector3(xForce, Random.Range(200, 300), 0));
        m_TimeToAutoCollect = 1;
        IsCollected = false;
    }
    public void Setup(BigNumber amount) {
        m_Rigidbody.gravityScale = 1;
        m_Amount = amount;
        //ItemManager.Instance.AddActiveItem(this);
    }
    public virtual void PickUp() {
        IsCollected = true;
        Transform pos = UIManager.Ins.GetUI<UIPanelIngame>().PanelGold.GoldTransform;
        Fly(pos);
    }
    private void Fly(Transform pos) {
        m_Rigidbody.gravityScale = 0;
        //ItemManager.Instance.RemoveActiveItem(this);
        StartCoroutine(CO_MoveToTarget(pos, Random.Range(1f, 1.5f)));
    }
    private IEnumerator CO_MoveToTarget(Transform target, float time) {
        float num = 0;
        num += Time.deltaTime / time;
        float num1 = (target.position - Transform.position).magnitude;
        while (num1 > 0.2f) {
            Vector3 v = Vector3.Lerp(Transform.position, target.position, num);
            Transform.position = v;
            num += Time.deltaTime / time;
            num1 = (target.position - Transform.position).magnitude;
            yield return Yielders.EndOfFrame;
        }
        Transform.position = target.position;

        // EventManager.TriggerEvent("PingGold");
        ProfileManager.PlayerData.AddGameResource(ResourceData.ResourceType.GOLD, m_Amount) ;
        PrefabManager.Instance.DespawnPool(gameObject);
    }
}
