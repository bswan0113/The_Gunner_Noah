using Features.Item.Weapon.Common;
using UnityEngine;
using Utils;

namespace Features.Item.Weapon.Normal
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