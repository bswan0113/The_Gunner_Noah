using Cinemachine;
using Core.Managers;
using Features.Inventory;
using Features.Item;
using Features.Item.Common;
using Features.Item.Potion;
using Features.Item.Weapon.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI
{
    public class Crosshair : MonoBehaviour
    {

        [SerializeField] private float interactRange = 50f;
        [SerializeField] private LayerMask itemBaseLayer;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Player.Player player;
        [SerializeField]private PlayerInventory playerInventory;
        private FieldItem _currentItemBase;

        private float _lastCheckTime;
        private float _checkInterval = 0.5f;

        public Material transparentMaterial;
        private GameObject lastFadedObject;
        private Material originalMaterial;
        [SerializeField]private Image crossHair;

        // private FocusState _currentFocusState;
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // _currentFocusState = FocusState.Normal;
        }
        void Awake()
        {
            if (playerCamera == null)
                playerCamera = Camera.main;
        }

        void Update()
        {
            CheckForItemBase();
            HandlePickup();
            Perspective();
            LockMouse();
        }

        private void LockMouse()
        {
            if (GameManager.Instance.MouseSettingLock)
            {
                crossHair.enabled = false;
                return;
            }

            if ((player.IsAiming || Input.GetKey(KeyCode.LeftAlt)))
            {
                Cursor.lockState = CursorLockMode.None;
                crossHair.enabled = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                crossHair.enabled = false;
            }
        }

        void FixedUpdate()
        {
            transform.position = Input.mousePosition;
        }
        void CheckForItemBase(){

            if (Time.time - _lastCheckTime > _checkInterval)
            {
                _lastCheckTime = Time.time;
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                // Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 1f);
                if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemBaseLayer))
                {
                    FieldItem itemBase = hit.collider.GetComponent<FieldItem>();

                    if (itemBase != null && itemBase != _currentItemBase)
                    {
                        _currentItemBase?.OnInvestigateEnd();
                        _currentItemBase = itemBase;
                        _currentItemBase.OnInvestigateStart();
                    }
                }
                else
                {
                    if (_currentItemBase != null)
                    {
                        _currentItemBase.OnInvestigateEnd();
                        _currentItemBase = null;
                    }
                }
            }

        }

        void HandlePickup()
        {
            if (_currentItemBase != null && Input.GetKeyDown(KeyCode.E))
            {
                ItemData pickedUpItemData = _currentItemBase.GetItemData();
                if (pickedUpItemData == null) return;

                playerInventory.AddItem(pickedUpItemData);

                if (pickedUpItemData is GunItemData gunData)
                {
                    player.EquipGun(gunData);
                }else if (pickedUpItemData is PotionItemData potionData)
                {
                    player.EquipPotion(potionData);
                }

                _currentItemBase.OnPickup();
                _currentItemBase = null;
            }
        }

        void Perspective()
        {
            Transform playerTransform = player.gameObject.transform;
            Vector3 direction = playerTransform.position - playerCamera.transform.position;
            float distance = direction.magnitude;

            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, direction, out hit, distance))
            {
                if (hit.collider.transform != playerTransform)
                {
                    if (lastFadedObject != null && lastFadedObject != hit.collider.gameObject)
                    {
                        RestoreLastObject();
                    }
                    Renderer rend = hit.collider.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        if (lastFadedObject != hit.collider.gameObject)
                        {
                            originalMaterial = rend.material;
                            lastFadedObject = hit.collider.gameObject;
                            rend.material = transparentMaterial;
                        }
                    }
                }
                else if (lastFadedObject != null)
                {
                    RestoreLastObject();
                }
            }
            else
            {
                if (lastFadedObject != null)
                {
                    RestoreLastObject();
                }
            }
        }

        void RestoreLastObject()
        {
            Renderer rend = lastFadedObject.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = originalMaterial;
            }
            lastFadedObject = null;
            originalMaterial = null;
        }

    }

}