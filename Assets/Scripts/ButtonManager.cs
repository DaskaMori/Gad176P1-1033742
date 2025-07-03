using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "GameLevel";
    public string endGameScene = "EndGame";

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        UpdateVisibility(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVisibility(scene.name);
    }

    private void UpdateVisibility(string sceneName)
    {
        bool shouldShow = sceneName == mainMenuScene || sceneName == endGameScene;
        gameObject.SetActive(shouldShow);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(gameScene);
    }

   
}