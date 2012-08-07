using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Text
{
    public class TextRenderer
    {
        public SpriteFont Font { get; set; }

        private SpriteBatch _spriteBatch;

        public String Text { get; set; }

        public Color Color { get; set; }

        public Vector2 Position { get; set; }

        public TextRenderer()
        {
            Text = String.Empty;
            Position = Vector2.Zero;
            Color = Color.White;
            Font = Shared.Fonts.Default;
        }

        public void Initialize(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Font, Text, Position, Color);
            _spriteBatch.End();
        }
    }
}
