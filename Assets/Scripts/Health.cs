
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour, IHealth
{
    [Header("Health Settings")]
    public int maxHealth     = 100;
    public float stunDuration = 1f;

    private int currentHealth;
    private bool isStunned;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    public void Stun(float seconds)
    {
        if (isStunned) return;
        StartCoroutine(StunRoutine(seconds));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;

        var pm = GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        var eb = GetComponent<EnemyMovementBase>();
        if (eb != null) eb.enabled = false;

        yield return new WaitForSeconds(duration);

        if (pm != null) pm.enabled = true;
        if (eb != null) eb.enabled = true;

        isStunned = false;
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}