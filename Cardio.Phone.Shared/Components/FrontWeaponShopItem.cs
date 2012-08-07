using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Inventory.Level;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Components
{
    public class FrontWeaponShopItem: LevelShopItem
    {
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default, String.Format("damage: {0}", Component.FrontWeapon.Damage), Position + new Vector2(TextOffsetLeft, 60), Color.White);
        }

        protected override void LoadComponent()
        {
            base.LoadComponent();
            Component.IsShooting = false;
            Component.IsMoving = true;
            Component.FrontWeaponAssetName = UpgradeName;
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Component.Gun.StopSound();
            Component.Gun.ShouldGenerateBullets = false;
        }

        public static new FrontWeaponShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new FrontWeaponShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }

        public override void OnBuy()
        {
            var inventory = Game.Services.GetService<InventoryService>();

            if (inventory.Money > Price)
            {
                inventory.FrontWeapon = UpgradeName;
                inventory.Money -= Price;
            }
            base.OnBuy();
        }
    }
}
