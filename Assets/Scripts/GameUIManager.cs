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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        string text = $"x{count}";
        if (playerIndex == 1)
            player1KeysCountText.text = text;
        else if (playerIndex == 2)
            player2KeysCountText.text = text;
        else
            Debug.LogWarning($"UpdateKeys: invalid index {playerIndex}");
    }
}