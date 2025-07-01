using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class EnemyBase : EnemyMovementBase, IDamageable, IStunnable
{
    [Header("Enemy Stats")] public int maxHealth = 3;
    public float detectRange = 5f;

    [Header("Line of Sight")] [Tooltip("Layers that block sight (e.g. collision walls)")]
    public LayerMask obstacleMask;

    protected int currentHealth;
    protected Transform[] players;

    protected bool HasLineOfSight(Vector3 dest)
    {
        var start = (Vector2)transform.position;
        var end = (Vector2)dest;
        var hit = Physics2D.Linecast(start, end, obstacleMask);
        return hit.collider == null;
    }

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
        var pms = FindObjectsOfType<PlayerMovement>();
        players = new Transform[pms.Length];
        for (int i = 0; i < pms.Length; i++)
            players[i] = pms[i].transform;
    }

    protected virtual void Update()
    {
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    public virtual void Stun(float seconds)
    {
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}