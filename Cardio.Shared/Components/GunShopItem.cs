using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.UI.Extensions;

namespace Cardio.UI.Components
{
    public class GunShopItem : LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default, String.Format("damage: {0}", Component.Gun.Damage), Position + new Vector2(TEXT_OFFSET_LEFT, 60), Color.White);
        }

        protected override void LoadComponent()
        {
            base.LoadComponent();
            Component.IsShooting = true;
            Component.IsMoving = true;
            Component.GunAssetName = UpgradeName;
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Component.Gun.StopSound();
            var inventory = Game.Services.GetService<InventoryService>();
            BuyButton.Selected += (s, e) =>
            {
                if (inventory.Money > Price)
                {
                    inventory.Gun = UpgradeName;
                    inventory.Money -= Price;
                }
            };
        }

        public static new GunShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new GunShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }
    }
}
