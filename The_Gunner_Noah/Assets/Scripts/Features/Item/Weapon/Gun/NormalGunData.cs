using Core;
using Core.Managers;
using Features.Item.Weapon.Bullet;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "Normal Gun", menuName = "GunData/NormalGun")]
    public class NormalGunData : GunItemData
    {

        public override void Fire(Transform firePoint, GameObject gameObject, Vector3 targetPoint = new Vector3())
        {
            Quaternion rotation;

            if (targetPoint != Vector3.zero)
            {
                Vector3 direction = (targetPoint - firePoint.position).normalized;
                rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                rotation = firePoint.rotation;
            }
            GameObject bulletObj = Instantiate(bulletData.bulletPrefab, firePoint.position, rotation);
            bulletObj.GetComponent<BulletObject>().SetOwner(gameObject);
            AudioManager.Instance.PlaySfx(AudioManager.Instance.bullet);
        }
    }
}