using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class RepeatableButton : Button
{
    private const float MinTimeInterval = 0.1f;
    private const float MaxTimeInterval = 0.5f;
    private float m_CurrentTimeInterval;
    private Coroutine m_RepeatCoroutine;
    private bool m_IsClickDone;
    public UnityEvent onPointerUp;

    public override void OnPointerDown(PointerEventData eventData)
    {
        m_IsClickDone = false;
        m_RepeatCoroutine = StartCoroutine(CoRepeat());
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        StopCoroutine(m_RepeatCoroutine);

        if (!m_IsClickDone)
        {
            PressRepeat();
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        return;
    }

    private IEnumerator CoRepeat()
    {
        m_CurrentTimeInterval = MaxTimeInterval;
        while (true)
        {
            yield return new WaitForSeconds(m_CurrentTimeInterval);
            if (m_CurrentTimeInterval > MaxTimeInterval)
            {
                m_CurrentTimeInterval = MaxTimeInterval;
            }
            if (m_CurrentTimeInterval > MinTimeInterval)
            {
                m_CurrentTimeInterval -= 0.5f * m_CurrentTimeInterval;
            }
            PressRepeat();
            m_IsClickDone = true;
        }
    }
    private void PressRepeat()
    {
        if (!IsActive() || !IsInteractable())
            return;

        UISystemProfilerApi.AddMarker("Button.onClick", this);
        onClick.Invoke();
    }
    public void SetTimeInterval(float time)
    {
        m_CurrentTimeInterval = time;
    }
}
