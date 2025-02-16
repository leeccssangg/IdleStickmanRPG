using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelGatcha : MonoBehaviour
{
    [SerializeField] private List<UIGatchaTab> m_GatchaTabs;

    public void Setup()
    {
        m_GatchaTabs.ForEach(x => x.Setup());
    }
}
