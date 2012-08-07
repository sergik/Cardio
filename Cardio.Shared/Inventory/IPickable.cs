using System;
using Cardio.UI.Core;

namespace Cardio.UI.Inventory
{
    public interface IPickable: ICollidable
    {
        string InventoryName { get; set; }

        int Count { get; set; }

        bool IsBeingPicked { get; set; }
    }
}
