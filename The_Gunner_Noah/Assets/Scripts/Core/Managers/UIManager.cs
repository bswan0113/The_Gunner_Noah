using TMPro;
using UnityEngine;

namespace Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance{get ; private set;}

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
    }
}