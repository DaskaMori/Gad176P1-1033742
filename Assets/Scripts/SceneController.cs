using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string gameLevelScene = "GameLevel";
    public string endGameScene   = "EndGame";

    [Header("Rounds")]
    public int maxRounds = 3;
    private int currentRound = 1;
    
    public void StartGame()
    {
        currentRound = 1;
        SceneManager.LoadScene(gameLevelScene);
    }


    public void CompleteRound()
    {
        currentRound++;
        if (currentRound <= maxRounds)
            SceneManager.LoadScene(gameLevelScene);
        else
            SceneManager.LoadScene(endGameScene);
    }

    
    public void ReturnToMenu() => SceneManager.LoadScene(mainMenuScene);

    
    public void QuitGame() => Application.Quit();
}
