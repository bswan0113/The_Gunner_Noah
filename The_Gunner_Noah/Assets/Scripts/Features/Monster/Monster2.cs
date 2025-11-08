using Features.Item.abc;
using UnityEngine;

namespace Features.Monster
{
    public class Monster2 : MonoBehaviour, IDamageable
    {
        private float _hp;
        public float Hp => _hp;
        private float _maxHp;
        public float MaxHp => _maxHp;

        private Rigidbody _rigidbody;

        [SerializeField] private GameObject reward;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hp = 80;
        }

        void Update()
        {
            Death();
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
        }

        private void Death()
        {
            if (_hp <= 0)
            {
                Destroy(gameObject);
                Instantiate(reward, transform.position, transform.rotation);
            }
        }
    }
}