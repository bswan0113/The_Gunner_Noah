using System;
using UnityEngine;

namespace Features.Item.Weapon.Bullet
{
    public class HookBullet : BulletObject
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

        }

    }
}