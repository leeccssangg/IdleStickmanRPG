using UnityEngine;
using System.Collections;

public class StateMachine<T> {
    private T m_Owner;
    private State<T> m_CurrentState;
    private State<T> m_PreviousState;
    private State<T> m_GlobalState;
    private State<T> m_NextState;
    private bool IsPauseCurrentState = false;
    public StateMachine(T owner) {
        m_Owner = owner;
        m_CurrentState = null;
        m_PreviousState = null;
        m_GlobalState = null;
    }
    public void SetCurrentState(State<T> s) { m_CurrentState = s; }
    public void SetPreviousState(State<T> s) { m_PreviousState = s; }
    public void SetGlobalState(State<T> s) { m_GlobalState = s; }
    public void PauseCurrentState(bool isPause) { IsPauseCurrentState = isPause; }

    /// <summary>
    /// Change to global first because it change direction of ship.
    /// </summary>
    public void Update() {
        //if(GameManager.Instance.IsPauseGame())return;
        if (m_GlobalState != null) {
            m_GlobalState.Execute(m_Owner);
        }
        if (m_CurrentState != null && !IsPauseCurrentState) {
            m_CurrentState.Execute(m_Owner);
        }
    }
    public bool HandleMessage(Telegram msg) {
        if (m_CurrentState != null && m_CurrentState.OnMessage(m_Owner, msg)) {
            return true;
        }
        if (m_GlobalState != null && m_GlobalState.OnMessage(m_Owner, msg)) {
            return true;
        }
        return false;
    }
    public void ChangeState(State<T> newState) {
        m_PreviousState = m_CurrentState;
        m_CurrentState.Exit(m_Owner);
        m_CurrentState = newState;
        m_CurrentState.Enter(m_Owner);
    }
    public void RevertToPreviousState(){
        ChangeState(m_PreviousState);
    }
    public State<T> CurrentState { get { return m_CurrentState; } }
    public State<T> PreviousState { get { return m_PreviousState; } }
    public State<T> NextState { get { return m_NextState; } }
    public State<T> GlobalState { get { return m_GlobalState; } }
    public bool isInState(State<T> st) { return (m_CurrentState == st) ? true : false; }
}
