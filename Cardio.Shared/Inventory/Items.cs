using System;
using Cardio.UI.Core;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Inventory
{
    public static class Items
    {
        public static InventoryItem SmallMedkit { get; private set; }

        public static InventoryItem SmallShield { get; private set; }

        public static InventoryItem Oxygen { get; private set; }

        public static InventoryItem BloodElement { get; private set; }

        public static InventoryItem DnaPiece { get; private set; }

        public static void Initialize(ContentManager content)
        {
            SmallMedkit = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\Medkit"),
                MaxStackSize = 64,
                Name = "Medkit",
                Use = UseMedkit,
                SlotIndex = 0
            };

            SmallShield = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\Shield"),
                MaxStackSize = 64,
                Name = "Shields",
                Use = UseShield,
                SlotIndex = 1
            };

            Oxygen = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory"),
                MaxStackSize = 64,
                Name = "Oxygen",
                SlotIndex = 2
            };

            BloodElement = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory"),
                MaxStackSize = 64,
                Name = "Blood Element",
                SlotIndex = 2
            };

            DnaPiece = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory"),
                MaxStackSize = 64,
                Name = "DNA Piece",
                SlotIndex = 2
            };
        }

        private static void UseMedkit(GameState state)
        {
            SmallMedkit.IsEnabled = false;
            
            var script = new HealPlayerScript(80);
            script.Finished += (s, e) => SmallMedkit.IsEnabled = true;

            state.AddScript(script);
        }

        private static void UseShield(GameState state)
        {
            SmallShield.IsEnabled = false;

            var script = new UsePlayerShieldScript();
            script.Finished += (s, e) => SmallShield.IsEnabled = true;

            state.AddScript(script);
        }
    }
}
