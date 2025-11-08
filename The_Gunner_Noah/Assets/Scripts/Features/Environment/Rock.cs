using System.Collections;
using UnityEngine;
using Utils;

namespace Features.Environment
{
    public class Rock : MonoBehaviour, IPushable
    {

        [Header("Push Settings")]
        [SerializeField] private float pushForce = 200f;
        [SerializeField] private float drag = 2f;

        private Rigidbody _rigidbody;

        private bool _isPushed = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.mass = 10f;
            _rigidbody.drag = drag;
            _rigidbody.angularDrag = 0.5f;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.isKinematic = true;
        }

        public void Pushed(Vector3 pushDirection)
        {
            if (_isPushed) return;

            StartCoroutine(PushCoroutine(pushDirection));
        }

        private IEnumerator PushCoroutine(Vector3 pushDirection)
        {
            _isPushed = true;
            _rigidbody.isKinematic = false;

            pushDirection.y = 0;
            pushDirection.Normalize();

            _rigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            while (_rigidbody.velocity.magnitude > 0.1f)
            {
                yield return new WaitForFixedUpdate();
            }

            _rigidbody.isKinematic = true;
            _isPushed = false;
        }

    }
}