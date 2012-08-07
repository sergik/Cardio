using System;
using Cardio.UI.Core;

namespace Cardio.UI.Inventory
{
    public class PickableGameObject: DrawableGameObject, IPickable
    {
        public string InventoryName { get; set; }

        public int Count { get; set; }

        public bool IsBeingPicked { get; set; }
    }
}