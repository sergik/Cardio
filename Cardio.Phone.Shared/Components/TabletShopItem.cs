using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Inventory;
using Cardio.Phone.Shared.Inventory.Level;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Components
{
    public class TabletShopItem : LevelShopItem
    {
        public const int MedcitShopItemOffset = 15;

        public Tablet Tablet { get; set; }

        public Rectangle DrawRectangle { get; set; }

        protected override void DrawItemText()
        {
            base.DrawItemText();
            SpriteBatch.DrawString(Fonts.Default,
                                   String.Format("heals: {0}", Tablet.Damage),
                                   Position + new Vector2(TextOffsetLeft, 60), Color.White);
        }

        protected override void LoadComponent()
        {
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Tablet = Tablet.FromMetadata(game.Content.Load<TabletMetadata>(UpgradeName), game.Content);
            DrawRectangle = new Rectangle((int)(Position.X + ComponentOffsetLeft + MedcitShopItemOffset), (int)(Position.Y + ComponentOffsetTop), 64, 64);
        }

        public static new TabletShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var item = new TabletShopItem();
            FillWithMetadata(item, metadata);
            return item;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void DrawContent(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Tablet.InventoryTexture, DrawRectangle, Color.White);
            SpriteBatch.End();
        }

        public override void OnBuy()
        {
            var inventory = Game.Services.GetService<InventoryService>();

            if (inventory.Money > Price)
            {
                inventory.Tablet = UpgradeName;
                inventory.Money -= Price;
            }
            base.OnBuy();
        }
    }
}
