using System;
using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Persistance;
using Cardio.Phone.Shared.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Components
{
    public class LevelShopItem
    {
        protected const int TextOffsetLeft = 300;
        protected const int BiomaterialOffsetLeft = 640;
        protected const int BiomaterialOffsetTop = 25;
        protected const int ComponentOffsetLeft = 150;
        protected const int ComponentOffsetTop = 30;

        public bool Installed { get; set; }

        public bool Desabled { get; set; }
        protected SpriteBatch SpriteBatch
        { 
            get; set;
        }

        public string UpgradeName
        {
            get; protected set;
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

        public Nanobot Component
        { 
            get; set;
        }

        public Texture2D Border
        {
            get;
            set;
        }

        public int? ActiveTabIndex { get; set; }

        public Texture2D Biomaterial
        {
            get; set;
        }

        public virtual void Initialize(Game game)
        {
            Game = game;
            SpriteBatch = game.Services.GetService<SpriteBatch>();
            LoadComponent();
            if (Component != null)
            {
                Component.Initialize(Game, SpriteBatch, null);
            }

            LoadContent();
        }

        protected virtual void LoadComponent()
        {
            Component = new Nanobot();
            Component.WorldPosition = Position + new Vector2(ComponentOffsetLeft, ComponentOffsetTop);
        }

        public void LoadContent()
        {
            Border = Game.Content.Load<Texture2D>(@"Textures\Borders\Border2");
            Biomaterial = Game.Content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory");
        }

        public virtual void Update(GameTime gameTime)
        {
            Component.Update(null, gameTime);
        }

        public virtual void DrawContent(GameTime gameTime)
        {
            Component.Draw(gameTime);
        }

        private Color GetBorderColor()
        {
            if(Installed)
            {
                return new Color(0f, 1f, 0f, 0.3f);
            }
            if(Desabled)
            {
                return new Color(1f, 0f, 0f, 0.3f);
            }
            return Color.White;
        }

        public void Draw(GameTime gameTime)
        {
            DrawContent(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Border, new Rectangle((int)Position.X + 120, (int)Position.Y + 10, 150, 100), null, GetBorderColor());
            SpriteBatch.Draw(Biomaterial, Position + new Vector2(BiomaterialOffsetLeft, BiomaterialOffsetTop), null, Color.White);
            SpriteBatch.DrawString(Fonts.Default, String.Format("x{0}", Price),
                                   Position + new Vector2(BiomaterialOffsetLeft + 15, BiomaterialOffsetTop + 10),
                                   Color.White);
            DrawItemText();
            SpriteBatch.End();

        }

        protected virtual void DrawItemText()
        {
            SpriteBatch.DrawString(Fonts.Tutorial, ItemTitle.ToLower(), Position + new Vector2(TextOffsetLeft, 20), Color.White);
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

        public virtual void OnBuy()
        {
            SavedGame.Save(Game.Services.GetService<GameState>());
        }

        protected void ShowConfirmationDialog()
        {
            var confirmationScreen = new ConfirmationScreen(Game){ShopSelectedCategory = ActiveTabIndex};

            confirmationScreen.Item = this;
            var screenManager = (ScreenManager)Game.Services.GetService(typeof(ScreenManager));
            LoadingScreen.Load(screenManager, false, confirmationScreen);
        }

        public void HandleInput()
        {
            if (InputManager.IsMouseButtonTriggered(x => x.LeftButton))
            {
                if (new Rectangle((int)Position.X + 120, (int)Position.Y + 10, 150, 100).Contains(
                    InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y))
                {
                    if (!Installed && !Desabled)
                    {
                        ShowConfirmationDialog();
                    }
                }
            }
        }
    }
}
