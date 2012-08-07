using System;
using Cardio.UI.Actions;
using Cardio.UI.Core;
using Cardio.UI.Input;
using Cardio.UI.Input.Touch;
using Cardio.UI.Inventory;
using Cardio.UI.Scenes.Actions;
using Cardio.UI.Screens;
using Cardio.UI.Sounds;
using Cardio.UI.Twitter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cardio.UI.Extensions;

namespace Cardio.UI
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CardioGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const string TwitterSettingsFile = "twitter.xml";

        public CardioGame()
        {
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / 60.0);

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = false
            };
            _graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Items.Initialize(Content);

            GameState gameState = TestData.CreateDefaultGameState(this);
            Services.AddService(gameState);

            // Inventory
            Services.AddService(gameState.Inventory);

            var inventoryMappingService = new InventoryMappingService();
            Services.AddService(inventoryMappingService);


            InputManager.Initialize();
            Fonts.Initialize(this);
            SoundManager.Initialize(this);

            var twitter = TwitterProxy.From(TwitterSettingsFile);
            twitter.Initialize();
            Services.AddService(typeof(ITwitter), twitter);

            Exiting += (o, e) => twitter.SaveTo(TwitterSettingsFile);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof (SpriteBatch), _spriteBatch);

            var screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            var menuBackgroundComponent = new MenuBackgroundComponent(this, screenManager);
            menuBackgroundComponent.DrawOrder = -1;
            Components.Add(menuBackgroundComponent);

            InitActions();

            base.Initialize();

            LoadingScreen.Load(screenManager, false, new MainMenuScreen());
        }

        private static void InitActions()
        {
            ActionCatalog.AddAction(PlayerAction.Move,
                new[] {ButtonType.Right, ButtonType.Right, ButtonType.Right, ButtonType.Right});
            ActionCatalog.AddAction(PlayerAction.Shoot,
                new[] {ButtonType.Left, ButtonType.Left, ButtonType.Left, ButtonType.Left});
            ActionCatalog.AddAction(PlayerAction.Evade,
                new[] {ButtonType.Left, ButtonType.Right, ButtonType.Left, ButtonType.Right});
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.IsKeyTriggered(Keys.F))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
