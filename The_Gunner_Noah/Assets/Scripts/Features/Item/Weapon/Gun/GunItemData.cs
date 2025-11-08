using UnityEngine;
using Utils;

namespace Features.Item.abc
{
    public abstract class GunItemData : ItemData
    {
        [Header("Gun Stats")]
        public GunType gunType;
        public float fireRate;
        public int ammoCapacity;
        public float reloadTime;
        public float staminaCost;

        public BulletData bulletData;

        protected override InventoryType InventoryType => InventoryType.Gun;

        public abstract void Fire(Transform firePoint);
    }
}