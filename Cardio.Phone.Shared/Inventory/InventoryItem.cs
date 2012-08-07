using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Inventory
{
    public class InventoryItem
    {
        public string Name { get; set; }

        public int SlotIndex { get; set; }

        public Texture2D InventoryTexture { get; set; }

        public int MaxStackSize { get; set; }

        public Point Size { get; set; }

        public event Action<GameState> Use;

        public bool IsReuseble { get; set; }

        public int ReuseTime { get; set; }

        public int ColdownTime { get; set; }

        public void OnUse(GameState state)
        {
            if(IsReuseble)
            {
                IsEnabled = false;
                ColdownTime = ReuseTime;
            }
            if (Use != null)
            {
                Use(state);
            }
        }

        public bool IsUsable
        {
            get { return Use != null; }
        }

        public bool IsEnabled { get; set; }

        public InventoryItem()
        {
            Size = new Point(1, 1);
            IsEnabled = true;
        }
    }
}