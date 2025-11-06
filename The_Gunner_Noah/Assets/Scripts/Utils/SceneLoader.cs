using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string nextSceneName = "MainMenuScene";

        public static string SceneToLoadOverride;

        void Start()
        {
            Initialize().Forget();
        }

        async UniTaskVoid Initialize()
        {

            string sceneToLoad = SceneToLoadOverride ?? nextSceneName;
            SceneToLoadOverride = null;

            await SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            Scene loadedScene = SceneManager.GetSceneByName(sceneToLoad);
            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
            }
        }
    }
}