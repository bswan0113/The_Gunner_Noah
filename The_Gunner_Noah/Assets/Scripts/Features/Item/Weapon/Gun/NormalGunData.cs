using UnityEngine;
using Utils;

namespace Features.Item.abc
{
    [CreateAssetMenu(fileName = "Normal Gun", menuName = "GunData/NormalGun")]
    public class NormalGunData : GunItemData
    {
        public override void Fire(Transform firePoint)
        {
            BsLogger.Log($"Fire!!! {Extensions.ToDebugString(bulletData)}");
            Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}