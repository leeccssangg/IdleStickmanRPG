using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public Vector3 originalPos;
    private bool IsPause;

    void Awake() {
        if (camTransform == null) {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    public void ShakeCamera() {
        shakeDuration = 0.25f;
    }
    public void ShakeCamera(float duration) {
        shakeDuration = duration;
    }
    public void Pause() {
        IsPause = true;
    }
    public void Resume() {
        IsPause = false;
    }
    public void SetOriginalPos(Vector3 v) {
        originalPos = v;
    }
    void OnEnable() {
        originalPos = camTransform.localPosition;
    }

    void Update() {
        if (IsPause) return;
        if (shakeDuration > 0) {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        } else {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}