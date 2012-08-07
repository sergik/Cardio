using System;
using Cardio.Phone.Shared.Core;

namespace Cardio.Phone.Shared.Inventory
{
    public interface IPickable: ICollidable
    {
        string InventoryName { get; set; }

        int Count { get; set; }

        bool IsBeingPicked { get; set; }
    }
}
