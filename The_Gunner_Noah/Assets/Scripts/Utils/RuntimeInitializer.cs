using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class RuntimeInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeBeforeSceneLoad()
        {
#if UNITY_EDITOR
            string currentScenePath = SceneManager.GetActiveScene().path;

            if (currentScenePath != SceneLoaderConst.BootstrapscenePath)
            {
                string previousScene = currentScenePath;
                EditorPrefs.SetString(SceneLoaderConst.PreviousSceneKey, previousScene);

                string sceneName = System.IO.Path.GetFileNameWithoutExtension(previousScene);
                SceneLoader.SceneToLoadOverride = sceneName;

                // Single 모드로 Bootstrap 로드 (기존 씬 언로드)
                SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneLoaderConst.BootstrapscenePath), LoadSceneMode.Single);
            }
#endif
        }
    }
}