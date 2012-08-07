using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Tutorial
{
    public class LevelText:  IPositioned
    {
        private Game _game;
        public Vector2 WorldPosition { get; set; }

        public String Text { get; set; }

        protected SpriteBatch SpriteBatch { get; private set; }

        protected SpriteFont Font { get; private set; }

        protected GameState GameState { get; private set; }

        public LevelText(String text, Vector2 worldPosition)
        {
            Text = text;
            WorldPosition = worldPosition;
            Font = Fonts.Tutorial;
        }

        public void Initialize(Game game)
        {
            _game = game;
            SpriteBatch = game.Services.GetService<SpriteBatch>();
            GameState = game.Services.GetService<GameState>();
        }

        protected void LoadContent()
        {
            Font = _game.Content.Load<SpriteFont>("Fonts\\Tutorial");
        }

        public void Draw(GameTime gameTime)
        {
            var measured = Font.MeasureString(Text);


            var scale = 1.0f;

            var origin = new Vector2(measured.X / 2, measured.Y / 2);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend,
                null, null, null, null,
                GameState.Camera.Transform);

            SpriteBatch.DrawString(Font, Text, WorldPosition, Color.White, 0, origin, scale,
                SpriteEffects.None, 1f);

            SpriteBatch.End();
        }
    }
}
