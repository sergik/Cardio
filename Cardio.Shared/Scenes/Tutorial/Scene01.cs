using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Scenes.Tutorial
{
    public class TutorialScene: Scene
    {
        public Texture2D TutorialTexture { get; private set; }

        protected GameState GameState { get; private set; }

        protected SpriteBatch SpriteBatch { get; private set; }

        protected SpriteFont Font { get; private set; }

        public override void Initialize(Game game, SpriteBatch spriteBatch, Components.ICamera2D camera)
        {
            GameState = game.Services.GetService<GameState>();
            SpriteBatch = game.Services.GetService<SpriteBatch>();

            Font = game.Content.Load<SpriteFont>("Fonts\\Tutorial");

            base.Initialize(game, spriteBatch, camera);
        }

        protected void DrawText(String text, Vector2 position)
        {
            var measured = Font.MeasureString(text);
            var origin = new Vector2(measured.X / 2, measured.Y / 2);

            var scale = 1f;

            SpriteBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend,
                null, null, null, null,
                GameState.Camera.Transform);

            SpriteBatch.DrawString(Font, text, position, Color.White, 0, origin, 1f,
                SpriteEffects.None, 1f);

            SpriteBatch.End();
        }
    }

    public class Scene01: TutorialScene
    {
        private float _targetScale;
        private float _targetScaleTime = 500;
        private float _currentScaleTime = 0;

        public override void Initialize(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch, Components.ICamera2D camera)
        {
            camera.FocusedAt = null;
            camera.Position = new Vector2(StartPosition, 0);
            _targetScale = camera.Scale;
            camera.Scale = 1f;

            base.Initialize(game, spriteBatch, camera);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawText("Welcome to Cardio game", new Vector2(StartPosition, 0));

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            var time = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_currentScaleTime < _targetScaleTime)
            {
                var diffTime = _targetScaleTime - _currentScaleTime;
                var diff = _targetScale - GameState.Camera.Scale;
                GameState.Camera.Scale += diff / diffTime * time;
                _currentScaleTime += time;
            }
            
            base.Update(gameTime, gameState);
        }
    }
}
