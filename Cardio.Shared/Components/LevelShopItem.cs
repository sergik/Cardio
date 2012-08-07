using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Characters.Player;
using Cardio.UI.Core;
using Cardio.UI.Input;
using Cardio.UI.Screens;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Cardio.UI.Extensions;

namespace Cardio.UI.Components
{
    public class LevelShopItem
    {
        protected const int TEXT_OFFSET_LEFT = 400;
        protected SpriteBatch SpriteBatch
        { 
            get; set;
        }

        protected string UpgradeName
        {
            get; set;
        }

        public Game Game
        {
            get; private set;
        }

        public Vector2 Position
        {
            get; set;
        }

        public int Price
        {
            get; set;
        }

        public String ItemTitle
        {
            get; set;
        }

        public GrowingMenuEntry BuyButton
        {
            get; set;
        }

        public Nanobot Component
        { 
            get; set;
        }

        public Texture2D Border
        {
            get;
            set;
        }

        public Texture2D Biomaterial
        {
            get; set;
        }

        public virtual void Initialize(Game game)
        {
            Game = game;
            SpriteBatch = game.Services.GetService<SpriteBatch>();

            LoadComponent();
            Component.Initialize(Game, SpriteBatch, null);

            BuyButton = new GrowingMenuEntry("buy");
            BuyButton.Position = Position + new Vector2(1050, 40);
            BuyButton.Initialize(game);

            LoadContent();
        }

        protected virtual void LoadComponent()
        {
            Component = new Nanobot();
            Component.WorldPosition = Position + new Vector2(200, 15);
        }

        public void LoadContent()
        {
            Border = Game.Content.Load<Texture2D>(@"Textures\Borders\Border1");
            Biomaterial = Game.Content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory");
        }

        public void Update(GameTime gameTime)
        {
            Component.Update(null, gameTime);
            BuyButton.Update(null, gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Component.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Border, new Rectangle((int)Position.X + 150, (int)Position.Y, 230, 140), null, Color.White);
            SpriteBatch.Draw(Biomaterial, Position + new Vector2(940, 15), null, Color.White);
            SpriteBatch.DrawString(Fonts.Default, String.Format("x{0}", Price), Position + new Vector2(970, 40), Color.White);
            DrawItemText();
            SpriteBatch.End();

            BuyButton.Draw(null, gameTime);
        }

        protected virtual void DrawItemText()
        {
            SpriteBatch.DrawString(Fonts.Tutorial, ItemTitle.ToLower(), Position + new Vector2(TEXT_OFFSET_LEFT, 20), Color.White);
        }

        public static LevelShopItem FromMetadata(ShopItemMetadata metadata)
        {
            var levelShopItem = new LevelShopItem();
            FillWithMetadata(levelShopItem, metadata);

            return levelShopItem;
        }

        public static void FillWithMetadata(LevelShopItem existingItem, ShopItemMetadata metadata)
        {
            existingItem.Price = metadata.Price;
            existingItem.UpgradeName = metadata.UpgradeName;
            existingItem.ItemTitle = metadata.ItemTitle;
        }

        public void HandleInput()
        {
            if (InputManager.IsMouseButtonTriggered(x => x.LeftButton))
            {
                if (BuyButton.EntryArea.Contains(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y))
                {
                    BuyButton.OnSelectEntry();
                }
            }
        }
    }
}
