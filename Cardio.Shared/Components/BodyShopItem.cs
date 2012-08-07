using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.UI.Extensions;

namespace Cardio.UI.Components
{
    public class BodyShopItem: LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default, String.Format("max health: {0}", Component.Body.MaxHealth), Position + new Vector2(TEXT_OFFSET_LEFT, 60), Color.White);
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            var inventory = game.Services.GetService<InventoryService>();
            BuyButton.Selected += (s, e) =>
            {
                if (inventory.Money > Price)
                {
                    inventory.Body = UpgradeName;
                    inventory.Money -= Price;
                }
            };
        }

        protected override void LoadComponent()
        {
            base.LoadComponent();
            Component.IsMoving = true;
            Component.BodyAssetName = UpgradeName;
        }

        public static new BodyShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new BodyShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }
    }
}
