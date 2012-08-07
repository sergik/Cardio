using System;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Components
{
    public class LevelFinished: DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;

        private SpriteFont _spriteFont;

        private float _targetFadeInTime = 600;
        private float _currentFadeInTime;

        public String Text { get; set; }

        private Vector2 _screenCenter;

        public LevelFinished(Game game) : base(game)
        {
            Text = "LEVEL COMPLETED";
        }

        public override void Initialize()
        {
            _spriteBatch = Game.Services.GetService<SpriteBatch>();
            _spriteFont = Fonts.Tutorial;

            _screenCenter = new Vector2(Game.GraphicsDevice.Viewport.Width / 2f, Game.GraphicsDevice.Viewport.Height / 2f);

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = 1f;

            if (_currentFadeInTime < _targetFadeInTime)
            {
                scale = _currentFadeInTime / _targetFadeInTime;

                _currentFadeInTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            _spriteBatch.Begin();
            Phone.Shared.Text.Fonts.DrawCenteredText(_spriteBatch, _spriteFont, Text, _screenCenter, scale, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
