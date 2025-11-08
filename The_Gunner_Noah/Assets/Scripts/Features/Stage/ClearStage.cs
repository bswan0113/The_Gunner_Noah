using System.Collections;
using Core.Managers;
using TMPro;
using UnityEngine;

namespace Features.Stage
{
    public class ClearStage : MonoBehaviour
    {
        private Collider _stageCollider;
        private bool _isPlayerPassed = false;

        [SerializeField] private float panelActivationDelay = 2f;
        void Awake()
        {
            _stageCollider = GetComponent<Collider>();
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !_isPlayerPassed)
            {
                _isPlayerPassed = true;
                if (_stageCollider != null)
                {
                    _stageCollider.isTrigger = false;
                    StartCoroutine(ShowClearPanelRoutine());
                }
            }
        }

        private IEnumerator ShowClearPanelRoutine()
        {
            yield return new WaitForSeconds(panelActivationDelay);
            GameManager.Instance.GameClear();
        }
    }
}