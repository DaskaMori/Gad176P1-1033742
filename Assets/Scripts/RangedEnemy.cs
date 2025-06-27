using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Turret Settings")]
    public GameObject projectilePrefab;
    public float fireInterval = 2f;
    public float fireRange    = 5f;

    private float fireTimer;

    protected override void Awake()
    {
        base.Awake();
        fireTimer = fireInterval;
    }

    protected override void Update()
    {
        Transform bestTarget = null;
        float   bestDist    = float.MaxValue;

        foreach (var p in players)
        {
            if (p == null) continue;
            float d = Vector2.Distance(transform.position, p.position);
            if (d <= fireRange && d < bestDist && HasLineOfSight(p.position))
            {
                bestDist    = d;
                bestTarget  = p;
            }
        }

        if (bestTarget != null)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Fire(bestTarget);
                fireTimer = fireInterval;
            }
        }
    }

    private void Fire(Transform target)
    {
        if (projectilePrefab == null) return;

        Vector2 dir = (target.position - transform.position).normalized;

        var go   = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetOwner(this);
            proj.SetDirection(dir);
        }
    }
}