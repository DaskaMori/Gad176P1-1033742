using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class BladeDamage : MonoBehaviour
{
    [Tooltip("Damage dealt on hit.")]
    public int damage = 50;

    void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var dmg = other.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
    }
}