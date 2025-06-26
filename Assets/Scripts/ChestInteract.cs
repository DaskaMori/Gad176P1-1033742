using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class ChestController : MonoBehaviour
{
    [Header("Chest Sprites")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite priedSprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private float frameDelay = 0.1f;

    [Header("Key UI (assign per‐player)")]
    [SerializeField] private Image keyImageP1;
    [SerializeField] private Image keyImageP2;

    private SpriteRenderer spriteRenderer;
    private bool opened;
    private Dictionary<PlayerID, Image> keyImages;
    private List<PlayerMovement> playersInRange = new List<PlayerMovement>();

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;

        keyImages = new Dictionary<PlayerID, Image>
        {
            { PlayerID.One, keyImageP1 },
            { PlayerID.Two, keyImageP2 }
        };

        foreach (var img in keyImages.Values)
        {
            if (img != null)
                img.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        foreach (var player in playersInRange)
        {
            var binding = ControlsConfig.GetBinding(player.playerID);

            if (Input.GetKeyDown(binding.Interact) && !player.hasKey)
            {
                player.hasKey = true;
                ShowKeyUI(player.playerID);
                Debug.Log($"Player {player.playerID} picked up the key!");
                
                if (!opened)
                    StartCoroutine(OpenSequence());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null && !playersInRange.Contains(player))
            playersInRange.Add(player);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
            playersInRange.Remove(player);
    }

    private void ShowKeyUI(PlayerID playerID)
    {
        if (keyImages.TryGetValue(playerID, out var img) && img != null)
            img.gameObject.SetActive(true);
    }

    private IEnumerator OpenSequence()
    {
        opened = true;

        spriteRenderer.sprite = priedSprite;
        yield return new WaitForSeconds(frameDelay);

        spriteRenderer.sprite = openSprite;
    }
}
