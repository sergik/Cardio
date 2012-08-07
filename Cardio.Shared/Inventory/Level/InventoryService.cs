using System.Linq;
using Cardio.UI.Core;

namespace Cardio.UI.Inventory.Level
{
    public class InventoryService
    {
        private readonly int _moneySlotIndex;

        public static readonly int Capacity = 4;

        public int Money
        {
            get
            {
                if (Slots[_moneySlotIndex] != null)
                {
                    return Slots[_moneySlotIndex].Count;
                }
                return 0;
            }

            set
            {
                if (Slots[_moneySlotIndex] != null)
                {
                    Slots[_moneySlotIndex].Count = value > 0 ? value : 0;
                }
            }
        }

        public string Gun { get; set; }

        public string Shield { get; set; }

        public string Body { get; set; }

        public InventoryEntry[] Slots { get; private set; }

        public InventoryService()
        {
            Slots = new InventoryEntry[Capacity];
            SetDefaultAsset();
            _moneySlotIndex = Items.BloodElement.SlotIndex;
        }

        private void SetDefaultAsset()
        {
            Shield = @"Shields\Shield1";
            Gun = @"Weapons\BaseWeapon";
            Body = @"Bodies\Body1";
        }

        public void UseItemFromSlot(int index, GameState state)
        {
            var slot = Slots[index];

            if (slot == null || slot.Count == 0)
            {
                return;
            }

            if (slot.Base.Use != null)
            {
                slot.Base.Use(state);
                slot.Count--;

                if (slot.Count == 0)
                {
                    Slots[index] = null;
                }
            }
        }

        public void Add(InventoryItem item)
        {
            if (Slots[item.SlotIndex] == null)
            {
                Slots[item.SlotIndex] = new InventoryEntry{Base = item, Count = 1};
            }
            else
            {
                Slots[item.SlotIndex].Count++;
            }
        }

        public void Add(InventoryItem item, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Add(item);
            }
        }

        public void Set(InventoryItem item, int count)
        {
            if (Slots[item.SlotIndex] == null)
            {
                Slots[item.SlotIndex] = new InventoryEntry { Base = item, Count = count };
            }
            else
            {
                Slots[item.SlotIndex].Count = count;
            }
        }

        public void ToActive(int regularIndex, int activeIndex)
        {
            var regular = Slots[regularIndex];
            var active = Slots[activeIndex];

            Slots[regularIndex] = active;
            Slots[activeIndex] = regular;
        }

        public bool ActiveItemExistsAt(int index)
        {
            return index < Slots.Length && Slots[index] != null;
        }

        public void Reset()
        {
            for (int i = 0; i < Slots.Length; i++ )
            {
                Slots[i] = null;
            }
            SetDefaultAsset();
        }
    }
}