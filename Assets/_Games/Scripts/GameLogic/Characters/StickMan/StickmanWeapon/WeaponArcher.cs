using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArcher : StickmanWeapon{
    // public override Vector3 GetFireDirection() {
    //     IngameObject target = m_Owner.Target;
    //     if(target == null) {
    //         return transform.forward;
    //     } else {
    //         float dir = transform.position.x - target.Transform.position.x > 0 ? -1 : 1;
    //         Vector3 v = transform.position + Vector3.up * 0.55f + Vector3.right * dir * 1.5f;
    //         if(m_FirePos != null) {
    //             Vector3 direct = v - m_FirePos.position;
    //             return direct.normalized;
    //         } else {
    //             Vector3 direct = v - transform.position;
    //             return direct.normalized;
    //         }
    //     }
    // }
    public override Vector3 GetFireDirection() {
        IngameObject target = m_Owner.Target;
        if(target == null) {
            return transform.position + Vector3.up * 0.55f + Vector3.right  * 1.5f;
        } else {
            float dir = transform.position.x - target.Transform.position.x > 0 ? -1 : 1;
            var v = target.Transform.position + Vector3.up *0.5f;
            if(m_FirePos != null) {
                Vector3 direct = v - m_FirePos.position;
                return direct.normalized;
            } else {
                Vector3 direct = v - transform.position;
                return direct.normalized;
            }
        }
    }
    
}