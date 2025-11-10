using System.Collections;
using Core.Managers;
using UnityEngine;

namespace Features.Environment
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private Light lightningLight;
        [SerializeField] private float minInterval = 3f;
        [SerializeField] private float maxInterval = 10f;

        private void Start()
        {
            lightningLight.enabled = false;
            StartCoroutine(LightningEffect());
        }

        private IEnumerator LightningEffect()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

                for (int i = 0; i < Random.Range(2, 4); i++)
                {
                    lightningLight.enabled = true;
                    lightningLight.intensity = Random.Range(0.8f, 1.5f);
                    yield return new WaitForSeconds(0.05f);
                    lightningLight.enabled = false;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
                AudioManager.Instance.PlaySfx(AudioManager.Instance.thunder);
            }
        }
    }
}