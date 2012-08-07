using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.UI.Extensions;

namespace Cardio.UI.Components
{
    public class ShieldShopItem : LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default,
                                   String.Format("efficiency: {0}%", (1 - Component.Shield.Efficiency)*100),
                                   Position + new Vector2(TEXT_OFFSET_LEFT, 60), Color.White);
        }

        protected override void LoadComponent()
        {
            base.LoadComponent();
            Component.ShieldAssetName = UpgradeName;
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Component.ActivateShield();
            var inventory = Game.Services.GetService<InventoryService>();
            BuyButton.Selected += (s, e) =>
            {
                if (inventory.Money > Price)
                {
                    inventory.Shield = UpgradeName;
                    inventory.Money -= Price;
                }
            };
        }

        public static new ShieldShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new ShieldShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }
    }
}
