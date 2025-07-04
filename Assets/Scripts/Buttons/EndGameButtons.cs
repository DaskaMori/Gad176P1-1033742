using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buttons
{
    public class EndGameButtons : MonoBehaviour
    {
        [Header("Scenes")]
        public string gameScene     = "GameLevel";
        public string mainMenuScene = "MainMenu";

        public void OnBackToMenuClicked()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}