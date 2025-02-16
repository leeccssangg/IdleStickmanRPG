using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelWave:MonoBehaviour {

    public List<GameObject> m_WavePoints = new List<GameObject>();
    public List<GameObject> m_WaveActivePoints = new List<GameObject>();

    public void Setup(int wave) {
        for(int i = 0;i < 4;i++) {
            if(i <= wave - 1) {
                m_WaveActivePoints[i].SetActive(true);
            } else {
                m_WaveActivePoints[i].SetActive(false);
            }
        }
    }
}
