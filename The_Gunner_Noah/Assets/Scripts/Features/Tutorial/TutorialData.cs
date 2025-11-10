using UnityEngine;

namespace Features.Common
{
    [CreateAssetMenu(fileName = "Tutorial_", menuName = "Tutorial")]
    public class TutorialData : ScriptableObject
    {
        [TextArea (3,5)]
        public string tutorialText;
        public float duration;
    }
}