using UnityEngine;
using Utils;

namespace Features.Item.Weapon
{
    public abstract class Bullet : MonoBehaviour
    {
        protected GameObject BulletPrefab;
        protected float BulletSpeed;
        protected float BulletRange;
        protected float Damage;
        protected Rigidbody Rigidbody;
        protected GunType GunType;
        protected Vector3 StartPosition;


        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Setup(float damage, float speed, float range)
        {
            Damage = damage;
            BulletSpeed = speed;
            BulletRange = range;
            StartPosition = transform.position;
        }

        protected virtual void OnEnable()
        {
            if(Rigidbody != null)
            {
                Rigidbody.useGravity = false;
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
                Rigidbody.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse);
            }
        }

        protected virtual void ReturnToPool()
        {
            BulletFactory.Instance.ReturnBullet(this, GunType);
        }
    }
}