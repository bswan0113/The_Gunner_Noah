using UnityEngine;
using UnityEngine.UI;

namespace Features.Player
{
    public class Condition : MonoBehaviour
    {
        public float curValue;
        public float startValue = 100f;
        public float maxValue = 100f;
        public float passiveValue;
        public Image uiBar;

        void Start()
        {
            curValue = startValue;
        }

        void Update()
        {
            if (startValue > 0)
            {
                uiBar.fillAmount = GetPercentage();
            }
        }

        float GetPercentage()
        {
            return curValue / maxValue;
        }

        public void Add(float value)
        {
            curValue = Mathf.Min(curValue + value, maxValue);
        }

        public void Subtract(float value)
        {
            curValue = Mathf.Max(curValue - value, 0f);
        }
    }
}