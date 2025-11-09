using Features.Common;
using Features.Obstacles;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Monster
{
    public class Monster2 : MonoBehaviour, IDamageable
    {
        private float _hp;
        public float Hp => _hp;
        private float _maxHp;
        public float MaxHp => _maxHp;

        private Rigidbody _rigidbody;

        [SerializeField] private Operated reward;
        [SerializeField] private Slider hpBar;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hp = 10000;
            _maxHp = _hp;
            hpBar.value = _hp / _maxHp;
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
            hpBar.value = _hp / _maxHp;
            if (_hp <= 0f)
            {
                this.Death();
            }
        }

        private void Death()
        {
            if (_hp <= 0)
            {
                Destroy(gameObject);
                reward.Action();
            }
        }
    }
}