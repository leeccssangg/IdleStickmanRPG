using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManger : SingletonFree<CameraManger>{
    public Transform m_CameraPos;
    public Transform m_Target;
    public float helpCameraFollowFaster = 4;
    public float CamXOffeset;
    public CameraShaker m_CameraSkake;
    
    private static Camera m_MainCamera;
    public static Camera MainCamera {
        get {
            if (m_MainCamera == null) {
                m_MainCamera = Camera.main;
            }
            return m_MainCamera;
        }
    }
    void FixedUpdate(){
        if(m_Target == null) return;
        var position = this.transform.position;
        position = new Vector3(Mathf.Lerp(position.x, m_Target.position.x - CamXOffeset, Time.deltaTime * helpCameraFollowFaster),
                        0.1f, position.z);
        transform.position = position;
    }
    private void Update(){
        IngameManager.Ins.Background.UpdateBGPosition(transform);
    }
    public void ShakeCamera(float duration = 0.25f){
        m_CameraSkake.ShakeCamera(duration);
    }
    public static bool IsOutOfCamera(Vector3 pos){
        Vector3 screenPoint = MainCamera.WorldToViewportPoint(pos);
        bool onScreen = screenPoint is{
            x: > 0 and < 1,
            y: > 0 and < 1
        };
        return !onScreen;
    }
    public Vector3 ScreenPoint(Vector3 pos){
        return MainCamera.WorldToViewportPoint(pos);
    }
}