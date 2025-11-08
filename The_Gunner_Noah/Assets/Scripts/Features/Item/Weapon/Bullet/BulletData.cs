using UnityEngine;

namespace Features.Item.Weapon.Bullet
{

    [CreateAssetMenu(fileName = "New Bullet", menuName = "GunData/BulletData")]
    public class BulletData : ScriptableObject
    {
        public GameObject bulletPrefab;
        public float speed;
        public float damage;
        public float range;
    }
}