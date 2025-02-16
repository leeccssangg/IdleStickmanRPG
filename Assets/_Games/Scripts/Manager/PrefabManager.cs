using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    private static PrefabManager m_Instance;
    public static PrefabManager Instance {
        get {
            return m_Instance;
        }
    }
    private Dictionary<string, GameObject> m_CharacterPrefabDic = new Dictionary<string, GameObject>();
    public List<GameObject> m_CharacterPrefabs = new List<GameObject>();
    [ShowInInspector]
    private Dictionary<string,GameObject> m_StickmanPrefaDic = new Dictionary<string,GameObject>();
    public List<GameObject> m_StickmansPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> m_SubCharacterPrefabDic = new Dictionary<string, GameObject>();
    public List<GameObject> m_SubCharacterPrefabs = new List<GameObject>();
    [ShowInInspector]
    private Dictionary<string, GameObject> m_IngameObjectPrefabDic = new Dictionary<string, GameObject>();
    public GameObject[] m_IngameObjectPrefabs;

    [ShowInInspector]
    private Dictionary<string, GameObject> m_EnemyPrefabDic = new Dictionary<string, GameObject>();
    public GameObject[] m_EnemyPrefabs;

    [ShowInInspector]
    private Dictionary<string, GameObject> m_BulletPrefabDic = new Dictionary<string, GameObject>();
    public GameObject[] m_BulletPrefabs;

    private void Awake() {
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
        InitPrefab();
    }

    public void InitPrefab() {
        for (int i = 0; i < m_IngameObjectPrefabs.Length; i++) {
            GameObject iPrefab = m_IngameObjectPrefabs[i];
            if (iPrefab == null) continue;
            string iName = iPrefab.name;
            try {
                m_IngameObjectPrefabDic.Add(iName, iPrefab);
            } catch (System.Exception) {
                continue;
            }
        }
        for(int i = 0;i < m_StickmansPrefabs.Count;i++) {
            GameObject iPrefab = m_StickmansPrefabs[i];
            m_StickmanPrefaDic.Add(iPrefab.name,iPrefab);
        }
        for (int i = 0; i < m_CharacterPrefabs.Count; i++) {
            GameObject iPrefab = m_CharacterPrefabs[i];
            m_CharacterPrefabDic.Add(iPrefab.name, iPrefab);
        }
        for (int i = 0; i < m_SubCharacterPrefabs.Count; i++) {
            GameObject iPrefab = m_SubCharacterPrefabs[i];
            m_SubCharacterPrefabDic.Add(iPrefab.name, iPrefab);
        }
        for(int i = 0;i < m_EnemyPrefabs.Length;i++) {
            GameObject iPrefab = m_EnemyPrefabs[i];
            if(iPrefab == null)
                continue;
            string iName = iPrefab.name;
            try {
                m_EnemyPrefabDic.Add(iName,iPrefab);
            } catch(System.Exception) {
                continue;
            }
        }
        for (int i = 0; i < m_BulletPrefabs.Length; i++) {
            GameObject iPrefab = m_BulletPrefabs[i];
            if (iPrefab == null) continue;
            string iName = iPrefab.name;
            try {
                m_BulletPrefabDic.Add(iName, iPrefab);
            } catch (System.Exception) {
                continue;
            }
        }
        //string demoBullet = "Bullet_1";
        //CreatePool(demoBullet, GetBulletPrefabByName(demoBullet), 20);
        //string starFallBullet = "Bullet_StarFall";
        //CreatePool(starFallBullet, GetBulletPrefabByName(starFallBullet), 5);
        //string bullet2 = "Bullet_Slash";
        //CreatePool(bullet2, GetBulletPrefabByName(bullet2), 20);
        //string effectImpact = "EffectSpark";
        //CreatePool(effectImpact, GetPrefabByName("Effect_Impact"), 20);
        //string effectSummon = "Effect_Summon";
        //CreatePool(effectSummon, GetPrefabByName(effectSummon), 20);
    }
    public void CreateEnemyPool(string name) {
        if (!SimplePool.IsHasPool(name)) {
            CreatePool(name, GetEnemyPrefabByName(name), 10);
        }
    }
    public GameObject GetPrefabByName(string name) {
        GameObject rPrefab = null;
        if (m_IngameObjectPrefabDic.TryGetValue(name, out rPrefab)) {
            return rPrefab;
        }
        return null;
    }
    public bool HasPool(string name) {
        return SimplePool.IsHasPool(name);
    }
    public void ClearPools() {
        SimplePool.Release();
    }
    public void ReleasePools(string name) {
        SimplePool.Release(name);
    }
    public void CreatePool(string name, GameObject prefab, int amount) {
        SimplePool.Preload(prefab, amount, name);
    }
    public GameObject SpawnPool(string name) {
        if (SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name, Vector3.zero, Quaternion.identity);
            return go;
        } else {
            GameObject prefab = GetPrefabByName(name);
            if (prefab != null) {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name);
                return go;
            }
        }
        return null;
    }
    public GameObject SpawnPool(string name, Vector3 pos) {
        if (SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        } else {
            GameObject prefab = GetPrefabByName(name);
            if (prefab != null) {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name, pos);
                return go;
            }
        }
        return null;
    }
    public GameObject SpawnEnemyPool(string name,Vector3 pos) {
        if (SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
            return go;
        } else {
            GameObject prefab = GetEnemyPrefabByName(name);
            if (prefab != null) {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnEnemyPool(name, pos);
                return go;
            }
        }
        return null;
    }
    public Stickman SpawnStickmanPool(string name, Vector3 pos) {
        if(SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name,pos,Quaternion.identity);
            return go.GetComponent<Stickman>();
        } else {
            GameObject prefab = GetStickmanPrefabName(name);
            if(prefab != null) {
                SimplePool.Preload(prefab,1,name);
                return SpawnStickmanPool(name,pos);
            }
        }
        return null;
    }
    public Enemy SpawnEnemy(string name,Vector3 pos) {
        if(SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name,pos,Quaternion.identity);
            return go.GetComponent<Enemy>();
        } else {
            GameObject prefab = GetEnemyPrefabByName(name);
            if(prefab != null) {
                SimplePool.Preload(prefab,1,name);
                return SpawnEnemy(name,pos);
            }
        }
        return null;
    }
    public Enemy SpawnEnemy(string name) {
        Vector3 pos = Vector3.zero;
        if(SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name,pos,Quaternion.identity);
            return go.GetComponent<Enemy>();
        } else {
            GameObject prefab = GetEnemyPrefabByName(name);
            if(prefab != null) {
                SimplePool.Preload(prefab,1,name);
                return SpawnEnemy(name,pos);
            }
        }
        return null;
    }
    public GameObject SpawnPool(GameObject prefab, Vector3 pos) {
        if (SimplePool.IsHasPool(prefab.name)) {
            GameObject go = SimplePool.Spawn(prefab.name, pos, Quaternion.identity);
            return go;
        } else {
            if (prefab != null) {
                SimplePool.Preload(prefab, 1, prefab.name);
                GameObject go = SpawnPool(prefab, pos);
                return go;
            }
        }
        return null;
    }
    public GameObject SpawnBulletPool(string name) {
        if (SimplePool.IsHasPool(name)) {
            GameObject go = SimplePool.Spawn(name, Vector3.zero, Quaternion.identity);
            return go;
        } else {
            GameObject prefab = GetBulletPrefabByName(name);
            if (prefab != null) {
                SimplePool.Preload(prefab, 1, name);
                GameObject go = SpawnPool(name);
                return go;
            }
        }
        return null;
    }
    public void DespawnPool(GameObject go) {
        SimplePool.Despawn(go);
    }
    public GameObject GetMasterCharacterPrefabByID(string name) {
        if (m_CharacterPrefabDic.ContainsKey(name)) {
            return m_CharacterPrefabDic[name];
        }
        return null;
    }
    public GameObject GetHeroPrefabByID(string name) {
        if (m_SubCharacterPrefabDic.ContainsKey(name)) {
            return m_SubCharacterPrefabDic[name];
        }
        return null;
    }
    public GameObject GetEnemyPrefabByName(string name) {
        if (m_EnemyPrefabDic.ContainsKey(name)) {
            return m_EnemyPrefabDic[name];
        }
        return null;
    }
    public GameObject GetStickmanPrefabName(string name) {
        if(m_StickmanPrefaDic.ContainsKey(name)) {
            return m_StickmanPrefaDic[name];
        }
        return null;
    }
    public GameObject GetBulletPrefabByName(string name) {
        if (m_BulletPrefabDic.ContainsKey(name)) {
            return m_BulletPrefabDic[name];
        }
        return null;
    }
    public GameObject GetWorldConfig(int world) {
        string name = "World_" + world;
        GameObject go = Resources.Load<GameObject>("Levels/" + name);
        return go;
    }
}


[System.Serializable]
public class PrefabByID {
    public int id;
    public GameObject prefab;
}