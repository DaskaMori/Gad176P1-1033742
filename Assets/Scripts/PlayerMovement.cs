using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(Health))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Setup")]
    public PlayerID playerID;

    [Header("Inventory")]
    public bool hasKey = false;

    [Header("Grid Movement")]
    public float moveSpeed = 2f;
    public float tileSize  = 0.16f;

    [Header("Collision Tilemaps")]
    public Tilemap collision;

    private ControlsBinding binding;
    private bool isMoving = false;
    [SerializeField] public bool isAlive  = true;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        binding = ControlsConfig.GetBinding(playerID);
    }

    void Update()
    {
        if (!isAlive || isMoving) return;

        Vector3 dir = Vector3.zero;
        if (Input.GetKeyDown(binding.Up))    dir = Vector3.up;
        if (Input.GetKeyDown(binding.Down))  dir = Vector3.down;
        if (Input.GetKeyDown(binding.Left))  dir = Vector3.left;
        if (Input.GetKeyDown(binding.Right)) dir = Vector3.right;

        if (dir != Vector3.zero && !IsBlocked(dir))
        {
            spriteRenderer.flipX = dir.x < 0;
            StartCoroutine(MoveOneTile(dir));
        }

        if (Input.GetKeyDown(binding.Interact)) 
            Interact();
    }

    private bool IsBlocked(Vector3 direction)
    {
        Vector3 worldDest = transform.position + direction * tileSize;
        Vector3Int cellPos = collision.WorldToCell(worldDest);
        return collision.HasTile(cellPos);
    }

    private IEnumerator MoveOneTile(Vector3 direction)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos   = startPos + direction * tileSize;
        float duration   = tileSize / moveSpeed;
        float elapsed    = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }

    private void Interact()
    {
        Debug.Log($"Player {playerID} pressed Interact at {transform.position}");
    }
}
