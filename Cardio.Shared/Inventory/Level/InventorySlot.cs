using System;
using Cardio.UI.Core;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Inventory.Level
{
    public class InventorySlot
    {
        private int _itemsCount;

        public int ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                _itemsCount = value;
                CountText = _itemsCount > 1 ? String.Format("x{0}", _itemsCount) : String.Empty;
            }
        }

        public String CountText { get; private set; }

        public String ItemName { get; set; }

        public Texture2D ItemTexture { get; set; }

        public Action<GameState> UseAction { get; set; }

        public bool Use(GameState state)
        {
            if (ItemsCount > 0 && UseAction != null)
            {
                UseAction(state);
                ItemsCount--;
                return true;
            }

            return false;
        }
    }
}
