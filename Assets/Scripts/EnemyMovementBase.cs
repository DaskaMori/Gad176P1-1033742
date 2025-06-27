using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class EnemyMovementBase : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.5f;
    public float tileSize  = 0.16f;

    [Header("Tilemaps")]
    public Tilemap collision;
    public Tilemap hazard;

    protected bool isMoving;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (collision == null)
        {
            var colObj = GameObject.Find("Collision");
            if (colObj != null)
                collision = colObj.GetComponent<Tilemap>();
        }
        if (hazard == null)
        {
            var hazObj = GameObject.Find("Hazard");
            if (hazObj != null)
                hazard = hazObj.GetComponent<Tilemap>();
        }
    }
    
    protected bool CanMoveTo(Vector3 dest)
    {
        if (collision != null)
        {
            var cell = collision.WorldToCell(dest);
            if (collision.HasTile(cell))
                return false;
        }
        if (hazard != null)
        {
            var hcell = hazard.WorldToCell(dest);
            if (hazard.HasTile(hcell))
                return false;
        }
        return true;
    }


    protected IEnumerator MoveOneTile(Vector2 dir)
    {
        if (isMoving) yield break;
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end   = start + (Vector3)dir * tileSize;
        if (!CanMoveTo(end))
        {
            isMoving = false;
            yield break;
        }

        float duration = tileSize / moveSpeed;
        float t = 0f;
        spriteRenderer.flipX = dir.x < 0;

        while (t < duration)
        {
            transform.position = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}