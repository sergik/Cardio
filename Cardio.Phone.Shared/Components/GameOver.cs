using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Components
{
    public class GameOver: DrawableGameComponent
    {
        private GameState _gameState;

        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Vector2 _screenCenter;

        private float _targetFadeInTime = 600;
        private float _currentFadeInTime;

        public String Text
        {
            get; set;
        }

        public GameOver(Game game) : base(game)
        {
            Text = "GAME OVER";
        }

        public override void Initialize()
        {
            _gameState = Game.Services.GetService<GameState>();
            _spriteBatch = Game.Services.GetService<SpriteBatch>();
            _spriteFont = Fonts.Tutorial;

            _screenCenter = new Vector2(Game.GraphicsDevice.Viewport.Width / 2f, Game.GraphicsDevice.Viewport.Height / 2f);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameState.Player.Health <= 0 && !_gameState.IsGodModeEnabled && !_gameState.IsGameOver)
            {
                _gameState.IsGameOver = true;
                _gameState.AddScript(new GameOverScript(Game));
                _gameState.Level.Melody.Stop();
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.IsGameOver)
            {
                float scale = 1f;
                if (_currentFadeInTime <= _targetFadeInTime)
                {
                    scale = _currentFadeInTime / _targetFadeInTime;
                    _currentFadeInTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                _spriteBatch.Begin();
                Shared.Text.Fonts.DrawCenteredText(_spriteBatch, _spriteFont, Text, _screenCenter, scale, Color.White);
                _spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}
