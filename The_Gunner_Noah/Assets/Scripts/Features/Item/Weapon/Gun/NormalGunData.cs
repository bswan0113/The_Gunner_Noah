using Features.Item.Weapon.Bullet;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "Normal Gun", menuName = "GunData/NormalGun")]
    public class NormalGunData : GunItemData
    {

        public override void Fire(Transform firePoint, GameObject gameObject)
        {
            GameObject bulletObj =  Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
            bulletObj.GetComponent<BulletObject>().SetOwner(gameObject);
        }
    }
}