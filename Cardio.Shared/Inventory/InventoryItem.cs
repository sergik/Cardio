using System;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Inventory
{
    public class InventoryItem
    {
        public string Name { get; set; }

        public int SlotIndex { get; set; }

        public Texture2D InventoryTexture { get; set; }

        public int MaxStackSize { get; set; }

        public Point Size { get; set; }

        public Action<GameState> Use { get; set; }

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