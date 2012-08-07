using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Components
{
    public class SpikeShopItem: LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default, String.Format("active time: {0}s", Component.Spikes.ActiveTime/1000), Position + new Vector2(TextOffsetLeft, 60), Color.White);
        }

        protected override void LoadComponent()
        {
            base.LoadComponent();
            Component.IsMoving = true;
            Component.SpikesAssetName = UpgradeName;
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Component.Spikes.ActivatedInShop = true;
        }

        public static new SpikeShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new SpikeShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }

        public override void OnBuy()
        {
            var inventory = Game.Services.GetService<InventoryService>();

            if (inventory.Money > Price)
            {
                inventory.Spike = UpgradeName;
                inventory.Money -= Price;
            }
            base.OnBuy();
        }
    }
}
