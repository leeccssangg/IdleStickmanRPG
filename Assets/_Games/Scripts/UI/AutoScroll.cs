using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll:MonoBehaviour {
    public Scrollbar m_ScrollBar;
    public int m_currentRow;
    public int m_Row;
    public float distance;
    public float targetValue;
    public float targetValue2;
    public float[] pos;
    public void AutoScrollToTaget(int row,int currentRow) {
        m_Row = row;
        currentRow /= 5;
        m_currentRow = currentRow;
        pos = new float[m_currentRow];
        distance = 1f/(row - 1);
        for(int i = 0;i < pos.Length;i++) {
            pos[i] = distance * i;
        }
        float value = 1f;
        targetValue = distance * currentRow;
        targetValue2 = (1 - targetValue);
        DOTween.To(()=> value,x => value = x,(targetValue2),0.5f).OnUpdate(()=>m_ScrollBar.value = value);
    }
}
