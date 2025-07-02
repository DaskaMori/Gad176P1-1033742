using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player References")] public PlayerMovement player1;

    [Header("Rounds")] 
    public int maxRounds = 3;
    public int roundsToWin = 2;
    
    [Header("Win Condition")]
    public int keysToWin = 5;

    private int currentRound = 1;
    private Dictionary<PlayerID, int> roundWins = new();

    private Dictionary<PlayerID, int> keyCounts = new();
    
    void Start()
    {
        ResetRound();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public void AddKey(PlayerID player)
    {
        keyCounts.TryAdd(player, 0);
        keyCounts[player]++;
        GameUIManager.Instance.AddKey((int)player);
    }
    
    public int GetKeyCount(PlayerID player)
    {
        return keyCounts.TryGetValue(player, out var c) ? c : 0;
    }

    public void PlayerWinsRound(PlayerID winningPlayer)
    {
        if (!roundWins.ContainsKey(winningPlayer)) roundWins[winningPlayer] = 0;
        roundWins[winningPlayer]++;

        GameUIManager.Instance.ShowWinText((int)winningPlayer);

        if (roundWins[winningPlayer] >= roundsToWin)
        {
            StartCoroutine(EndGameCoroutine(winningPlayer));
        }
        else
        {
            StartCoroutine(NextRoundCoroutine());
        }
    }
    
    private IEnumerator NextRoundCoroutine()
    {
        yield return new WaitForSeconds(5f);
        currentRound++;
        ResetRound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private IEnumerator EndGameCoroutine(PlayerID winner)
    {
        yield return new WaitForSeconds(5f);
        GameUIManager.Instance.ShowGameOverText((int)winner);
        SceneManager.LoadScene("EndGame");
    }

    private void ResetRound()
    {
        keyCounts.Clear();
        GameUIManager.Instance.ResetKeys();
        GameUIManager.Instance.UpdateRound(currentRound);
    }
    
}