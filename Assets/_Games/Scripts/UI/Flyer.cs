using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class Flyer : MonoBehaviour {
    private Transform m_Transform;
    public Transform Transform {
        get {
            if(m_Transform == null) {
                m_Transform = transform;
            }
            return m_Transform;
        }
    }
    public delegate void FlyerCallback();
    public IEnumerator FlyToTarget(Vector3 target, FlyerCallback callback)
    {
        Debug.Log(target);
        Vector3 start = gameObject.transform.position;
        Vector3 end = target;

        float length = (end - start).magnitude;

        Vector3 up = (end - start).normalized * length / 6.0f;
        Vector3 right = Quaternion.AngleAxis(270, Vector3.forward) * up;

        Vector3 v1 = start + up;
        Vector3 v2 = v1 + up * 2;
        Vector3 v3 = v2 + up * 2;

        v1 = v1 + new Vector3(UnityEngine.Random.Range(-length / 5.0f, length / 5.0f), UnityEngine.Random.Range(-length / 8.0f, length / 8.0f), 0);
        v2 = v2 + new Vector3(UnityEngine.Random.Range(-length / 5.0f, length / 5.0f), UnityEngine.Random.Range(-length / 8.0f, length / 8.0f), 0);
        v3 = v3 + new Vector3(UnityEngine.Random.Range(-length / 5.0f, length / 5.0f), UnityEngine.Random.Range(-length / 8.0f, length / 8.0f), 0);

        Vector3[] next = new Vector3[] { start, start, v1, v2, v3, end, end };
        transform.DORotate(new Vector3(0,0,360f), 1.0f,RotateMode.FastBeyond360).SetLoops(1);
        transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        transform.DOPath(next, 1.0f,PathType.CatmullRom,PathMode.TopDown2D).SetEase(Ease.InQuad);// move it back to the start without an LTSpline

        yield return Yielders.Get(1.0f);

        if (callback != null) {
            callback();
        }

        yield return Yielders.EndOfFrame;
    }

    public void FlyToTargetOneSide(Transform target, FlyerCallback callback, bool rotate = true, int side = -1, float scaleTo = 1.0f) {
        StartCoroutine(coFlyToTargetOneSide(target, callback,  rotate, side, scaleTo));
    }
    IEnumerator coFlyToTargetOneSide(Transform target, FlyerCallback callback, bool rotate = true, int side = -1, float scaleTo = 1.0f)
    {
        float time1 = 1f;
        //float time2 = 0.50f;

        float y = UnityEngine.Random.Range(-1f, 1f);
        float x = UnityEngine.Random.Range(-1f, 1f);
        Vector3 target1 = new Vector3(x, y, 0) + transform.position;

        //time1 = (float)Math.Sqrt(x * x + y * y) / 3.0f;

        //StartCoroutine(coMoveToTarget(target1, time1));
        //yield return Yielders.Get(0.5f);

        StartCoroutine(coMoveToTarget(target, time1));
        yield return Yielders.Get(time1);

        SimplePool.Despawn(gameObject);
        if (callback != null) {
            callback();
        }
        yield return Yielders.EndOfFrame;
    }
    IEnumerator coMoveToTarget(Transform target, float time) {
        velocity = Vector3.zero;
        smoothTime = 0.35f;
        float num = 0;
        num += Time.deltaTime/time;
        float num1 = (target.position - Transform.position).magnitude;
        float z = Transform.position.z;
        while (num1>0.02f) {
            Vector3 p = target.position;
            p.z = z;
            Vector3 v = Vector3.SmoothDamp(Transform.position, p, ref velocity, smoothTime);
            Transform.position = v;
            num += Time.deltaTime / time;
            num1 = (target.position - Transform.position).magnitude;
            yield return Yielders.EndOfFrame;
        }
        Transform.position = target.position;
    }
    public void FlyToTargetOneSide(Vector3 target, FlyerCallback callback, bool rotate = true, int side = -1, float scaleTo = 1.0f) {
        StartCoroutine(coFlyToTargetOneSide(target, callback, rotate, side, scaleTo));
    }

    IEnumerator coFlyToTargetOneSide(Vector3 target, FlyerCallback callback, bool rotate = true, int side = -1, float scaleTo = 1.0f) {
        float time1 = 2.0f;
        float time2 = 1f;
        
        float y = UnityEngine.Random.Range(-1.0f, 1.0f);
        float x = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 target1 = new Vector3(x, y, 0) + transform.position;

        time1 = (float)Math.Sqrt(x * x + y * y) / 3.0f;

        //StartCoroutine(coMoveToTarget(target1, time1));
        //yield return Yielders.Get(time1);

        StartCoroutine(coMoveToTarget(target, time2));
        yield return Yielders.Get(time2);

        SimplePool.Despawn(gameObject);
        if (callback != null) {
            callback();
        }
        yield return Yielders.EndOfFrame;
    }
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 1f;
    IEnumerator coMoveToTarget(Vector3 target, float time) {
        velocity = Vector3.zero;
        smoothTime = 0.35f;
        float num = 0;
        num += Time.deltaTime / time;
        float num1 = (target - Transform.position).magnitude;
        while (num1 > 0.02f) {
            Vector3 v = Vector3.SmoothDamp(Transform.position, target, ref velocity, smoothTime);
            Transform.position = v;
            num += Time.deltaTime / time;
            num1 = (target - Transform.position).magnitude;
            yield return Yielders.EndOfFrame;
        }
        Transform.position = target;
    }

    public IEnumerator FlyToBottle(Vector3 target, float time)
    {
        Vector3 start = gameObject.transform.position;
        Vector3 end = target;

        float length = (end - start).magnitude;

        Vector3 up = (end - start).normalized * length / 4.0f;
        Vector3 right = Quaternion.AngleAxis(270, Vector3.forward) * up;

        Vector3 v1 = start + up + right * UnityEngine.Random.Range(1.0f, 1.5f);
        Vector3 v2 = (v1 + end) / 2;

        Vector3[] next = new Vector3[] { start, start, v1, v2, end, end };

        transform.DOLocalRotate( new Vector3(0,0,360), time);

        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
        transform.DOPath(next, time).SetEase(Ease.InOutQuad); // move it back to the start without an LTSpline

        yield return Yielders.Get(time);
        yield return Yielders.EndOfFrame;
    }

    //internal void FlyToTargetOneSide(object position, Action onDiamondReach, bool v) {
    //    throw new NotImplementedException();
    //}

    public void FlyToTargetType1(Vector3 target, FlyerCallback callback) {
        StartCoroutine(coFlyToTargetType1(target, callback));
    }

    IEnumerator coFlyToTargetType1(Vector3 target, FlyerCallback callback) {

        transform.DOMoveY(target.y, 1.5f).SetEase(Ease.InBack);
        transform.DOMoveX(target.x, 1.5f).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(1.5f);

        SimplePool.Despawn(gameObject);
        if (callback != null) {
            callback();
        }
        yield return Yielders.EndOfFrame;
    }

    public void FlyToTargetEXP(Vector3 target, FlyerCallback callback, bool rotate = true, int side = -1, float scaleTo = 1.0f, float range = 1f, float flyTime1 = 2f) {
        StartCoroutine(coFlyToTargetEXP(target, callback, rotate, side, scaleTo, range, flyTime1));
    }

    IEnumerator coFlyToTargetEXP(Vector3 target, FlyerCallback callback, bool rotate = true, int side = -1, float scaleTo = 1.0f, float range = 1f, float flyTime1 = 2f) {
        float time1 = 1.0f;
        float time2 = 0.5f;

        float y = UnityEngine.Random.Range(-range, range);
        float x = UnityEngine.Random.Range(-range, range);
        Vector3 target1 = new Vector3(x, y, 0) + transform.position;

        time1 = (float)Math.Sqrt(x * x + y * y) / flyTime1;

        transform.DOMove(target1, time1).SetEase(Ease.InOutQuad);
        yield return Yielders.Get(time1);
        yield return Yielders.Get(0.3f);
        if(rotate) {
            transform.DOLocalRotate( transform.localEulerAngles + new Vector3(0,0,360), 1.0f);
        }
        transform.DOMove(target, time2).SetEase(Ease.InQuad);
        yield return Yielders.Get(time2);

        SimplePool.Despawn(gameObject);
        if (callback != null) {
            callback();
        }
        yield return Yielders.EndOfFrame;
    }
}
