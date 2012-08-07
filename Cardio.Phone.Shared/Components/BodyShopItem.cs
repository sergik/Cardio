using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Components
{
    public class BodyShopItem: LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default, String.Format("max health: {0}", Component.Body.MaxHealth), Position + new Vector2(TextOffsetLeft, 60), Color.White);
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

        public override void OnBuy()
        {
            var inventory = Game.Services.GetService<InventoryService>();

            if (inventory.Money > Price)
            {
                inventory.Body = UpgradeName;
                inventory.Money -= Price;
            }
            base.OnBuy();
        }
    }
}
