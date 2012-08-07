using System;
using System.Linq;
using Cardio.Phone.Shared.Actions;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Rhythm;
using Cardio.Phone.Shared.Actions;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.GUI;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Input.Matches;
using Cardio.Phone.Shared.Input.Touch;
using Cardio.Phone.Shared.Inventory;
using Cardio.Phone.Shared.Levels;
using Cardio.Phone.Shared.Screens.LevelMenu;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Screens
{
    using Microsoft.Advertising.Mobile.Xna;
    using Microsoft.Xna.Framework.Graphics;

    public class LevelScreen: GameScreen
    {
        public Level Level
        {
            get { return GameState.Level; }
            set { GameState.Level = value; }
        }

        public GameState GameState { get; private set; }

        private ITouchService _touchService;
        private WindowsInputHandler _inputHandler;
        private ComboController _comboController;
        private DrawableAd bannerAd;
        private static readonly string AdUnitId = "10041188"; 

        private RhythmEngine _rhythmEngine;

        //private BloomComponent _bloom;

        private LevelUI _levelUI;

        private bool _isPaused;

        public override void LoadContent()
        {
            this.CreateAd();
            var game = ScreenManager.Game;
            GameState = game.Services.GetService<GameState>();

            // *********************************************************
            // DO NOT forget to remove all added here
            // services and game components on UnloadContent
            // *********************************************************

            var services = ScreenManager.Game.Services;

            // Rhythm engine
            _rhythmEngine = new RhythmEngine(ScreenManager.Game);
            _rhythmEngine.Initialize();
            services.AddService(_rhythmEngine);


            _touchService = new TouchService();
            services.AddService(_touchService);


            _inputHandler = new WindowsInputHandler(ScreenManager.Game, this);
            _inputHandler.Initialize();

            _comboController = new ComboController(100);
            GameState.Combo = _comboController;
            _comboController.Initialize(ScreenManager.Game);

            GameState.Player.AddScript(new CheckForPickableObjectsScript(game));

            // Resources
            services.AddService(new ResourceBuilder(new InventoryMappingService()));

            Level.Initialize(ScreenManager.Game, ScreenManager.SpriteBatch, GameState.Camera, GameState);

            GameState.Camera.Initialize(game);
            GameState.Player.Initialize(game, ScreenManager.SpriteBatch);
            _levelUI = new LevelUI(ScreenManager.Game);
            _levelUI.Initialize();

            //// Effects
            //_bloom = new BloomComponent(ScreenManager.Game);
            //_bloom.Initialize();
            //services.AddService(_bloom);

            WireUpEvents();

            ApplyDefaultMatchingStrategy();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            var services = ScreenManager.Game.Services;

            GameState.ClearScripts();

            _rhythmEngine.MelodyPlayer.RemoveMelody(true);
            _rhythmEngine.PatternGenerator.IsEnabled = false;

            services.RemoveService<ReactionsCatalog>();
            services.RemoveService<ITouchService>();
            services.RemoveService<RhythmEngine>();
            services.RemoveService<ResourceBuilder>();
            Level.Melody.Stop();
            GC.Collect();
            //services.RemoveService<BloomComponent>();
        }

        private void WireUpEvents()
        {
            Level.Finished += (s, e) => LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen());
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (coveredByOtherScreen)
            {
                base.Update(gameTime, otherScreenHasFocus, false);
                return;
            }

            if (_isPaused)
            {
                Resume();
            }

            GameState.HandledMouseOnThisUpdate = false;

            _levelUI.Update(gameTime);

            Level.Update(gameTime, GameState);

            if (GameState.AreControlsEnabled && !Level.IsFinished)
            {
                _inputHandler.Update(gameTime);

                GameState.ReactionProgress.Update(gameTime);
                _comboController.Update(gameTime);
            }


            GameState.Camera.Update(gameTime);
            Level.Background.Position = (int)GameState.Camera.Position.X;

            GameState.Player.Update(GameState, gameTime);

            //_bloom.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Resume()
        {
            Sounds.SoundManager.ResumeAll();
            _isPaused = false;
        }

        public override void Draw(GameTime gameTime)
        {
            //_bloom.BeginDraw();
            ScreenManager.Game.GraphicsDevice.Clear(Color.Black);

            Level.Draw(gameTime, GameState);

            GameState.Player.Draw(gameTime);

            //_bloom.Draw(gameTime);

           _levelUI.Draw(gameTime);

            if (GameState.AreControlsEnabled)
            {
                _inputHandler.Draw(gameTime); 
            }

            if (_isPaused)
            {
                ScreenManager.FadeBackBufferToBlack(0.5f);
            }

            base.Draw(gameTime);
        }

        public void Pause()
        {
            Sounds.SoundManager.PauseAll();
            _isPaused = true;
        }

        public void ApplyDefaultMatchingStrategy()
        {
            var strategy = new DefaultMatchStrategy(GameState);
            strategy.Failed += (o, e) =>
            {
                _comboController.Reset();
            };
            _inputHandler.MatchStrategy = strategy;
        }

        public void ApplyTouchSequenceMathchingStrategy()
        {
            var strategy = new TouchSequenceMatchStrategy(GameState, Enumerable.Repeat(ButtonType.Left, 123).ToList());
            _inputHandler.MatchStrategy = strategy;
        }

        private void CreateAd()
        {
            // Create a banner ad for the game.
            int width = 480;
            int height = 80;
            int x = (ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Width - width) / 2; // centered on the display
            int y = 5;

            bannerAd = AdGameComponent.Current.CreateAd(AdUnitId, new Rectangle(x, y, width, height), true);
        }
    }
}
