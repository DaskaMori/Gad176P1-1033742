using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class EnemyBase : EnemyMovementBase, IStunnable
{
    [Header("Enemy Stats")] 
    public float detectRange = 5f;

    [Header("Line of Sight")] [Tooltip("Layers that block sight (e.g. collision walls)")]
    public LayerMask obstacleMask;

    protected Health health;        
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

        health = GetComponent<Health>();
        if (health == null)
            Debug.LogError($"{name} needs a Health component!");

        var pms = FindObjectsOfType<PlayerMovement>();
        players = new Transform[pms.Length];
        for (int i = 0; i < pms.Length; i++)
            players[i] = pms[i].transform;
    }

    protected virtual void Update()
    {
        if (health.currentHealth <= 0) return;

    }
    
    public virtual void Stun(float seconds)
    {
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    
}