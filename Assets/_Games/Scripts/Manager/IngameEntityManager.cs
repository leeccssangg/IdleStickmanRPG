using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngameEntityManager 
{
    public static IngameEntityManager Ins{ get; } = new IngameEntityManager();

    internal class EntityMap : Dictionary<double, IngameObject> { }
    private EntityMap m_IngameObjectMap = new EntityMap();
    private EntityMap m_IngameCharacterTeam1 = new EntityMap();
    private EntityMap m_IngameCharacterTeam2 = new EntityMap();

    private Dictionary<int, string> m_NameObjectMap = new Dictionary<int, string>();


    public IngameObject GetEntityFromID(int id ) {
        IngameObject ent = null;
        if(m_IngameObjectMap.TryGetValue(id, out ent)) {
            return ent;
        }
        return null;
    }
    public void RegisterEntity( IngameObject newEntity ) {
        if (!m_IngameObjectMap.ContainsKey(newEntity.ID)) {
            m_IngameObjectMap.Add(newEntity.ID, newEntity);
        }
        else {
            m_IngameObjectMap[newEntity.ID] = newEntity;
        }
        if (!m_NameObjectMap.ContainsKey(newEntity.ID)) {
            m_NameObjectMap.Add(newEntity.ID,newEntity.name);
        }
        else {
            m_NameObjectMap[newEntity.ID] = newEntity.name;
        }
    }
    public void UnRegisterEntity(IngameObject entity ) {
        RemoveEntity(entity);
        entity.ID = -1;
    }
    public void RemoveEntity( IngameObject entity ) {
        if (entity == null) {
            return;
        }
        if (m_IngameObjectMap.ContainsKey(entity.ID)) {
            m_IngameObjectMap.Remove(entity.ID);
        }
        if (m_NameObjectMap.ContainsKey(entity.ID)) {
            m_NameObjectMap.Remove(entity.ID);
        }
        if (m_IngameCharacterTeam1.ContainsKey(entity.ID)) {
            m_IngameCharacterTeam1.Remove(entity.ID);
        }
        if (m_IngameCharacterTeam2.ContainsKey(entity.ID)) {
            m_IngameCharacterTeam2.Remove(entity.ID);
        }
    }
    public void RegisterTeam(IngameObject newEntity,IngameTeam ingameTeam) {
        switch (ingameTeam) {
            case IngameTeam.Team1:
                if (!m_IngameCharacterTeam1.ContainsKey(newEntity.ID)) {
                    m_IngameCharacterTeam1.Add(newEntity.ID,newEntity);
                } else {
                    m_IngameCharacterTeam1[newEntity.ID] = newEntity;
                }
                break;
            case IngameTeam.Team2:
                if (!m_IngameCharacterTeam2.ContainsKey(newEntity.ID)) {
                    m_IngameCharacterTeam2.Add(newEntity.ID, newEntity);
                } else {
                    m_IngameCharacterTeam2[newEntity.ID] = newEntity;
                }
                break;
        }
    }
    public List<IngameObject> GetAllBullet() {
        List<IngameObject> list = new List<IngameObject>();
        foreach (int key in m_IngameObjectMap.Keys) {
            IngameObject c;
            if (m_IngameObjectMap.TryGetValue(key, out c)) {
                if (c != null && c.m_IngameType == IngameType.BULLET) {
                    list.Add(c);
                }
            }
        }
        return list;
    }
    public List<IngameObject> GetAllEnemyInRange( IngameTeam team,Vector3 center, float range) {
        EntityMap dic;
        if (team == IngameTeam.Team1) {
            dic = m_IngameCharacterTeam2;
        } else {
            dic = m_IngameCharacterTeam1;
        }
        List<IngameObject> list = new List<IngameObject>();
        foreach (int key in dic.Keys) {
            IngameObject c;
            if (dic.TryGetValue(key, out c)) {
                if (c != null && !c.IsDead()) {
                    Vector3 cPos = c.transform.position;
                    if ((Vector3.Distance(center,cPos)) <= range) {
                        if(!list.Contains(c)) list.Add(c);
                    }
                }
            }
        }
        return list;
    }
    public List<IngameObject> GetAllEnemyInRange( IngameTeam team, Vector3 from, Vector3 to ) {
        EntityMap dic;
        if (team == IngameTeam.Team1) {
            dic = m_IngameCharacterTeam2;
        } else {
            dic = m_IngameCharacterTeam1;
        }
        List<IngameObject> list = new List<IngameObject>();
        foreach (int key in dic.Keys) {
            IngameObject c;
            if (dic.TryGetValue(key, out c)) {
                if (c != null && !c.IsDead()) {
                    Vector3 cPos = c.transform.position;
                    if (Utilss.IsInRange(cPos.x, from.x, to.x)) {
                        list.Add(c);
                    }
                }
            }
        }
        return list;
    }
    public IngameObject GetNearestEnemy(Transform finder, IngameTeam characterTeam ) {
        switch (characterTeam) {
            case IngameTeam.Team1:
                return GetNearestCharacterTeam2(finder.position);
            case IngameTeam.Team2:
                return GetNearestCharacterTeam1(finder.position);
        }
        return null;
    }
    public IngameObject GetNearestCharacterTeam2(Vector3 finderPos ) {
        IngameObject nearestCharacter = null;
        float nearestDistance = 9999999;
        foreach(int key in m_IngameCharacterTeam2.Keys) {
            IngameObject c;
            if(m_IngameCharacterTeam2.TryGetValue(key, out c)) {
                if(c != null && !c.IsDead() && !c.IsHideOnRadar && !c.IsOutCamera()) {
                    Vector3 cPos = c.Transform.position;
                    float distance = Vector3.Distance(cPos, finderPos);
                    if(distance < nearestDistance) {
                        nearestCharacter = c;
                        nearestDistance = distance;
                    }
                }
            }
        }
        return nearestCharacter;
    }
    public IngameObject GetNearestCharacterTeam1( Vector3 finderPos ) {
        IngameObject nearestCharacter = null;
        float nearestDistance = 9999999;
        foreach (int key in m_IngameCharacterTeam1.Keys) {
            IngameObject c;
            if (m_IngameCharacterTeam1.TryGetValue(key, out c)) {
                if (c != null && !c.IsDead() && !c.IsOutCamera()) {
                    Vector3 cPos = c.transform.position;
                    float distance = (cPos - finderPos).magnitude;
                    if (distance < nearestDistance) {
                        nearestCharacter = c;
                        nearestDistance = distance;
                    }
                }
            }
        }
        return nearestCharacter;
    }
    public IngameObject GetRandomEnemy() {
        IngameObject enemy = null;
        if( m_IngameCharacterTeam2.Count == 0) return enemy;
        int key = (int)m_IngameCharacterTeam2.Keys.ElementAt(Random.Range(0,m_IngameCharacterTeam2.Count));
        if (m_IngameCharacterTeam2.TryGetValue(key, out var c)) {
            if (c != null && !c.IsDead() && !c.IsOutCamera()) {
                enemy = c;
            }
        }
        return enemy;
    }
    public List<IngameObject> GetRandomEnemies( int amount, IngameTeam ingameTeam ) {
        switch (ingameTeam) {
            case IngameTeam.Team1:
                return GetRandomCharactersTeam2(amount);
            case IngameTeam.Team2:
                return GetRandomCharactersTeam1(amount);
        }
        return new List<IngameObject>();
    }
    public List<IngameObject> GetRandomCharactersTeam1( int amount ) {
        List<IngameObject> list = new List<IngameObject>();
        Dictionary<double, IngameObject>.KeyCollection keys = m_IngameCharacterTeam1.Keys;
        foreach (int key in keys) {
            IngameObject c;
            if (m_IngameCharacterTeam1.TryGetValue(key, out c)) {
                if (!c.IsDead()) {
                    list.Add(c);
                }
            }
        }
        list.Shuffle();
        while (list.Count > amount) {
            list.RemoveAt(0);
        }
        return list;
    }
    public List<IngameObject> GetAllEnemies(IngameTeam ingameTeam ) {
        switch (ingameTeam) {
            case IngameTeam.Team1:
                return GetAllCharacterTeam2();
            case IngameTeam.Team2:
                return GetAllCharacterTeam1();
        }
        return new List<IngameObject>();
    }
    public List<IngameObject> GetRandomCharactersTeam2( int amount ) {
        List<IngameObject> list = new List<IngameObject>();
        Dictionary<double, IngameObject>.KeyCollection keys = m_IngameCharacterTeam2.Keys;
        foreach (int key in keys) {
            IngameObject c;
            if (m_IngameCharacterTeam2.TryGetValue(key, out c)) {
                if (!c.IsDead() && !c.IsOutCamera() && c.gameObject.activeInHierarchy) {
                    list.Add(c);
                }
            }
        }
        list.Shuffle();
        while (list.Count > amount) {
            list.RemoveAt(0);
        }
        return list;
    }
    public List<IngameObject> GetAllCharacterTeam1() {
        List<IngameObject> list = new List<IngameObject>();
        Dictionary<double, IngameObject>.KeyCollection keys = m_IngameCharacterTeam1.Keys;
        foreach (int key in keys) {
            IngameObject c;
            if (m_IngameCharacterTeam1.TryGetValue(key, out c)) {
                if (!c.IsDead() && !c.IsOutCamera()) {
                    list.Add(c);
                }
            }
        }
        return list;
    }
    public List<IngameObject> GetAllCharacterTeam2() {
        List<IngameObject> list = new List<IngameObject>();
        Dictionary<double, IngameObject>.KeyCollection keys = m_IngameCharacterTeam2.Keys;
        foreach (int key in keys) {
            IngameObject c;
            if (m_IngameCharacterTeam2.TryGetValue(key, out c)) {
                if (!c.IsDead() && !c.IsOutCamera()) {
                    if(!list.Contains(c)) list.Add(c);
                }
            }
        }
        return list;
    }
}
