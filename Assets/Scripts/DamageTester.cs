using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public Health health;
    public int damagePerHit = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            health.TakeDamage(damagePerHit);
        }
    }
}