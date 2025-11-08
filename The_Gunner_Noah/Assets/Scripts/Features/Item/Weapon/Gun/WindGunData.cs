using Features.Item.Weapon.Bullet;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "WindGun Gun", menuName = "GunData/WindGun")]
    public class WindGunData : GunItemData
    {
        public override void Fire(Transform firePoint)
        {
            GameObject bulletObj = Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
            bulletObj.GetComponent<BulletObject>().SetDirection(firePoint.forward);
        }
    }
}