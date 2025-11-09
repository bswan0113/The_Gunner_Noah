using Features.Item.Weapon.Bullet;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "Hook Gun", menuName = "GunData/HookGunData")]
    public class HookGunData : GunItemData
    {

        private bool _isRopeShot = false;

        public override void Fire(Transform firePoint, GameObject gameObject)
        {
            BulletData bullet = Instantiate(bulletData, firePoint.position, firePoint.rotation);
        }
    }
}