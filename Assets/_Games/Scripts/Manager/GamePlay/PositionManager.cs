using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PositionManager : MonoBehaviour{
    
    [SerializeField] private float m_Distance = 6f;
    [SerializeField] private Transform m_AllTransform;
    [FormerlySerializedAs("m_StickmanGroundPoTransforms")]
    [SerializeField]
    private Transform[] m_StickmanGroundTransforms;

    [SerializeField]
    private Transform[] m_StickmanFlyPoTransforms;

    [SerializeField]
    private Transform m_PlayerTranform;

    [SerializeField]
    private Transform m_EnemyFlyTransform;

    [SerializeField]
    private Transform m_EnemyGroundTransform;

    public Transform PlayerTranform{ get => m_PlayerTranform; set => m_PlayerTranform = value; }
    public Transform EnemyGroundTransform{ get => m_EnemyGroundTransform; set => m_EnemyGroundTransform = value; }
    public void ResetAllPosition(){
        m_AllTransform.position = Vector3.zero;
    }
    public void SetNextWave(){
        m_AllTransform.position += Vector3.right * m_Distance;
    }
    private Vector3 m_Position;
    private void Update(){
        Vector3 v = IngameManager.Ins.Hero.PlayerTransform.position;
        m_Position.x = v.x + 2f;
        m_Position.y = m_AllTransform.position.y;
        m_Position.z = 0;
        m_AllTransform.position = m_Position;
    }
    public Transform GetStickmanGroundTransform(int index,Platform platform){
        switch(platform){
            case Platform.NONE:
                break;
            case Platform.GROUND:
                return m_StickmanGroundTransforms[index-1];
            case Platform.FLY:
                return m_StickmanFlyPoTransforms[index-1];
            default:
                break;
        }
        return null;
    }
    public Vector3 GetEnemySpawnPos(Platform plasform){
        switch(plasform){
            case Platform.NONE:
                break;
            case Platform.GROUND:
                return m_EnemyGroundTransform.position;
            case Platform.FLY:
                return GetEnemyFlyPos();
            default:
                break;
        }
        return Vector3.zero;
    }
    public Vector3 GetEnemyFlyPos(){
        float num = Random.Range(-1, 1f);
        return m_EnemyFlyTransform.position + Vector3.up * num;
    }
}