using Sirenix.OdinInspector;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanPosition:MonoBehaviour {
    public List<Transform> m_GoundTrans;
    public List<Transform> FlyTrans;

    private Dictionary<int,Vector3> m_GroundPosition = new();
    private Dictionary<int,Vector3> m_FlyPosition = new();

    public void ResetPosition() {
        m_GroundPosition.Clear();
        m_FlyPosition.Clear();
        for(int i = 0;i < m_GoundTrans.Count;i++) {
            m_GroundPosition.Add(i+1,m_GoundTrans[i].position);
        }
        for(int i = 0;i < FlyTrans.Count;i++) {
            m_FlyPosition.Add(i + 1,FlyTrans[i].position);
        }
    }
    [Button]
    public void SetNextRoundPosition() {
        float num = 6f;
        {
            List<int> keys = new List<int>(m_GroundPosition.Keys);
            foreach(int k in keys) {
                Vector3 v = m_GroundPosition[k];
                v += new Vector3(num,0,0);
                m_GroundPosition[k] = v;
            }
        }
        {
            List<int> keys = new List<int>(m_FlyPosition.Keys);
            foreach(int k in keys) {
                Vector3 v = m_FlyPosition[k];
                v += new Vector3(num,0,0);
                m_FlyPosition[k] = v;
            }
        }

    }
    public Vector3 GetPosition(int slotID,Platform platform) {
        if(platform == Platform.GROUND) {
            return GetGroundSlot(slotID);
        } else {
            return GetFlySlot(slotID);
        }
    }
    public Vector3 GetGroundSlot(int slotID) {
        return m_GroundPosition[slotID];
    }
    public Vector3 GetFlySlot(int slotID) {
        return m_FlyPosition[slotID];
    }
}
