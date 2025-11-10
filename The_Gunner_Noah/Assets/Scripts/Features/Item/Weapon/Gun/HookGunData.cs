using Features.Item.Weapon.Bullet;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "Hook Gun", menuName = "GunData/HookGunData")]
    public class HookGunData : GunItemData
    {

        public override void Fire(Transform firePoint, GameObject gameObject, Vector3 targetPoint = new Vector3())
        {
            BulletData bullet = Instantiate(bulletData, firePoint.position, firePoint.rotation);
        }
    }
}