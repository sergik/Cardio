using System;
using Cardio.Phone.Shared.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Components
{
    public class GunShopItem : LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default, String.Format("damage: {0}", Component.Gun.Damage), Position + new Vector2(TextOffsetLeft, 60), Color.White);
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
            Component.Gun.ShouldGenerateBullets = false;
        }

        public static new GunShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new GunShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }

        public override void OnBuy()
        {
            var inventory = Game.Services.GetService<InventoryService>();

            if (inventory.Money > Price)
            {
                inventory.Gun = UpgradeName;
                inventory.Money -= Price;
            }
            base.OnBuy();
        }
    }
}
