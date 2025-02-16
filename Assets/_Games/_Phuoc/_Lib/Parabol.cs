using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Parabol : MonoBehaviour
{
    public Transform Begin;
    public Transform End;
    public Transform Tf;
    private float firingAngle = 45;//goc nem
    private float gravity = 20;// trong luc

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SimulateProjectile(Start));
    }

    IEnumerator SimulateProjectile(UnityAction callBack)
    {
        Tf.position = Begin.position;

        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(0.25f);

        // Move projectile to the position of throwing object + add some offset if needed.
        // Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Tf.position, End.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Tf.rotation = Quaternion.LookRotation(End.position - Tf.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Tf.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        callBack?.Invoke();
    }
}
