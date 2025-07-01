using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BladeDamage : MonoBehaviour
{
    [Tooltip("How much damage each hit does.")]
    public int damage = 30;
    float reloadTimer;

    void Update()
    {
        if (reloadTimer > 0f)
            reloadTimer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var dmg = coll.GetComponent<IDamageable>();
        {
            dmg.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}