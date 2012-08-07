using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Heartbeat;
using Cardio.Phone.Shared.Inventory.Level;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.GUI
{
    public class LevelUI: DrawableGameComponent
    {
        private Game _game;

        private InventoryComponent _inventoryComponent;

        private ProgressBar _playerHealthBar;

        private GameOver _gameOver;

        private LevelFinished _levelFinished;

        private GameState _gameState;

        private SpriteBatch _spriteBatch;

        public LevelUI(Game game) : base(game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            _gameState = _game.Services.GetService<GameState>();
            // Game Over
            _gameOver = new GameOver(Game);
            _gameOver.Initialize();


            // Level finished
            _levelFinished = new LevelFinished(Game);
            _levelFinished.Initialize();


            _inventoryComponent = new InventoryComponent(Game);
            _inventoryComponent.Initialize();

            _spriteBatch = Game.Services.GetService<SpriteBatch>();

            // Player health
            _playerHealthBar = new ProgressBar(Game, @"Textures\UI\Health", @"Textures\UI\HealthVessel")
            {
                DestinationRectangle = new Rectangle(20, 10, 250, 20),
                Max = _gameState.Player.MaxHealth
            };
            _playerHealthBar.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
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
                _spriteBatch.Begin();
                _playerHealthBar.Draw(gameTime);
                _spriteBatch.End();
            }

            if (_gameState.IsInventoryEnabled)
            {
                _inventoryComponent.Draw(gameTime);
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
