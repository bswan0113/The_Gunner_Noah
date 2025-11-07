using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using Utils;

namespace Features.Item.Weapon
{
    public abstract class Gun : Item
    {
        [Header("Gun Stats")]
        [SerializeField] protected float damage;
        [SerializeField] protected float fireRate;
        [SerializeField] protected int ammoCapacity;
        [SerializeField] protected float reloadTime;
        [SerializeField] protected float staminaCost;
        protected GunType GunType;

        protected int currentAmmo;
        private float nextFireTime = 0f;


        protected BulletFactory BulletFactory;
        [SerializeField]protected Transform muzzlePoint;

        protected virtual void Awake()
        {
            currentAmmo = ammoCapacity;
            BulletFactory = BulletFactory.Instance;
        }

        public void TryFire()
        {
            // TODO 스태미나 체크
            // TODO 탄약 체크
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Fire();
            }
        }
        protected virtual void Fire()
        {
            if (muzzlePoint == null)
            {
                return;
            }
            GameObject bullet = BulletFactory.GetBullet(GunType).gameObject;
            bullet.transform.position = muzzlePoint.position;
            bullet.transform.rotation = muzzlePoint.rotation;
        }

    }

    public enum GunType
    {
        Normal, Wind, Hook
    }
}