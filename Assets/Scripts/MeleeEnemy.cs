using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float stunCooldown = 5f;
    public float aggroRadius  = 3f;

    private readonly Dictionary<IStunnable, float> lastStun = new Dictionary<IStunnable, float>();
    private enum State { Idle, Chase }
    private State state = State.Idle;
    private Transform target;
    private bool isStunned = false;

    protected override void Update()
    {
        if (isStunned) return;
        if (isMoving) return;

        bool inAggro = false;
        foreach (var p in players)
        {
            if (p != null && Vector2.Distance(transform.position, p.position) <= aggroRadius)
            {
                inAggro = true;
                break;
            }
        }
        if (!inAggro)
        {
            state = State.Idle;
            RandomWalk();
            return;
        }

        target = SelectNearestVisiblePlayer();
        if (target == null)
        {
            state = State.Idle;
            RandomWalk();
            return;
        }

        float dist = Vector2.Distance(transform.position, target.position);
        bool visible = HasLineOfSight(target.position);
        if (state == State.Idle && dist <= detectRange && visible)
            state = State.Chase;
        else if (state == State.Chase && (dist > detectRange || !visible))
            state = State.Idle;

        if (state == State.Idle)
            RandomWalk();
        else if (state == State.Chase)
            StartCoroutine(MoveOneTile(DirectionTo(target.position)));
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var stunnable = coll.collider.GetComponentInParent<IStunnable>();
        if (stunnable == null) return;

        float lastTime = lastStun.GetValueOrDefault(stunnable, -Mathf.Infinity);
        if (Time.time - lastTime < stunCooldown) return;

        stunnable.Stun(stunCooldown);
        lastStun[stunnable] = Time.time;

        StartCoroutine(SelfStun(7f));
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

    private void RandomWalk()
    {
        Vector2 dir = Random.insideUnitCircle;
        dir = Mathf.Abs(dir.x) > Mathf.Abs(dir.y)
            ? new Vector2(Mathf.Sign(dir.x), 0)
            : new Vector2(0, Mathf.Sign(dir.y));
        if (dir != Vector2.zero)
            StartCoroutine(MoveOneTile(dir));
    }

    private Vector2 DirectionTo(Vector3 dest)
    {
        Vector2 diff = dest - transform.position;
        return Mathf.Abs(diff.x) > Mathf.Abs(diff.y)
            ? new Vector2(Mathf.Sign(diff.x), 0)
            : new Vector2(0, Mathf.Sign(diff.y));
    }

    private IEnumerator SelfStun(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
}

