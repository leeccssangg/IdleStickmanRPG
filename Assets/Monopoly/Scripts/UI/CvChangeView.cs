using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CvChangeView : UICanvas
{
    [field: SerializeField] public Transform UnMask {get; private set;}
    public void SetupOnOpen(Vector3 startTarget, Vector3 endTarget, float delayStart, float delayClose, UnityAction onDuringChangeView, UnityAction onChangeViewComplete)
    {
        UnMask.localScale = Vector3.one * 50;
        UnMask.position = startTarget;
        UnMask.DOScale(Vector3.zero, 1f)
            .SetDelay(delayStart)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                UnMask.position = endTarget;
                onDuringChangeView?.Invoke();
            });
        UnMask.DOScale(Vector3.one * 50, 1f)
            .SetEase(Ease.InSine)
            .SetDelay(2f + delayStart)
            .OnComplete(() =>
            {
                onChangeViewComplete?.Invoke();
                Close(delayClose);
            });
    }

}
