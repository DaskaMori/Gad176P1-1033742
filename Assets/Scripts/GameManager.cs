using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player References")]
    public PlayerMovement player1;
    public Transform      player1Spawn;
    public PlayerMovement player2;
    public Transform      player2Spawn;

    [Header("Rounds")] 
    public int maxRounds = 3;
    public int roundsToWin = 2;
    
    [Header("Win Condition")]
    public int keysToWin = 5;

    private int currentRound = 1;
    private Dictionary<PlayerID, int> roundWins = new();
    
    private Dictionary<PlayerID, int> keyCounts = new();
    
    [Header("Respawn Settings")]
    bool respawningP1 = false;
    bool respawningP2 = false;
    public float respawnDelay = 3f;
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
        var allPlayers = FindObjectsOfType<PlayerMovement>();

        /*
         * Requires the below code due to the scene reload. Had a bug that cause an error with the respawing function.
         * This error basically cleared the gamemanager of the player 1 and 2 game objects and their spawn locations.
         */
        player1 = null;
        foreach (var p in allPlayers)
        {
            if (p.playerID == PlayerID.One)
            {
                player1 = p;
                break;
            }
        }

        player2 = null;
        foreach (var p in allPlayers)
        {
            if (p.playerID == PlayerID.Two)
            {
                player2 = p;
                break;
            }
        }

        player1Spawn = GameObject.Find("SpawnPointP1")?.transform;
        player2Spawn = GameObject.Find("SpawnPointP2")?.transform;

        ResetRound();
        respawningP1 = respawningP2 = false;
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

    public void PlayerWinsRound(PlayerID player)
    {
        if (!roundWins.ContainsKey(player)) 
            roundWins[player] = 0;
        roundWins[player]++;

        if (roundWins[player] >= roundsToWin)
        {
            GameUIManager.Instance.ShowGameOverText((int)player);
            StartCoroutine(EndGameCoroutine(player));
        }
        else
        {
            GameUIManager.Instance.ShowWinText((int)player);
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
    
    void Update()
    {
        if (player1 != null && !player1.isAlive && !respawningP1)
        {
            respawningP1 = true;
            StartCoroutine(RespawnPlayer(player1, player1Spawn.position));
        }
        if (player2 != null && !player2.isAlive && !respawningP2)
        {
            respawningP2 = true;
            StartCoroutine(RespawnPlayer(player2, player2Spawn.position));
        }
    }
    
    private IEnumerator RespawnPlayer(PlayerMovement pm, Vector3 spawnPos)
    {
        yield return new WaitForSeconds(respawnDelay);

        pm.transform.position = spawnPos;

        var health = pm.GetComponent<Health>();
        if (health != null)
            health.ResetHealth();

        pm.ResetState();

        if (pm == player1) respawningP1 = false;
        else if (pm == player2) respawningP2 = false;
    }
}