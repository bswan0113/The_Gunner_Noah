using System.Collections;
using System.Collections.Generic;
using Features.Common;
using Features.Tutorial;
using TMPro;
using UnityEngine;

namespace Core.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance{get ; private set;}

        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private TextMeshProUGUI tutorialText;

        private HashSet<TutorialData> _completedTutorials = new HashSet<TutorialData>();
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

        public IEnumerator ShowTutorial(TutorialData tutorialData)
        {
            Debug.Log($"Tutorial requested: {tutorialData.name}, Already completed: {_completedTutorials.Contains(tutorialData)}");
            if (_completedTutorials.Contains(tutorialData))
            {
                yield break;
            }
            _completedTutorials.Add(tutorialData);
            tutorialPanel.SetActive(true);
            tutorialText.text = tutorialData.tutorialText;
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(tutorialData.duration);
            Time.timeScale = 1f;
            tutorialPanel.SetActive(false);
        }
    }
}