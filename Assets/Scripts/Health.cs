using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour, IDamageable, IStunnable
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Tooltip("Which character this is (One, Two or Monster).")]
    public PlayerID playerID;

    private bool isStunned;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        RefreshUI();
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        RefreshUI();
        if (currentHealth == 0) Die();
    }

    private void RefreshUI()
    {
        if (playerID == PlayerID.One || playerID == PlayerID.Two)
        {
            GameUIManager.Instance
                ?.UpdateHealth((int)playerID, currentHealth, maxHealth);
        }
    }

    public void Stun(float seconds)
    {
        if (isStunned || currentHealth <= 0) return;
        StartCoroutine(StunRoutine(seconds));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;

        var pm = GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.StopAllCoroutines();
            pm.enabled = false;
        }

        var eb = GetComponent<EnemyMovementBase>();
        if (eb != null)
        {
            eb.StopAllCoroutines();
            eb.enabled = false;
        }

        yield return new WaitForSeconds(duration);

        if (pm != null) pm.enabled = true;
        if (eb != null) eb.enabled = true;

        isStunned = false;
    }

    
    public void ResetHealth()
    {
        StopAllCoroutines();
        currentHealth = maxHealth;
        isStunned     = false;

        var pm = GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.enabled = true;      
            pm.isAlive = true;      
        }

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.simulated = true;

        if (pm != null)
            GameUIManager.Instance.UpdateHealth((int)pm.playerID, currentHealth, maxHealth);
    }

    private void Die()
    {
        var pm = GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.StopAllCoroutines();
            pm.isAlive = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}