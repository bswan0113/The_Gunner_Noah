using Core;
using Core.Managers;
using Features.Item.Weapon.Bullet;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.Item.Weapon.Gun
{
    [CreateAssetMenu(fileName = "WindGun Gun", menuName = "GunData/WindGun")]
    public class WindGunData : GunItemData
    {
        public override void Fire(Transform firePoint, GameObject gameObject, Vector3 targetPoint = new Vector3())
        {
            GameObject bulletObj = Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
            bulletObj.GetComponent<BulletObject>().SetOwner(gameObject);

            Vector3 direction;
            if (targetPoint != Vector3.zero)
            {
                direction = (targetPoint - firePoint.position).normalized;
            }
            else
            {
                direction = firePoint.forward;
            }

            bulletObj.GetComponent<BulletObject>().SetDirection(direction);
            AudioManager.Instance.PlaySfx(AudioManager.Instance.windBullet);
        }
    }
}