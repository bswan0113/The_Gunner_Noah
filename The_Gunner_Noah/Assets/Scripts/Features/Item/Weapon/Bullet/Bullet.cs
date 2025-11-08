using Features.Item.abc;
using UnityEngine;

namespace Features.Item.Weapon.Common
{
    public abstract class BulletObject : MonoBehaviour
    {
        protected float _speed;
        protected float _damage;
        protected float _range;
        private Rigidbody _rigidbody;
        [SerializeField]private BulletData data;

        protected Vector3 _direction;


        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            _rigidbody.useGravity = false;
            _speed = data.speed;
            _damage = data.damage;
            _range = data.range;
        }
        protected virtual void Start()
        {
            _rigidbody.velocity = transform.forward * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;
            Hit(other.gameObject);
        }

        abstract public void Hit(GameObject hitObject);
        public void SetDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }
    }
}