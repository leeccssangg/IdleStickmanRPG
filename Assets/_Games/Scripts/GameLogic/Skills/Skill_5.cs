using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_5 : Skill{
    public ParticleSystem m_Effect;

    protected override void Attack(){
        SetFirePos();
        if(m_Effect){
            m_Effect.transform.position = new Vector3(){
                x = m_FirePos.position.x, y = m_Effect.transform.position.y, z = m_FirePos.position.z,
            };
            m_Effect.Play();
        }
        DoStartAttackEvent();
        List<IngameObject> enemies = IngameEntityManager.Ins.GetAllEnemies(IngameTeam.Team1);
        for(int i = 0; i < enemies.Count; i++){
            var enemy = enemies[i];
            DamageInfo damageInfo = new();
            enemy.OnHit(GetDamage(), enemy.Transform.position,damageInfo);
        }
    }
}