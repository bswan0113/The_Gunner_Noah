using UnityEngine;

namespace Features.Item.Common
{
    public abstract class ItemData : ScriptableObject
    {
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