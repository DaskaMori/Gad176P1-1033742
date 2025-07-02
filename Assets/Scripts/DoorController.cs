using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class DoorController : MonoBehaviour
{
    [Header("Door Sprites")]
    [SerializeField] private Sprite closedLeftSprite;
    [SerializeField] private Sprite closedRightSprite;
    [SerializeField] private Sprite openLeftSprite;
    [SerializeField] private Sprite openRightSprite;
    [SerializeField] private float openDelay = 0.1f;

    private bool isOpen;
    private List<PlayerMovement> playersInRange = new List<PlayerMovement>();
    private SpriteRenderer leftRenderer;
    private SpriteRenderer rightRenderer;

    void Start()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            if (sr.sprite == closedLeftSprite) leftRenderer = sr;
            else if (sr.sprite == closedRightSprite) rightRenderer = sr;
        }

        if (leftRenderer != null) leftRenderer.sprite = closedLeftSprite;
        if (rightRenderer != null) rightRenderer.sprite = closedRightSprite;
        isOpen = false;
    }

    void Update()
    {
        if (isOpen) return;

        foreach (var player in playersInRange)
        {
            var bind = ControlsConfig.GetBinding(player.playerID);
            if (Input.GetKeyDown(bind.Interact))
            {
                int have = GameManager.Instance.GetKeyCount(player.playerID);
                int need = GameManager.Instance.keysToWin;
                if (have < need)
                {
                    GameUIManager.Instance.ShowMessage((int)player.playerID, $"Need {need - have} more keys");
                }
                else
                {
                    StartCoroutine(OpenAndWin(player));
                }
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var pm = other.GetComponentInParent<PlayerMovement>();
        if (pm != null && !playersInRange.Contains(pm))
            playersInRange.Add(pm);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var pm = other.GetComponentInParent<PlayerMovement>();
        if (pm != null)
            playersInRange.Remove(pm);
    }

    private IEnumerator OpenAndWin(PlayerMovement player)
    {
        isOpen = true;
        if (leftRenderer != null) leftRenderer.sprite = openLeftSprite;
        if (rightRenderer != null) rightRenderer.sprite = openRightSprite;

        yield return new WaitForSeconds(openDelay);

        GameManager.Instance.PlayerWinsRound(player.playerID);
    }
}
