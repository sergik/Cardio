using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Inventory.Level;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Components
{
    public class ShieldShopItem : LevelShopItem
    {
        private const int ShieldRadius = 50;
        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default,
                                   String.Format("efficiency: {0}%", (1 - Component.Shield.Efficiency)*100),
                                   Position + new Vector2(TextOffsetLeft, 60), Color.White);
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
        }

        public static new ShieldShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new ShieldShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Component.Shield.Radius = ShieldRadius;
        }

        public override void OnBuy()
        {
            var inventory = Game.Services.GetService<InventoryService>();

            if (inventory.Money > Price)
            {
                    inventory.Shield = UpgradeName;
                    inventory.Money -= Price;
            }
            base.OnBuy();
        }
    }
}
