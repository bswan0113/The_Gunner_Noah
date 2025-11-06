using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Core.Managers
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public SceneTransitionManager( )
        {
        }

        public void Initialize()
        {
            BsLogger.Log("[SceneTransitionManager] Awake");
        }

        public void FadeAndLoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}