using System;
using Cardio.Phone.Shared;
using Cardio.Phone.Shared.Actions;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Input.Touch;
using Cardio.Phone.Shared.Inventory;
using Cardio.Phone.Shared.Screens;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.UI
{
    using Microsoft.Advertising.Mobile.Xna;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CardioGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private static readonly string ApplicationId = "318c0a5c-ae84-4356-924c-c0bf47597aaa";

        //private const string TwitterSettingsFile = "twitter.xml";

        public CardioGame()
        {
            TargetElapsedTime = TimeSpan.FromTicks(166666);

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480,
                IsFullScreen = true
            };
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Items.Initialize(Content);
            AdGameComponent.Initialize(this, ApplicationId);
            Components.Add(AdGameComponent.Current);
            GameState gameState = TestData.CreateDefaultGameState(this);
            Services.AddService(gameState);

            // Inventory
            Services.AddService(gameState.Inventory);

            var cache = new GameCache();
            Services.AddService(cache);

            var inventoryMappingService = new InventoryMappingService();
            Services.AddService(inventoryMappingService);

            InputManager.Initialize();
            Fonts.Initialize(this);
            SoundManager.Initialize(this);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof (SpriteBatch), _spriteBatch);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);
            Services.AddService(typeof(ScreenManager), _screenManager);

            var menuBackgroundComponent = new MenuBackgroundComponent(this, _screenManager);
            menuBackgroundComponent.DrawOrder = -1;
            Components.Add(menuBackgroundComponent);


            base.Initialize();
            LoadingScreen.Load(_screenManager, false, new AlternativeMenuScreen());
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
