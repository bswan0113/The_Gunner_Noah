using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Features.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        public float movementSpeed;
        public float jumpPower;
        public LayerMask groundLayerMask;
        public float rotationSpeed = 15f;

        private Rigidbody _rigidbody;
        private Transform cameraTransform;
        private Vector2 curMovementInput;

        [Header("Ground Check")]
        public Transform groundCheck;
        public float groundDistance = 0.2f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            MovePlayer();
            RotatePlayer();
        }

        private void MovePlayer()
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * curMovementInput.y + camRight * curMovementInput.x).normalized;

            Vector3 finalVelocity = moveDirection * movementSpeed;
            finalVelocity.y = _rigidbody.velocity.y;

            _rigidbody.velocity = finalVelocity;
        }

        private void RotatePlayer()
        {
            Vector3 lookDirection = cameraTransform.forward;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && IsGrounded())
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }

        bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);
        }

        private void OnTriggerStay(Collider other)
        {
            // 부딪힌 오브젝트의 태그가 "Water" 라면
            if (other.CompareTag("Water"))
            {
                // 여기에 스태미나 감소 로직을 넣으세요.
                // 예: stamina -= 10f * Time.deltaTime;
                Debug.Log("물 속에 있다! 스태미나 감소!");
            }
        }
    }
}