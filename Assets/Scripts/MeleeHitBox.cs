using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class MeleeHitBox : MonoBehaviour
{
    [Header("Attack Settings")]
    public float activeDuration = 0.15f;
    public float stunDuration  = 1f;

    public bool IsAttacking { get; private set; }

    private Collider2D col;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.isKinematic = true;
        col.isTrigger = true;

        sprite.enabled = false;
        col.enabled = false;
    }
    
    public void TriggerAttack(Vector3 direction, float tileSize)
    {
        if (!IsAttacking)
            StartCoroutine(DoAttack(direction, tileSize));
    }

    IEnumerator DoAttack(Vector3 dir, float tileSize)
    {
        IsAttacking = true;
        var origin = transform.parent.position;
        var target = origin + dir * tileSize;
        Debug.Log($"[MeleeHitBox] origin={origin}  dir={dir}  target={target}");

        transform.position = target;
        transform.position = target;
        transform.rotation = Quaternion.Euler(0,0, Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg);

        sprite.enabled = true;
        col.enabled = true;

        yield return new WaitForSeconds(activeDuration);

        sprite.enabled = false;
        col.enabled = false;

        transform.position = origin;
        transform.rotation = Quaternion.identity;
        IsAttacking = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == transform.root)
            return;
        
        var st = other.GetComponentInParent<IStunnable>();
        if (st != null)
        {
            Debug.Log($"HitBox stunned {other.name}");
            st.Stun(stunDuration);
        }
    }

    public void ResetState()
    {
        StopAllCoroutines();
        IsAttacking = false;
        col.enabled = false;
        sprite.enabled = false;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}