using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Heartbeat;
using Cardio.UI.Inventory.Level;
using Cardio.UI.Extensions;
using Microsoft.Xna.Framework;

namespace Cardio.UI.GUI
{
    public class LevelUI: DrawableGameComponent
    {
        private Game _game;
        private InputSequenceDisplay _inputDisplay;

        private HeartbeatComponent _heartbeatComponent;
        
        private InventoryComponent _inventoryComponent;

        private ProgressBar _playerHealthBar;

        private GameOver _gameOver;

        private LevelFinished _levelFinished;

        private GameState _gameState;

        public LevelUI(Game game) : base(game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            _gameState = _game.Services.GetService<GameState>();
            _inputDisplay = new InputSequenceDisplay(Game);
            _inputDisplay.Initialize();

            // Game Over
            _gameOver = new GameOver(Game);
            _gameOver.Initialize();


            // Level finished
            _levelFinished = new LevelFinished(Game);
            _levelFinished.Initialize();


            _inventoryComponent = new InventoryComponent(Game);
            _inventoryComponent.Initialize();


            _heartbeatComponent = new HeartbeatComponent(Game);
            _heartbeatComponent.Initialize();


            // Player health
            _playerHealthBar = new ProgressBar(Game, @"Textures\UI\Health", @"Textures\UI\HealthVessel")
            {
                DestinationRectangle = new Rectangle(64, 24, 250, 24),
                Max = _gameState.Player.MaxHealth
            };
            _playerHealthBar.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameState.AreControlsEnabled && !_gameState.Level.IsFinished)
            {
                _inputDisplay.Update(gameTime);
            }

            if (_gameState.IsHeartbeatEnabled)
            {
                _heartbeatComponent.Update(gameTime);
            }
            else
            {
                _heartbeatComponent.Pause();
            }

            if (_gameState.IsInventoryEnabled)
            {
                _inventoryComponent.Update(gameTime);
            }

            if (_gameState.IsHealthBarEnabled)
            {
                _playerHealthBar.Value = _gameState.Player.Health;
                _playerHealthBar.Update(gameTime);
            }

            if (_gameState.Level.IsFinished)
            {
                _levelFinished.Update(gameTime);
            }

            _gameOver.Update(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.IsHealthBarEnabled)
            {
                _playerHealthBar.Draw(gameTime);
            }

            if (_gameState.IsHeartbeatEnabled)
            {
                _heartbeatComponent.Draw(gameTime);
            }

            if (_gameState.IsInventoryEnabled)
            {
                _inventoryComponent.Draw(gameTime);
            }

            if (_gameState.AreControlsEnabled)
            {
                _inputDisplay.Draw(gameTime);
            }

            _gameOver.Draw(gameTime);

            if (_gameState.Level.IsFinished)
            {
                _levelFinished.Draw(gameTime);
            }
            
            base.Draw(gameTime);
        }
    }
}
