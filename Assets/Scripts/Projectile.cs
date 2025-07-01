using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 30;
    public float lifeTime = 3f;

    private Rigidbody2D rb;
    private Collider2D  projCollider;
    private EnemyBase   owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        projCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(EnemyBase e)
    {
        owner = e;

        foreach (var c in e.GetComponentsInChildren<Collider2D>())
        {
            Physics2D.IgnoreCollision(projCollider, c);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        dir.Normalize();
        rb.velocity = dir * speed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var dmg = coll.GetComponent<IDamageable>();
        if (dmg != null && coll.GetComponent<EnemyBase>() != owner)
        {
            dmg.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (!coll.isTrigger)
            Destroy(gameObject);
    }
}