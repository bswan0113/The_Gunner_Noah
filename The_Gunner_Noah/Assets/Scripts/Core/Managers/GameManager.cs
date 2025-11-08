using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance{get ; private set;}

        [SerializeField] GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI clearText;
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            Time.timeScale = 1;
        }

        public void GameOver()
        {
            clearText.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void GameClear()
        {
            gameOverPanel.SetActive(true);
            clearText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void Quit()
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void Retry()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}