using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.Item.abc
{
    [CreateAssetMenu(fileName = "Potion_", menuName = "Potion")]
    public class PotionItemData : ItemData
    {
        public int amount;
        public PotionType potionType;
        public float duration;
        public PowerUpTargetStat targetStat;

        protected override InventoryType InventoryType => InventoryType.Potion;

    }

    public enum PotionType
    {
        PowerUp, Heal
    }

    public enum PowerUpTargetStat
    {
        None, Jump
    }
}