using UnityEngine;
using UnityEngine.UI;

namespace Pextension
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class UICanvas : MonoBehaviour
    {
        //public bool IsAvoidBackKey = false;
        public bool IsDestroyOnClose = false;

        //protected RectTransform m_RectTransform;
        //private Animator Animator;
        //private bool m_IsInit = false;
        //private float m_OffsetY = 0;

        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            //m_RectTransform = GetComponent<RectTransform>();
            //Animator = GetComponent<Animator>();

            //float ratio = (float)Screen.height / (float)Screen.width;

            //// xu ly tai tho
            //if (ratio > 2.1f)
            //{
            //    Vector2 leftBottom = m_RectTransform.offsetMin;
            //    Vector2 rightTop = m_RectTransform.offsetMax;
            //    rightTop.y = -100f;
            //    m_RectTransform.offsetMax = rightTop;
            //    leftBottom.y = 0f;
            //    m_RectTransform.offsetMin = leftBottom;
            //    m_OffsetY = 100f;
            //}
            //m_IsInit = true;
        }

        public virtual void Setup()
        {
            UIGame.Ins.AddBackUI(this);
            UIGame.Ins.PushBackAction(this, BackKey);
        }

        public virtual void BackKey()
        {

        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            //anim
        }

        public virtual void Close()
        {
            UIGame.Ins.RemoveBackUI(this);
            //anim
            gameObject.SetActive(false);
            if (IsDestroyOnClose)
            {
                Destroy(gameObject);
            }

        }
    }
}

