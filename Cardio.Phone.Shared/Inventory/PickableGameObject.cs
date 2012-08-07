using System;
using Cardio.Phone.Shared.Core;

namespace Cardio.Phone.Shared.Inventory
{
    public class PickableGameObject: DrawableGameObject, IPickable
    {
        public string InventoryName { get; set; }

        public int Count { get; set; }

        public bool IsBeingPicked { get; set; }
    }
}