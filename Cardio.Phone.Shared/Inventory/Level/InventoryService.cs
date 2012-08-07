using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;

namespace Cardio.Phone.Shared.Inventory.Level
{
    public class InventoryService
    {
        private readonly int _moneySlotIndex;

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

        public string Medcit { get; set; }

        public string Tablet { get; set; }

        public string Spike { get; set; }

        public string FrontWeapon { get; set; }

        public List<InventoryEntry> Slots { get; private set; }

        public InventoryService()
        {
            Slots = new List<InventoryEntry>();
            SetDefaultAsset();
            _moneySlotIndex = Items.BloodElement.SlotIndex;
        }

        private void SetDefaultAsset()
        {
            Shield = @"Shields\Shield1";
            Gun = @"Weapons\BaseWeapon";
            Body = @"Bodies\Body1";
            Medcit = @"Inventory\Medcits\MedcitLevel1";
            //Tablet = @"Inventory\Tablets\Tablet1";
        }

        public void UseItemFromSlot(int index, GameState state)
        {
            var slot = Slots[index];

            if (slot == null || !slot.Base.IsUsable)
            {
                return;
            }

            if (slot.Base.IsReuseble)
            {
                slot.Base.OnUse(state);
            }

            if (slot.Count > 0)
            {
                slot.Base.OnUse(state);
                slot.Count--;

                if (slot.Count == 0)
                {
                    Slots[index] = null;
                }
            }
        }

        public void Add(InventoryItem item)
        {
            var entry = GetEntryByName(item.Name);
            if (entry == null)
            {
                Slots.Add(new InventoryEntry {Base = item, Count = 1});
            }
        }

        public void Add(InventoryItem item, int count)
        {
            var entry = GetEntryByName(item.Name);
            if (entry == null)
            {
                Slots.Add(new InventoryEntry {Base = item, Count = count});
            }
            else
            {
                entry.Count += count;
            }
        }

        private InventoryEntry GetEntryByName(string name)
        {
            return Slots.FirstOrDefault(e => e.Base.Name == name);
        }

        public void AddReuseble(InventoryItem item)
        {
            var entry = GetEntryByName(item.Name);
            if (entry == null)
            {
                item.IsReuseble = true;
                Slots.Add(new InventoryEntry { Base = item });
            }
            else
            {
                entry.Base.IsReuseble = true;
            }
          
            //if (Slots[item.SlotIndex] == null)
            //{
            //    Slots[item.SlotIndex] = new InventoryEntry { Base = item};
            //}
            //else
            //{
            //    Slots[item.SlotIndex].Base.IsReuseble = true;
            //}
        }

        //public void ToActive(int regularIndex, int activeIndex)
        //{
        //    var regular = Slots[regularIndex];
        //    var active = Slots[activeIndex];

        //    Slots[regularIndex] = active;
        //    Slots[activeIndex] = regular;
        //}

        public bool ActiveItemExistsAt(int index)
        {
            return index < Slots.Count && Slots[index] != null;
        }

        public void Reset()
        {
            for (int i = 0; i < Slots.Count; i++ )
            {
                Slots[i] = null;
            }
            SetDefaultAsset();
        }
    }
}