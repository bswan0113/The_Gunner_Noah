using System.Collections.Generic;
using Features.Item;
using Features.Item.Common;
using TMPro;
using UnityEngine;
using Utils;

namespace Features.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        private Dictionary<InventoryType, List<ItemData>> _inventory = new Dictionary<InventoryType, List<ItemData>>();
        private Dictionary<InventoryType, int> _currentIndex = new Dictionary<InventoryType, int>();
        [SerializeField] private TextMeshProUGUI gunText;
        [SerializeField] private TextMeshProUGUI potionText;

        public void AddItem(ItemData itemData)
        {
            InventoryType type = itemData.GetInventoryType();
            if (!_inventory.ContainsKey(type))
            {
                _inventory[type] = new List<ItemData>();
            }
            if (!_currentIndex.ContainsKey(type))
            {
                _currentIndex[type] = 0;
            }
            _inventory[type].Add(itemData);

            // UI 업데이트
            if (type == InventoryType.Gun)
            {
                UpdateGunUI();
            }
            else if (type == InventoryType.Potion)
            {
                UpdatePotionUI();
            }
        }

        public ItemData ChangeNextItem(InventoryType type)
        {
            if (!_inventory.ContainsKey(type)) return null;

            List<ItemData> items = _inventory[type];
            if (items == null || items.Count == 0) return null;

            if (!_currentIndex.ContainsKey(type))
            {
                _currentIndex[type] = 0;
            }

            _currentIndex[type] = (_currentIndex[type] + 1) % items.Count;

            if (type == InventoryType.Gun)
            {
                UpdateGunUI();
            }
            else if (type == InventoryType.Potion)
            {
                UpdatePotionUI();
            }

            return _inventory[type][_currentIndex[type]];
        }

        public ItemData GetCurrentItem(InventoryType type)
        {
            if (!_inventory.ContainsKey(type)) return null;

            List<ItemData> items = _inventory[type];
            if (items == null || items.Count == 0) return null;

            return items[_currentIndex[type]];
        }

        public void RemovePotion(ItemData itemData)
        {
            if (itemData == null) return;

            InventoryType type = itemData.GetInventoryType();
            if (type != InventoryType.Potion) return;

            if (!_inventory.ContainsKey(type)) return;

            List<ItemData> potions = _inventory[type];
            if (potions == null || potions.Count == 0) return;

            int currentIdx = _currentIndex[type];
            potions.RemoveAt(currentIdx);

            if (potions.Count == 0)
            {
                _currentIndex[type] = 0;
                UpdatePotionUI();
                return;
            }
            if (currentIdx >= potions.Count)
            {
                _currentIndex[type] = potions.Count - 1;
            }
            UpdatePotionUI();
        }

        private void UpdateGunUI()
        {
            ItemData currentGun = GetCurrentItem(InventoryType.Gun);
            if (currentGun != null && gunText != null)
            {
                gunText.text = $"{currentGun.itemName}";
            }
        }

        private void UpdatePotionUI()
        {
            ItemData currentPotion = GetCurrentItem(InventoryType.Potion);
            if (potionText != null)
            {
                if (currentPotion != null)
                {
                    potionText.text = $"{currentPotion.itemName}";
                }
                else
                {
                    potionText.text = "None";
                }
            }
        }
    }
}