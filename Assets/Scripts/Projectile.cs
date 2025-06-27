using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifeTime = 3f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private EnemyBase owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(EnemyBase e)
    {
        owner = e;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * (speed * Time.fixedDeltaTime));
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var health = coll.GetComponent<IHealth>();
        if (health != null && coll.gameObject != owner?.gameObject)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!coll.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}