using System.Collections;
using System.Collections.Generic;
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

        private bool _isInWater = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
            _playerInventory = GetComponentInChildren<PlayerInventory>();
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
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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
                _gunEquipped.Fire(gunHoldPoint, this.gameObject);
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
    }
}