using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    public RectTransform m_FullProgressBar;
    public RectTransform m_ProgressBar;
    public Text m_TextProgress;
    private float m_MaxProgressSize;
    private float m_CurrentProgress;
    private float m_TargetProgress;
    private float t = 0;

}
