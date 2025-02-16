using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIEdge : MonoBehaviour
{
    public GameObject m_ImageFill;

    [Button]
    public void Setup(Transform startTarget, Transform endTarget)
    {
        RectTransform startRect = startTarget.GetComponent<RectTransform>();
        RectTransform endRect = endTarget.GetComponent<RectTransform>();

        float distance = Vector3.Distance(startRect.position, endRect.position);

        GetComponent<RectTransform>().position = (startRect.position + endRect.position) / 2;
        GetComponent<RectTransform>().rotation = Quaternion.LookRotation(Vector3.back, endRect.position - startRect.position);

        GetComponent<RectTransform>().sizeDelta = new Vector2(25, distance);
    }
    public void SetFill(bool value)
    {
        m_ImageFill.SetActive(value);
    }
}
