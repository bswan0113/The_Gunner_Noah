using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "Normal Gun", menuName = "GunData/NormalGun")]
    public class NormalGunData : GunItemData
    {
        public override void Fire(Transform firePoint)
        {
            Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}