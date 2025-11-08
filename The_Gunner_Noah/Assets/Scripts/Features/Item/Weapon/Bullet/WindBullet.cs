using Features.Common;
using UnityEngine;

namespace Features.Item.Weapon.Bullet
{
    public class WindBullet : BulletObject
    {
        private bool _hasHit = false;
        protected override void Start()
        {
            Destroy(gameObject, 1f);
        }
        public override void Hit(GameObject hitObject)
        {
            if (_hasHit) return;
            IPushable pushable = hitObject.GetComponent<IPushable>();
            if (pushable != null)
            {
                _hasHit = true;
                pushable.Pushed(_direction);
            }
        }
    }
}