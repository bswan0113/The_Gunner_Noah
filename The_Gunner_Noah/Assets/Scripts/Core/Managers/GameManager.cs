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
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Time.timeScale = 1;
            AudioManager.Instance.PlayBgm(AudioManager.Instance.bgm);
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
            AudioManager.Instance.PlaySfx(AudioManager.Instance.clear);
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

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            gameOverPanel.SetActive(false);
            Time.timeScale = 1;
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}