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
        private bool _mouseSettingLock;
        public bool MouseSettingLock => _mouseSettingLock;
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
            _mouseSettingLock = false;
        }

        public void GameOver()
        {
            ReleaseMouse();
            clearText.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void GameClear()
        {
            ReleaseMouse();
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
            _mouseSettingLock = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Retry()
        {
            _mouseSettingLock = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        private void ReleaseMouse()
        {
            _mouseSettingLock = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}