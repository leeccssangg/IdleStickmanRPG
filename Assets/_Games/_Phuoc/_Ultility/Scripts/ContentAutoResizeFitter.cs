using UnityEngine;
using UnityEngine.UI;
namespace MyExtension
{
    public abstract class ContentAutoResizeFitter : LayoutGroup
    {
        [SerializeField] protected float m_Spacing = 0;
        /// <summary>
        /// The spacing to use between layout elements in the layout group.
        /// </summary>
        public float Spacing
        {
            get => m_Spacing;
            set => SetProperty(ref m_Spacing, value);
        }

        //protected void CallCalculateFromParent()
        //{
        //    if (!m_IsInitSuccess)
        //    {
        //        m_ParentAutoResizeFitter = rectTransform.parent.GetComponent<HorizontalAutoResizeFitter>();
        //        m_IsInitSuccess = true;
        //    }
        //    if (!Application.isPlaying || m_ParentAutoResizeFitter == null) return;

        //    m_ParentAutoResizeFitter.Calculate();
        //}
        //protected virtual void GetRectChildren()
        //{
        //    rectChildren.Clear();
        //    List<Component> toIgnoreList = ListPool<Component>.Get();
        //    for (int i = 0; i < rectTransform.childCount; i++)
        //    {
        //        RectTransform rect = rectTransform.GetChild(i) as RectTransform;
        //        if (rect == null || !rect.gameObject.activeSelf)
        //            continue;

        //        rect.GetComponents(typeof(ILayoutIgnorer), toIgnoreList);

        //        if (toIgnoreList.Count == 0)
        //        {
        //            rectChildren.Add(rect);
        //            continue;
        //        }

        //        for (int j = 0; j < toIgnoreList.Count; j++)
        //        {
        //            ILayoutIgnorer ignorer = (ILayoutIgnorer)toIgnoreList[j];
        //            if (!ignorer.ignoreLayout)
        //            {
        //                rectChildren.Add(rect);
        //                break;
        //            }
        //        }
        //    }
        //    ListPool<Component>.Release(toIgnoreList);
        //    m_Tracker.Clear();
        //}
        protected abstract void Calculate();

        //private ContentAutoResizeFitter m_ParentAutoResizeFitter;
        //private bool m_IsInitSuccess = false;



#if UNITY_EDITOR

        private int m_Capacity = 10;
        private Vector2[] m_Sizes = new Vector2[10];

        protected virtual void Update()
        {
            if (Application.isPlaying)
                return;

            int count = transform.childCount;

            if (count > m_Capacity)
            {
                if (count > m_Capacity * 2)
                    m_Capacity = count;
                else
                    m_Capacity *= 2;

                m_Sizes = new Vector2[m_Capacity];
            }

            // If children size change in editor, update layout (case 945680 - Child GameObjects in a Horizontal/Vertical Layout Group don't display their correct position in the Editor)
            bool dirty = false;
            for (int i = 0; i < count; i++)
            {
                if (transform.GetChild(i) is not RectTransform t || t.sizeDelta == m_Sizes[i]) continue;

                dirty = true;
                m_Sizes[i] = t.sizeDelta;
            }

            if (dirty)
                LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

#endif
    } 
}
