using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestBullet : MonoBehaviour{
    public float m_SpeedRotate;
    public Transform m_Hammer;
    [SerializeField]
    private Transform m_Root;
    public Ease m_Ease;
    [Button]
    public  void FireTween(){
        m_Hammer.DOKill();
        var startPos = new Vector2(3.5f, 6);
        m_Hammer.DOLocalMove(Vector3.zero, 0.7f).From(startPos).SetEase(Ease.InCirc).OnComplete(() => {
            CameraManger.Ins.ShakeCamera(0.15f);
            // IngameManager.Ins.PutEffect(Transform.position, m_EndEffect);
            m_Root.localEulerAngles = (new Vector3(0, 0, Random.Range(90, 180)));
        }).OnUpdate(RotateHamer);
    }
    private void RotateHamer(){
        m_Root.Rotate(Vector3.forward * Time.deltaTime * m_SpeedRotate);
    }
    public void Update(){
        
    }
}