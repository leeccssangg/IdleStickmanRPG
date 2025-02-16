using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPosition:MonoBehaviour {

    public Transform m_MainCharacterPositon;
    public List<Transform> m_StickmanGroundPosition = new List<Transform>();
    public List<Transform> m_StickmanFlyPosition = new List<Transform>();

    public Vector3 m_MainCharacterPos;
    public Dictionary<int,Vector3> m_GroundPosDic = new Dictionary<int,Vector3>();
    public Dictionary<int,Vector3> m_FlyPosDic = new Dictionary<int,Vector3>();
    private void Awake() {
        //m_GroundPosDic.Clear();
        //m_FlyPosDic.Clear();
        //m_MainCharacterPos = m_MainCharacterPositon.position;
        //for(int i = 0;i < m_StickmanGroundPosition.Count;i++) {
        //    Transform t = m_StickmanGroundPosition[i];
        //    int num = i + 1;
        //    m_GroundPosDic.Add(num,t.position);
        //}
        //for(int i = 0;i < m_StickmanFlyPosition.Count;i++) {
        //    Transform t = m_StickmanFlyPosition[i];
        //    int nun = i + 1;
        //    m_FlyPosDic.Add(nun,t.position);
        //}
    }
    public void ResetCharacterPosition() {
        m_GroundPosDic.Clear();
        m_FlyPosDic.Clear();
        m_MainCharacterPos = m_MainCharacterPositon.position;
        for(int i = 0;i < m_StickmanGroundPosition.Count;i++) {
            Transform t = m_StickmanGroundPosition[i];
            int num = i + 1;
            m_GroundPosDic.Add(num,t.position);
        }
        for(int i = 0;i < m_StickmanFlyPosition.Count;i++) {
            Transform t = m_StickmanFlyPosition[i];
            int nun = i + 1;
            m_FlyPosDic.Add(nun,t.position);
        }
    }
    public Vector3 GetMainCharacterPosition() {
        return m_MainCharacterPos;
    }
    public Vector3 GetFreePosition(int slotID,Platform platform) {
        if(platform == Platform.GROUND) {
            return GetFreeGroundPosition(slotID);
        } else {
            return GetFreeFlyPosition(slotID);
        }
    }
    public Vector3 GetFreeGroundPosition(int slotID) {
        return m_GroundPosDic[slotID];
    }

    public Vector3 GetFreeFlyPosition(int slotID) {
        return m_FlyPosDic[slotID];
    }
    public void SetupNextRoundPosition() {
        float num = 6f;
        m_MainCharacterPos += new Vector3(num,0,0);
        {
            List<int> keys = new List<int>(m_GroundPosDic.Keys);
            foreach(int k in keys) {
                Vector3 v = m_GroundPosDic[k];
                v += new Vector3(num,0,0);
                m_GroundPosDic[k] = v;
            }
        }
        {
            List<int> keys = new List<int>(m_FlyPosDic.Keys);
            foreach(int k in keys) {
                Vector3 v = m_FlyPosDic[k];
                v += new Vector3(num,0,0);
                m_FlyPosDic[k] = v;
            }
        }
    }
    public void SetupPreviousRoundPosition() {
        float num = 8f;
        {
            List<int> keys = new List<int>(m_GroundPosDic.Keys);
            foreach(int k in keys) {
                Vector3 v = m_GroundPosDic[k];
                v -= new Vector3(num,0,0);
                m_GroundPosDic[k] = v;
            }
        }
        {
            List<int> keys = new List<int>(m_FlyPosDic.Keys);
            foreach(int k in keys) {
                Vector3 v = m_FlyPosDic[k];
                v -= new Vector3(num,0,0);
                m_FlyPosDic[k] = v;
            }
        }
    }
}
