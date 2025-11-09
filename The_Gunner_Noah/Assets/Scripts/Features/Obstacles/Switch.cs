using UnityEngine;
using Utils;

namespace Features.Obstacles
{
    public class Switch : MonoBehaviour
    {
        [SerializeField] private Operated operated;
        void OnTriggerEnter(Collider other)
        {
            if (operated == null) return;
            operated.Action();
        }
    }
}