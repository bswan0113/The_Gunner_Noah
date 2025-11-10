using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Core.Managers;
using Features.Common;
using Features.Inventory;
using Features.Item.Common;
using Features.Item.Potion;
using Features.Item.Weapon.Gun;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Features.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        [Header("Movement")] public float movementSpeed;
        public float jumpPower;
        public LayerMask groundLayerMask;
        public float rotationSpeed = 15f;
        public int jumpStamina = 10;

        private Rigidbody _rigidbody;
        private Transform cameraTransform;
        private Vector2 curMovementInput;

        [Header("Ground Check")] public Transform groundCheck;
        public float groundDistance = 0.2f;


        private GunItemData _gunEquipped;
        private PotionItemData _potionEquipped;
        public bool IsEquipped => _gunEquipped != null;
        private PlayerInventory _playerInventory;

        private bool _isShooting = false;
        private float _nextFireTime = 0f;

        [SerializeField] float waterEffectInterval = 0.5f;
        private float _nextWaterEffectTime = 0f;
        [SerializeField] float waterStaminaCost = 30f;

        [SerializeField] private Transform gunHoldPoint;



        public Condition health;
        public Condition stamina;


        private Vector3 _oriPosForTest;

        private bool _isInWater = false;

        private bool _isPressingForward = false;
        public bool IsPressingForward => _isPressingForward;

        private bool IsParkouring = false;

        private bool _isAiming = false;

        [SerializeField] CinemachineVirtualCamera fpsCamera;
        [SerializeField] CinemachineFreeLook tpsCamera;

        private Vector2 lookInput;

        [SerializeField] private TutorialData _waterTutorial;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
            _playerInventory = GetComponentInChildren<PlayerInventory>();
            _oriPosForTest = transform.position;
        }

        void Start()
        {
            // Test();
        }


        private void Update()
        {
            HandleShooting();
            RecoverStamina(stamina.passiveValue * Time.deltaTime);
            CheckAlive();
            HandleEffectWater();
        }

        private void FixedUpdate()
        {
            MovePlayer();

        }

        private void LateUpdate()
        {
            RotatePlayer();
        }

        private void MovePlayer()
        {
            if (IsParkouring) return;
            float horizontal = _isAiming ? 0f : curMovementInput.x;

            Vector3 moveDirection = (transform.forward * curMovementInput.y + transform.right * horizontal).normalized;

            Vector3 finalVelocity = moveDirection * movementSpeed;
            finalVelocity.y = _rigidbody.velocity.y;

            _rigidbody.velocity = finalVelocity;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (_isAiming) return;
            lookInput = context.ReadValue<Vector2>();
        }

        private void RotatePlayer()
        {
            if (_isAiming)
            {
                transform.Rotate(Vector3.up * curMovementInput.x * rotationSpeed);

            }
            else
            {
                transform.Rotate(Vector3.up * lookInput.x * rotationSpeed / 10);
            }
        }


        bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                StartCoroutine(TutorialManager.Instance.ShowTutorial(_waterTutorial));

                _isInWater = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                _isInWater = false;
            }
        }

        public void EquipGun(GunItemData gunItemData)
        {
            _gunEquipped = gunItemData;
        }

        public void EquipPotion(PotionItemData potionItemData)
        {
            _potionEquipped = potionItemData;
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            curMovementInput = context.ReadValue<Vector2>();
            if (context.control.path == "/Keyboard/w")
            {
                if (context.performed)
                {
                    _isPressingForward = true;
                }
                else if (context.canceled)
                {
                    _isPressingForward = false;
                }
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (stamina.curValue < jumpStamina) return;
            if (context.phase == InputActionPhase.Performed && IsGrounded())
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                UseStamina(jumpStamina);
            }
        }

        public void OnShoot(InputAction.CallbackContext context)
        {

            if (context.started)
            {
                _isShooting = true;

            }
            else if (context.canceled)
            {
                _isShooting = false;

            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isAiming = true;
                fpsCamera.Priority = 100;

            }
            else if (context.canceled)
            {
                _isAiming = false;
                fpsCamera.Priority = 0;
            }
        }

        private void HandleShooting()
        {
            if (!IsEquipped || !_isShooting)
            {
                return;
            }

            if (Time.time >= _nextFireTime)
            {
                if (stamina.curValue < _gunEquipped.staminaCost) return;
                UseStamina(_gunEquipped.staminaCost);
                _nextFireTime = Time.time + 1f / _gunEquipped.fireRate;
                if (_isAiming)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Vector3 targetPoint;

                    if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
                    {
                        targetPoint = hit.point;
                    }
                    else
                    {
                        targetPoint = ray.GetPoint(1000f);
                    }

                     _gunEquipped.Fire(gunHoldPoint, this.gameObject, targetPoint);
                }
                else
                {
                    _gunEquipped.Fire(gunHoldPoint, this.gameObject);
                }

            }
        }

        public void OnChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GunItemData gunItemData = _playerInventory.ChangeNextItem(InventoryType.Gun) as GunItemData;
                if (gunItemData != null) EquipGun(gunItemData);
            }
        }

        public void OnChangePotion(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PotionItemData potionItemData = _playerInventory.ChangeNextItem(InventoryType.Potion) as PotionItemData;
                if (potionItemData != null) EquipPotion(potionItemData);
            }
        }

        public void OnUsePotion(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                UsePotion();
            }
        }

        private void UsePotion()
        {
            if (_potionEquipped == null) return;
            if (_potionEquipped.potionType == PotionType.Heal)
            {
                health.Add(_potionEquipped.amount);
            }
            else if (_potionEquipped.potionType == PotionType.PowerUp)
            {
                StartCoroutine(ApplyPowerUp(_potionEquipped));
            }

            _playerInventory.RemovePotion(_potionEquipped);
            _potionEquipped = null;


        }

        private IEnumerator ApplyPowerUp(PotionItemData potion)
        {
            float originalValue = 0f;

            switch (potion.targetStat)
            {
                case PowerUpTargetStat.Jump:
                    originalValue = jumpPower;
                    jumpPower += potion.amount;
                    break;
            }

            yield return new WaitForSeconds(potion.duration);

            switch (potion.targetStat)
            {
                case PowerUpTargetStat.Jump:
                    jumpPower = originalValue;
                    break;
            }
        }

        public void TakeDamage(float damage)
        {
            health.Subtract(damage);
        }

        public void UseStamina(float amount)
        {
            stamina.Subtract(amount);
        }

        private void RecoverStamina(float amount)
        {
            if (_isInWater) return;
            stamina.Add(amount);
        }

        private void CheckAlive()
        {
            if (health.curValue <= 0 || transform.position.y < -10)
            {
                // if (_oriPosForTest != null)
                // {
                //     transform.position = _oriPosForTest;
                //     return;
                // }
                GameManager.Instance.GameOver();
            }
        }

        private void HandleEffectWater()
        {
            if (!_isInWater) return;
            if (Time.time >= _nextWaterEffectTime)
            {
                _nextWaterEffectTime  = Time.time + waterEffectInterval;
                if (stamina.curValue > 0)
                {
                    UseStamina(waterStaminaCost);
                }
                else
                {
                    UseStamina(waterStaminaCost);
                    health.Subtract(waterStaminaCost);
                }
            }
        }

        public void SetUseGravity()
        {
            _rigidbody.useGravity = true;
            // _rigidbody.isKinematic = false;
        }

        public void ReleaseUseGravity()
        {
            _rigidbody.useGravity = false;
            // _rigidbody.isKinematic = true;
        }

        public void SetPakurState(bool state)
        {
            IsParkouring = state;
        }

        public void SetRigidBodyConstraints(RigidbodyConstraints constraints)
        {
            _rigidbody.constraints |= constraints;
        }

        public void ReleaseRigidBodyConstraints(RigidbodyConstraints constraints)
        {
            _rigidbody.constraints &= ~constraints;
        }

    }
}