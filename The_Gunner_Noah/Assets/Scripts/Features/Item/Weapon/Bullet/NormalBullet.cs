using Features.Common;
using UnityEngine;
using Utils;

namespace Features.Item.Weapon.Bullet
{
    public class NormalBullet : BulletObject
    {
        protected override void Start()
        {
            base.Start();
            if (_speed > 0f && _range > 0f)
            {
                float lifetime = _range / _speed;
                Destroy(gameObject, lifetime);
            }
        }
        public override void Hit(GameObject hitObject)
        {
            if (owner == null) return;
            if(hitObject.CompareTag(owner.tag)) return;
            IDamageable damageable = hitObject.GetComponent<IDamageable>();
            BsLogger.Log($"Hit: {hitObject.name}");
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
        }

    }
}
