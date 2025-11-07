// NormalGun.cs
using UnityEngine;

namespace Features.Item.Weapon.Normal
{
    public class NormalGun : Gun
    {
        protected override void Awake()
        {
            base.Awake();
            GunType = GunType.Normal;
        }

        void Start()
        {
            GameObject bullet = BulletFactory.GetBullet(GunType).gameObject;
            bullet.transform.position = muzzlePoint.position;
            bullet.transform.rotation = muzzlePoint.rotation;
        }
    }
}