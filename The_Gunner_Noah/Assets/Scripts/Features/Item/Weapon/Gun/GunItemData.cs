using Features.Item.Common;
using Features.Item.Weapon.Bullet;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    public abstract class GunItemData : ItemData
    {
        [Header("Gun Stats")]
        public GunType gunType;
        public float fireRate;
        // public int ammoCapacity;
        // public float reloadTime;
        public float staminaCost;

        public BulletData bulletData;

        protected override InventoryType InventoryType => InventoryType.Gun;

        public abstract void Fire(Transform firePointm, GameObject gameObject,Vector3 targetPoint = new Vector3());
    }

    public enum GunType
    {
        Normal, Wind, Hook
    }
}