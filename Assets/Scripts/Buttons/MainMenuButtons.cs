using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buttons
{
    public class MainMenuButtons : MonoBehaviour
    {
        [Header("Scenes")]
        public string gameScene = "GameLevel";

        [Header("Panels")]
        public GameObject titlePanel;
        public GameObject instructionsPanel;

        void Awake()
        {
            titlePanel?.SetActive(true);
            instructionsPanel?.SetActive(false);
        }

        public void OnStartClicked()
        {
            SceneManager.LoadScene(gameScene);
        }

        public void OnInstructionsClicked()
        {
            titlePanel?.SetActive(false);
            instructionsPanel?.SetActive(true);
        }

        public void OnBackFromInstructions()
        {
            instructionsPanel?.SetActive(false);
            titlePanel?.SetActive(true);
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