using System.Linq;
using Cardio.UI.Actions;
using Cardio.UI.Core;
using Cardio.UI.Effects.Bloom;
using Cardio.UI.Extensions;
using Cardio.UI.GUI;
using Cardio.UI.Input;
using Cardio.UI.Input.Matches;
using Cardio.UI.Input.Touch;
using Cardio.UI.Inventory;
using Cardio.UI.Levels;
using Cardio.UI.Rhythm;
using Cardio.UI.Screens.LevelMenu;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Cardio.UI.Screens
{
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

        private RhythmEngine _rhythmEngine;
        private ReactionsCatalog _reactionCatalog;

        private BloomComponent _bloom;

        private LevelUI _levelUI;

        private bool _isPaused;

        public override void LoadContent()
        {
            var game = ScreenManager.Game;
            GameState = game.Services.GetService<GameState>();

            // *********************************************************
            // DO NOT forget to remove all added here
            // services and game components on UnloadContent
            // *********************************************************

            var services = ScreenManager.Game.Services;

            services.AddService(ScreenManager);

            // Rhythm engine
            _rhythmEngine = new RhythmEngine(ScreenManager.Game);
            _rhythmEngine.Initialize();
            services.AddService(_rhythmEngine);


            _reactionCatalog = new ReactionsCatalog();
            services.AddService(_reactionCatalog);

            // Input
            _touchService = new TouchService();
            services.AddService(_touchService);


            _inputHandler = new WindowsInputHandler(ScreenManager.Game, this);
            _inputHandler.Initialize();

            _comboController = new ComboController(100);
            GameState.Combo = _comboController;

            GameState.Player.AddScript(new CheckForPickableObjectsScript(game));

            // Resources
            services.AddService(new ResourceBuilder(new InventoryMappingService()));

            Level.Initialize(ScreenManager.Game, ScreenManager.SpriteBatch, GameState.Camera, GameState);

            _comboController.Melodies.Clear();
            _comboController.Melodies[0] =
                new ComboLevelMelody(ScreenManager.Game.Content.Load<SoundEffect>(Level.MelodyAsset));
            _comboController.Initialize(ScreenManager.Game);

            GameState.Camera.Initialize(game);
            GameState.Player.Initialize(game, ScreenManager.SpriteBatch);
            _levelUI = new LevelUI(ScreenManager.Game);
            _levelUI.Initialize();

            // Effects
            _bloom = new BloomComponent(ScreenManager.Game);
            _bloom.Initialize();
            services.AddService(_bloom);

            InitializeReactions();
            WireUpEvents();

            ApplyDefaultMatchingStrategy();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            var services = ScreenManager.Game.Services;

            GameState.ClearScripts();
            _reactionCatalog.ClearReactions();

            _rhythmEngine.MelodyPlayer.RemoveMelody(true);
            _rhythmEngine.PatternGenerator.IsEnabled = false;

            services.RemoveService<ReactionsCatalog>();
            services.RemoveService<ScreenManager>();
            services.RemoveService<ITouchService>();
            services.RemoveService<RhythmEngine>();
            services.RemoveService<ResourceBuilder>();
            services.RemoveService<BloomComponent>();
        }

        private void InitializeReactions()
        {
            _reactionCatalog.RegisterReaction(PlayerAction.Move, ActionReaction.MoveReaction(Level, GameState));
            _reactionCatalog.RegisterReaction(PlayerAction.Shoot, ActionReaction.ShootReaction(Level, GameState));
            _reactionCatalog.RegisterReaction(PlayerAction.Evade, ActionReaction.EvadeReaction(GameState));

            _reactionCatalog.ReactionInvoked += (o, e) => GameState.Combo.IncreaseComboLevel();
        }

        private void WireUpEvents()
        {
            _reactionCatalog.ReactionInvoked +=
                (o, e) => GameState.ReactionProgress.StartProgress(e.ReactionInvoked.Duration);

            Level.Finished += (s, e) => LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen());

            GameState.Player.Confused += (o, e) => GameState.ReactionProgress.Reset();
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

            _bloom.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Resume()
        {
            Sounds.SoundManager.ResumeAll();
            _isPaused = false;
        }

        public override void Draw(GameTime gameTime)
        {
            _bloom.BeginDraw();
            ScreenManager.Game.GraphicsDevice.Clear(Color.Black);

            Level.Draw(gameTime, GameState);

            GameState.Player.Draw(gameTime);

            _bloom.Draw(gameTime);

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
                GameState.Player.Confuse();
            };
            _inputHandler.MatchStrategy = strategy;
        }

        public void ApplyTouchSequenceMathchingStrategy()
        {
            var strategy = new TouchSequenceMatchStrategy(GameState, Enumerable.Repeat(ButtonType.Left, 123).ToList());
            _inputHandler.MatchStrategy = strategy;
        }
    }
}
