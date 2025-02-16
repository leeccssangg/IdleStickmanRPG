using UnityEngine;
using UnityEngine.Events;

public class CounterTime
{
    private UnityAction CounterDoneAction;
    private float totalTime;
    private float time;
    public float m_Time => totalTime - time;

    public void CounterStart(UnityAction startAction, UnityAction doneAction, float time)
    {
        startAction?.Invoke();
        CounterDoneAction = doneAction;
        totalTime = time;
        this.time = time;
    }

    public void CounterExecute()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                time = 0;
                CounterExit();
            }
        }
    }

    private void CounterExit()
    {
        CounterDoneAction?.Invoke();
    }
}
