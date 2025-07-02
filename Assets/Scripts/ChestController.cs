using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ChestController : MonoBehaviour
{
    [Header("Chest Sprites")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite priedSprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private float frameDelay = 0.1f;

    private SpriteRenderer spriteRenderer;
    private bool opened;
    private List<PlayerMovement> playersInRange = new List<PlayerMovement>();

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (opened) return;

        foreach (var player in playersInRange)
        {
            var bind = ControlsConfig.GetBinding(player.playerID);
            if (Input.GetKeyDown(bind.Interact))
            {
                GameManager.Instance.AddKey(player.playerID);
                Debug.Log($"Player {player.playerID} picked up the key!");
                StartCoroutine(OpenSequence());
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerMovement>();
        if (player != null && !playersInRange.Contains(player))
            playersInRange.Add(player);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerMovement>();
        if (player != null)
            playersInRange.Remove(player);
    }

    private IEnumerator OpenSequence()
    {
        opened = true;
        if (priedSprite != null)
            spriteRenderer.sprite = priedSprite;
        yield return new WaitForSeconds(frameDelay);
        if (openSprite != null)
            spriteRenderer.sprite = openSprite;
    }
}
