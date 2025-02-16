using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomGridLayout : GridLayoutGroup {
    RectTransform parentRect, rect;
    protected override void Awake() {
        rect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(parentRect.sizeDelta.x, rect.sizeDelta.y);
    }
    public override void CalculateLayoutInputHorizontal() {
        if (!gameObject.activeInHierarchy) return;
        if (constraint == Constraint.FixedColumnCount) {
            float width = rectTransform.rect.width - padding.left - padding.right - spacing.x * (constraintCount - 1);
            float size = width / constraintCount;
            float rate = size / m_CellSize.x;
            Vector2 newCellSize = m_CellSize * rate;
            m_CellSize = newCellSize;
        }
        base.CalculateLayoutInputHorizontal();
    }
   
}
