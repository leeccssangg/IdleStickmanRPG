using UnityEngine ;

namespace EasyUI.PickerWheelUI {
   [System.Serializable]
   public class WheelPiece  : MonoBehaviour {
        public Sprite m_Icon;
        public GiftType m_ItemType;

        [Tooltip("Reward amount")] public float m_Amount;

        [Tooltip("Probability in %")]
        [Range(0f, 100f)]
        public float m_Chance = 100f;

        [HideInInspector] public int m_Index;
        [HideInInspector] public double _weight = 0f;
        public WheelDataConfigs<GiftType> m_WheelDataConfigs;
        public void Setup(WheelDataConfigs<GiftType> wheelData)
        {
            m_WheelDataConfigs = wheelData;
            m_Icon = m_WheelDataConfigs.m_Icon;
            m_ItemType = m_WheelDataConfigs.m_ItemType;
            m_Amount = m_WheelDataConfigs.m_Amount;
            m_Chance = m_WheelDataConfigs.m_Chance;
        }
    }
}
