using UnityEngine;
using UnityEngine.Serialization;

public class Bl_Skill21 : Bullet{
    public IngameObject m_Enemy;
    private Transform m_OriginTransform;
    private Vector2 m_Offset;
    public override void FireChase(Transform originTrans){
        m_LifeTime = 20;
        m_OriginTransform = originTrans;
        m_Offset = Vector2.right * -1.5f;
        var dir = Quaternion.AngleAxis(Random.Range(-90, 90),Vector3.forward ) * Vector3.up;
        Transform.localEulerAngles = new Vector3(0, 0, Random.Range(-90, 90));
        StateMachine.ChangeState(BulletChaseTargetState.Instance);
    }
    public override void OnChaseExecute(){
        if(m_Enemy == null || m_Enemy.IsDead()){
            m_Enemy = IngameEntityManager.Ins.GetRandomEnemy();
        }
        if(m_Enemy == null || m_Enemy.IsDead()){
            m_TargetPos = (Vector2)m_OriginTransform.position + m_Offset;
        }else
            m_TargetPos = m_Enemy.Transform.position;
        
        
        Vector2 dir = m_TargetPos - m_Rigidbody.position;
        dir.Normalize();
        float rotateAmount = Vector3.Cross(dir, transform.up).z;
        Speed += m_Accel * Time.deltaTime;
        Speed = Mathf.Clamp(Speed, 0, m_MaxSpeed);
        m_Rigidbody.angularVelocity = -rotateAmount * Speed * 100f;
        m_Rigidbody.velocity = transform.up * Speed;
    }
}