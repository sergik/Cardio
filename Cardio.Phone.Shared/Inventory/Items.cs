using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Inventory
{
    public static class Items
    {
        //public static InventoryItem SmallMedkit { get; private set; }

        public static InventoryItem SmallShield { get; private set; }

        public static InventoryItem Oxygen { get; private set; }

        public static InventoryItem BloodElement { get; private set; }

        public static InventoryItem DnaPiece { get; private set; }

        public static InventoryItem Spike { get; private set; }

        public static void Initialize(ContentManager content)
        {
            //SmallMedkit = Medcit.FromMetadata(content.Load<MedcitMetadata>(@""), content);
            //    new InventoryItem
            //{
            //    InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\Medkit"),
            //    MaxStackSize = 64,
            //    Name = "Medkit",
            //    SlotIndex = 2,
            //    IsReuseble = true,
            //    ReuseTime = 5000
            //};
            //SmallMedkit.Use += UseMedkit;

            SmallShield = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\Shield"),
                MaxStackSize = 64,
                Name = "Shields",
                SlotIndex = -1,
                IsReuseble = true,
                ReuseTime = 5000
            };
            SmallShield.Use += UseShield;

            Oxygen = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory"),
                MaxStackSize = 64,
                Name = "Oxygen",
                SlotIndex = 0
            };

            BloodElement = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory"),
                MaxStackSize = 64,
                Name = "Blood Element",
                SlotIndex = 0,
            };

            DnaPiece = new InventoryItem
            {
                InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory"),
                MaxStackSize = 64,
                Name = "Dna Piece",
                SlotIndex = 0
            };

            Spike = new InventoryItem
                        {
                            InventoryTexture = content.Load<Texture2D>(@"Textures\Inventory\Spike"),
                            MaxStackSize = 64,
                            Name = "Spike",
                            SlotIndex = -1,
                            IsReuseble = true,
                            ReuseTime = 10000
                        };
            Spike.Use += UseSpike;
        }

        private static void UseShield(GameState state)
        {
            SmallShield.IsEnabled = false;

            var script = new UsePlayerShieldScript();

            state.AddScript(script);
        }

        private static void UseSpike(GameState state)
        {
            state.Player.Nanobot.Spikes.Activate();
        }
    }
}
