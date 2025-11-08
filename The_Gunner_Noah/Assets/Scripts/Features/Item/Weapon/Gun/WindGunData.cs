using Features.Item.Weapon.Common;
using UnityEngine;
using Utils;

namespace Features.Item.abc
{
    [CreateAssetMenu(fileName = "WindGun Gun", menuName = "GunData/WindGun")]
    public class WindGunData : GunItemData
    {
        public override void Fire(Transform firePoint)
        {
            BsLogger.Log($"Fire!!! WindGun {Extensions.ToDebugString(bulletData)}");
            GameObject bulletObj = Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
            bulletObj.GetComponent<BulletObject>().SetDirection(firePoint.forward);
        }
    }
}