using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Health))]
public class HazardOnTile : MonoBehaviour
{
    [Tooltip("The Tilemap that holds your deadly spike tiles")]
    public Tilemap hazardMap;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        if (hazardMap == null)
            Debug.LogError("Assign your Hazard Tilemap to hazardMap!");
    }

    void Update()
    {
        Vector3Int cellPos = hazardMap.WorldToCell(transform.position);
        if (hazardMap.HasTile(cellPos))
        {
            health.TakeDamage(health.currentHealth);
            Debug.Log("Stepped on a spike tile, insta-death!");
        }
    }
}