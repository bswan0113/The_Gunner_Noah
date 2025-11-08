using System;
using UnityEngine;

namespace Features.Monster
{
    public class Monster1 : MonoBehaviour, IDamageable
    {
        private float _hp;
        public float Hp => _hp;
        private float _maxHp;
        public float MaxHp => _maxHp;

        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hp = 50;
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
            if (_hp <= 0f)
            {
                this.Death();
            }
        }

        private void Death()
        {
            Destroy(gameObject);
        }

    }
}