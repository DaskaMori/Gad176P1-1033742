using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    
    [Header("Health Bars")]
    public Slider player1HealthBar;
    public Text   player1HealthText;
    public Slider player2HealthBar;
    public Text   player2HealthText;

    [Header("Round Counter")]
    public Text roundCounterText;

    [Header("Keys Display")]
    public Image player1KeyIcon;
    public Text  player1KeysCountText;
    public Image player2KeyIcon;
    public Text  player2KeysCountText;
    
    [Header("Notifications")]
    public Text player1NotificationText;
    public Text player2NotificationText;
    public float notificationDuration = 2f;
    
    [Header("Win Texts")]
    public Text player1WinText;   
    public Text player2WinText;
    
    [Header("Game Over")]
    public Text gameOverText;
    
    private int[] keyCounts = new int[3];
    private Coroutine msgCoroutineP1;
    private Coroutine msgCoroutineP2;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    void Start()
    {
        player1KeyIcon.gameObject.SetActive(false);
        player2KeyIcon.gameObject.SetActive(false);
        UpdateKeys(1, 0);
        UpdateKeys(2, 0);
        
        player1NotificationText.gameObject.SetActive(false);
        player2NotificationText.gameObject.SetActive(false);
        
        player1WinText.gameObject.SetActive(false);
        player2WinText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }
    
    
    public void UpdateHealth(int playerIndex, int current, int max)
    {
        float t = Mathf.Clamp01((float)current / max);
        if (playerIndex == 1)
        {
            player1HealthBar.value = t;
            player1HealthText.text = $"{current} / {max}";
        }
        else if (playerIndex == 2)
        {
            player2HealthBar.value = t;
            player2HealthText.text = $"{current} / {max}";
        }
        else
        {
            Debug.LogWarning($"UpdateHealth called with invalid index {playerIndex}");
        }
    }

    public void UpdateRound(int currentRound)
    {
        roundCounterText.text = $"Round {currentRound}";
    }

    public void UpdateKeys(int playerIndex, int count)
    {
        if (playerIndex == 1)
            player1KeysCountText.text = $"x{count}";
        else if (playerIndex == 2)
            player2KeysCountText.text = $"{count}x";
    }

    public void AddKey(int playerIndex)
    {
        if (playerIndex < 1 || playerIndex > 2) return;
        keyCounts[playerIndex]++;
        if (playerIndex == 1) player1KeyIcon.gameObject.SetActive(true);
        if (playerIndex == 2) player2KeyIcon.gameObject.SetActive(true);
        UpdateKeys(playerIndex, keyCounts[playerIndex]);
    }
    
    public void ShowMessage(int playerIndex, string msg)
    {
        if (playerIndex == 1)
        {
            if (msgCoroutineP1 != null) StopCoroutine(msgCoroutineP1);
            msgCoroutineP1 = StartCoroutine(MessageRoutine(player1NotificationText, msg));
        }
        else
        {
            if (msgCoroutineP2 != null) StopCoroutine(msgCoroutineP2);
            msgCoroutineP2 = StartCoroutine(MessageRoutine(player2NotificationText, msg));
        }
    }

    private IEnumerator MessageRoutine(Text textComp, string msg)
    {
        textComp.text = msg;
        textComp.gameObject.SetActive(true);
        yield return new WaitForSeconds(notificationDuration);
        textComp.gameObject.SetActive(false);
    }
    
    public void ShowWinText(int playerIndex)
    {
        player1WinText.gameObject.SetActive(false);
        player2WinText.gameObject.SetActive(false);

        if (playerIndex == 1) player1WinText.gameObject.SetActive(true);
        else if (playerIndex == 2) player2WinText.gameObject.SetActive(true);
    }
    
    public void ShowGameOverText(int winnerIndex)
    {
        gameOverText.text = $"Player {winnerIndex} wins the game!";
        gameOverText.gameObject.SetActive(true);
    }
    
    public void ResetKeys()
    {
        keyCounts = new int[3];
        player1KeyIcon.gameObject.SetActive(false);
        player2KeyIcon.gameObject.SetActive(false);
        UpdateKeys(1, 0);
        UpdateKeys(2, 0);
    }
}
