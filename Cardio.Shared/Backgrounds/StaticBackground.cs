using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Backgrounds
{
    public class StaticBackground: DrawableGameComponent
    {
        private readonly String _textureName;
        public Texture2D Texture { get; private set; }

        private SpriteBatch _spriteBatch;

        private Rectangle _targetRectangle;

        public StaticBackground(Game game, String textureName) : base(game)
        {
            _textureName = textureName;
        }

        protected override void LoadContent()
        {
            Texture = Game.Content.Load<Texture2D>(_textureName);
            
            base.LoadContent();
        }

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            _targetRectangle = new Rectangle(0,0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Texture, _targetRectangle, Color.White);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
