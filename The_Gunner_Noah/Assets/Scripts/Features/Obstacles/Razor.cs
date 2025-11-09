using Features.Common;
using UnityEngine;

namespace Features.Obstacles
{
    public class Razor : MonoBehaviour
    {
        [SerializeField] private float damage = 99f;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}
