using Core.Managers;
using UnityEngine;
using Utils;

namespace Features.Tutorial
{
    public class TutorialTrigger : MonoBehaviour
    {
        public TutorialData tutorialData;
        private bool _hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            BsLogger.Log($"OnTriggerEnter: {other.name}");
            if (_hasTriggered) return;
            if (!other.CompareTag("Player")) return;

            _hasTriggered = true;
            StartCoroutine(TutorialManager.Instance.ShowTutorial(tutorialData));
        }
    }
}