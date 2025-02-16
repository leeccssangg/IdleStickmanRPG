using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageDispatcher  : MonoBehaviour{
    public static float SEND_MSG_IMMEDIATELY = 0.0f;
    public static Object NO_ADDITIONAL_INFO = null;

    private static MessageDispatcher m_Instance = null;
    public static MessageDispatcher Instance {
        get {
            if (m_Instance == null) {
                GameObject go = new GameObject();
                go.name = "MessageDispatcher";
                m_Instance = go.AddComponent<MessageDispatcher>();
            }
            return m_Instance;
        }
    }
    private List<Telegram> PriorityQ = new List<Telegram>();

    void Awake() {
        m_Instance = this;
    }

    //void Update() {
    //    float currentTime = Time.time;
    //    while(PriorityQ.Count > 0
    //        && (PriorityQ[0].DispatchTime < currentTime)
    //        && (PriorityQ[0].DispatchTime > 0)) {
    //        Telegram telegram = PriorityQ[0];
    //        IngameObject pReceiver = IngameEntityManager.Instance.GetEntityFromID(telegram.Receiver);
    //        Discharge(pReceiver, telegram);
    //        PriorityQ.Remove(PriorityQ[0]);
    //    }
    //}

    //private void Discharge(IngameObject receiver, Telegram msg) {
    //    if (!receiver.HandleMessage(msg)) {
    //    }
    //}

    //public void DispatchMessage(float delay, int sender, int receiver, MessageTypes msg, Object extraObject, Vector3 extraVector) {
    //    IngameObject pSender = IngameEntityManager.Instance.GetEntityFromID(sender);
    //    IngameObject pReceiver = IngameEntityManager.Instance.GetEntityFromID(receiver);
    //    if (pReceiver == null) {
    //        return;
    //    }
    //    Telegram telegram = new Telegram(0, sender, receiver, msg, extraObject, extraVector);
    //    if (delay <= 0.0f) {
    //        Discharge(pReceiver, telegram);
    //    } else {
    //        float currentTime = Time.time;
    //        telegram.DispatchTime = currentTime + delay;
    //        PriorityQ.Add(telegram);
    //        TelegramComparer tc = new TelegramComparer();
    //        PriorityQ.Sort(tc);
    //        //Debug.Log("\nDelayed telegram from " + pSender.ID()
    //        //        + " recorded at time " + currentTime + " for "
    //        //        + pReceiver.ID() + ". Msg is " + msg);
    //    }
    //}
    //public void DispatchMessage(IngameType receiverType, MessageTypes msg, Object extraObject, Vector3 extraVector) {
    //    Stack<IngameObject> pReceivers = IngameEntityManager.Instance.GetAllEnemyByType(receiverType);
    //    if(pReceivers == null) {
    //        return;
    //    }
    //    Telegram telegram = new Telegram(0, (int)receiverType, msg, extraObject, extraVector);
    //    while(pReceivers.Count > 0) {
    //        Discharge(pReceivers.Pop(), telegram);
    //    }
    //}
}
