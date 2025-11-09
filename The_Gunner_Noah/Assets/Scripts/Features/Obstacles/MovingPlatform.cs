using UnityEngine;

namespace Features.Obstacles
{
    public class MovingPlatform : Movable
    {
        private Transform _originalParent = null;

        private void OnCollisionEnter(Collision collision)
        {
            _originalParent = collision.transform.parent;
            collision.transform.SetParent(transform);
        }

        private void OnCollisionExit(Collision collision)
        {
            collision.transform.SetParent(_originalParent);
        }
    }
}