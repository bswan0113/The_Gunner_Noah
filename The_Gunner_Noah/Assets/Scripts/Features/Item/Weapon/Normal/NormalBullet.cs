// NormalBullet.cs
using UnityEngine;

namespace Features.Item.Weapon.Normal
{
    public class NormalBullet : Bullet
    {
        private float _traveledDistanceSqr = 0f;
        private Vector3 _lastPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartPosition = transform.position;
            _lastPosition = transform.position;
            _traveledDistanceSqr = 0f;
        }

        void Update()
        {
            Vector3 deltaMove = transform.position - _lastPosition;
            _traveledDistanceSqr += deltaMove.sqrMagnitude;
            _lastPosition = transform.position;

            if (_traveledDistanceSqr >= BulletRange * BulletRange)
            {
                ReturnToPool();
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(Damage);
            }
            ReturnToPool();
        }
    }
}