using UnityEngine;

namespace Features.Item.Weapon.Bullet
{
    public abstract class BulletObject : MonoBehaviour
    {
        protected float _speed;
        protected float _damage;
        protected float _range;
        private Rigidbody _rigidbody;
        [SerializeField]private BulletData data;

        protected Vector3 _direction;

        public GameObject owner;


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
            // _rigidbody.AddTorque(transform.forward * 1000f);
        }

        public void SetOwner(GameObject ownerObject)
        {
            owner = ownerObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(owner == null) return;
            if (other.CompareTag(owner.tag)) return;
            Hit(other.gameObject);
        }

        abstract public void Hit(GameObject hitObject);
        public void SetDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }
    }
}