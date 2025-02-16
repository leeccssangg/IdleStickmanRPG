using System.Collections.Generic;

public abstract class Observer<T>
{
    public abstract void OnNotify(T id, string info);
}
public class Subject<T>
{
    protected List<Observer<T>> m_Observers = new List<Observer<T>>();

    public void AddObserver(Observer<T> observer)
    {
        m_Observers.Add(observer);
    }
    public void ClearObserver()
    {
        m_Observers.Clear();
    }
    public void RemoveObserver(Observer<T> observer)
    {
        m_Observers.Remove(observer);
    }

    public virtual void Notify(T id, string info)
    {
        int len = m_Observers.Count;
        for (int i = 0; i < len; i++)
        {
            m_Observers[i].OnNotify(id, info);
        }
    }
    public virtual bool AnyToClaim()
    {
        return false;
    }
};