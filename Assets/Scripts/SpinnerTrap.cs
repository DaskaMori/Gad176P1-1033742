using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpinnerTrap : MonoBehaviour
{
    [Tooltip("The blade object")]
    public Transform blade;

    [Tooltip("Rotation speed in degrees per second.")]
    public float spinSpeed = 180f;

    [Tooltip("Damage dealt on contact.")]
    public int damage = 1;

    void Update()
    {
        if (blade != null)
            blade.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var dmgTarget = other.GetComponent<IDamageable>();
        if (dmgTarget != null)
        {
            dmgTarget.TakeDamage(damage);
        }
    }
}
