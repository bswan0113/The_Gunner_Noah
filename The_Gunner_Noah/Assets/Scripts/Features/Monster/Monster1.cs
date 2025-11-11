using System;
using Core.Managers;
using Features.Common;
using Features.Item.Weapon.Gun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Features.Monster
{
    public class Monster1 : MonoBehaviour, IDamageable
    {
        private float _hp;
        public float Hp => _hp;
        private float _maxHp;
        public float MaxHp => _maxHp;

        private Rigidbody _rigidbody;

        [SerializeField] private Slider hpBar;

        [SerializeField] private GunItemData equipedItem;
        [SerializeField] public float minAttackDelay = 0.3f;
        [SerializeField] public float maxAttackDelay = 2f;
        private float _currentAttackDelay;
        private float _lastAttackTime;

        [SerializeField]private float detectRange = 1f;
        private Transform _playerTransform;
        private MonsterState _state;

        [SerializeField] private LayerMask playerLayer;

        [SerializeField] private bool isGimic = false;
        [SerializeField] private Monster2 boss;

        private bool _detectPlayer;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hp = 50;
            _maxHp = _hp;
            hpBar.value = _hp / _maxHp;
            _state = MonsterState.Sleeping;
        }

        void Start()
        {
            _currentAttackDelay = Random.Range(minAttackDelay, maxAttackDelay);
        }


        private void Update()
        {
            Attack();
            DetectPlayer();
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
            if (isGimic && boss != null)
            {
                boss.TakeDamage(2500);
            }
            AudioManager.Instance.PlaySfx(AudioManager.Instance.die);
            Destroy(gameObject);
        }

        private void Attack()
        {
            if(equipedItem == null) return;
            if (_state == MonsterState.Sleeping) return;
            if (Time.time >= _lastAttackTime + _currentAttackDelay)
            {
                equipedItem.Fire(gameObject.transform, gameObject);
                _lastAttackTime = Time.time;
                _currentAttackDelay = Random.Range(minAttackDelay, maxAttackDelay);
            }
        }

        private void DetectPlayer()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectRange, playerLayer);

            if (colliders.Length == 0)
            {
                _state = MonsterState.Sleeping;
                return;
            }

            Transform targetPlayer = colliders[0].transform;
            Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    if (!_detectPlayer)
                    {
                        AudioManager.Instance.PlaySfx(AudioManager.Instance.detect);
                        _detectPlayer = true;
                    }
                    transform.LookAt(targetPlayer);
                    _state = MonsterState.IsAttacking;
                    return;
                }
            }
            _state = MonsterState.Sleeping;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }


        public enum MonsterState
        {
            IsAttacking, Sleeping
        }

    }
}