using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundManager:MonoBehaviour {
    public Transform m_Platform;
    public List<Transform> backgroundTransform;
    [SerializeField] private List<Sprite> m_BackGroundSpriteList = new ();
    [SerializeField] private List<SpriteRenderer> m_BackGroundList = new ();
    
    public void Init(){
        SetBackground();
    }
    public void SetPlatformPosition() {
        Vector3 v = m_Platform.position;
        v.x = IngameManager.Ins.Hero.PlayerTransform.position.x;
        m_Platform.position = v;
    }
    public void SetBackground() {
        var level = ProfileManager.PlayerData.LevelData.l;
        var index = (level - 1) % m_BackGroundSpriteList.Count;
        var sprite = m_BackGroundSpriteList[index];
        for(int i = 0; i < m_BackGroundList.Count; i++){
            m_BackGroundList[i].sprite = sprite;
        }
    }
    public void UpdateBGPosition(Transform target) {
        backgroundTransform.Sort(SortBGPositionASC);
        Vector3 v = target.position;
        Transform it = backgroundTransform[2];
        int count = backgroundTransform.Count;
        if(v.x < it.position.x - 7.15f) {
            backgroundTransform[count - 1].position = backgroundTransform[0].position - new Vector3(7.15f,0,0);
        } else if(v.x > it.position.x + 7.15f) {
            backgroundTransform[0].position = backgroundTransform[count - 1].position + new Vector3(7.15f,0,0);
        }
    }
    private static int SortBGPositionASC(Transform x,Transform y) {
        if(x.position.x < y.position.x) {
            return -1;
        } else if(x.position.x > y.position.x) {
            return 1;
        } else {
            return 0;
        }
    }

}
