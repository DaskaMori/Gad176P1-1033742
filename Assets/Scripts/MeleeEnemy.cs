using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    [Header("Melee Settings")]
    public float stunCooldown = 5f;
    public float specialChargeTime = 1.5f;
    public int specialDamage = 100;

    private Transform axe;
    private Collider2D axeCollider;
    private Dictionary<IHealth, float> lastStun = new Dictionary<IHealth, float>();

    private enum State { Idle, Chase, Charging }
    private State state = State.Idle;
    private Transform target;

    void Start()
    {
        axe = transform.Find("Axe");
        if (axe) axeCollider = axe.GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        if (isMoving) return;
        target = SelectNearestVisiblePlayer();
        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);
        if (state == State.Idle && dist <= detectRange) state = State.Chase;
        else if (state == State.Chase && (dist > detectRange || !HasLineOfSight(target.position))) state = State.Idle;

        if (state == State.Idle) RandomWalk();
        else if (state == State.Chase) StartCoroutine(MoveOneTile(DirectionTo(target.position)));
    }

    private Transform SelectNearestVisiblePlayer()
    {
        Transform best = null;
        float minDist = float.MaxValue;
        foreach (var p in players)
        {
            if (p == null) continue;
            float d = Vector2.Distance(transform.position, p.position);
            if (d < minDist && d <= detectRange && HasLineOfSight(p.position))
            {
                minDist = d;
                best = p;
            }
        }
        return best;
    }

    private Vector2 DirectionTo(Vector3 dest)
    {
        Vector2 diff = dest - transform.position;
        return Mathf.Abs(diff.x) > Mathf.Abs(diff.y)
            ? new Vector2(Mathf.Sign(diff.x), 0)
            : new Vector2(0, Mathf.Sign(diff.y));
    }

    private void RandomWalk()
    {
        Vector2 dir = Random.insideUnitCircle;
        dir = Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? new Vector2(Mathf.Sign(dir.x), 0) : new Vector2(0, Mathf.Sign(dir.y));
        if (dir != Vector2.zero) StartCoroutine(MoveOneTile(dir));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponentInParent<IHealth>();
        if (health != null)
            health.Stun(stunCooldown);
    }


    private IEnumerator SpecialAttack(IHealth targetHealth)
    {
        state = State.Charging;
        if (axeCollider) axeCollider.enabled = true;
        yield return new WaitForSeconds(specialChargeTime);
        targetHealth.TakeDamage(specialDamage);
        if (axeCollider) axeCollider.enabled = false;
        state = State.Idle;
    }
}