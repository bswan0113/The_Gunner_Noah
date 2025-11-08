using UnityEngine;

namespace Features.Item.Common
{
    public abstract class ItemData : ScriptableObject
    {
        [Header("공통 정보")]
        public string itemName;
        [TextArea]
        public string itemDescription;
        protected abstract InventoryType InventoryType { get; }

        public InventoryType GetInventoryType() => InventoryType;
    }

    public enum InventoryType
    {
        Gun, Potion
    }
}