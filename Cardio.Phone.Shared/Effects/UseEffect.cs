using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Effects
{
    public class UseEffect : DrawableGameObject
    {
        public Color Color { get; set; }

        public Rectangle DrawRectangle { get; set; }

        public Texture2D Texture { get; set; }

        public override void Initialize(Game game, SpriteBatch spriteBatch, Components.ICamera2D camera)
        {
            SpriteBatch = spriteBatch;
            DrawRectangle = new Rectangle(0, 0, (int) camera.ViewportWidth, (int) camera.ViewportHeight);
            Texture = new Texture2D(game.GraphicsDevice, 1, 1);
            Texture.SetData(new[] { new Color(255, 255, 255, 0) });
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, DrawRectangle, Color);
            SpriteBatch.End();
        }


    }
}
