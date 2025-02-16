using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController:MonoBehaviour {

    public List<UIMainTab> m_UIMaintab = new List<UIMainTab>();
    public int m_CurrentPage;


    public void SetToPage(int index) {
        m_CurrentPage = index;
        for(int i = 0;i < m_UIMaintab.Count;i++) {
            int page = i + 1;
            if(m_CurrentPage == page) {
                m_UIMaintab[i].gameObject.SetActive(true);
            }
        }
    }
}
