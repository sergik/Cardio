using System;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.GUI
{
    public class ProgressBar: DrawableGameComponent
    {
        public float Min { get; set; }

        public float Max { get; set; }

        public float Value { get; set; }

        public string TextureName { get; private set; }
        public string BackTextureName { get; private set; }

        public Texture2D Texture { get; private set; }

        public Texture2D BackTexture { get; private set; }

        public Rectangle DestinationRectangle { get; set; }

        private Rectangle _sourceRectangle;
        private Rectangle _currentDestinationRectangle;

        private SpriteBatch _spriteBatch;

        public ProgressBar(Game game, string textureName) : base(game)
        {
            TextureName = textureName;
        }

        public ProgressBar(Game game, string textureName, string backTextureName)
            : base(game)
        {
            TextureName = textureName;
            BackTextureName = backTextureName;
        }

        public override void Initialize()
        {
            _spriteBatch = Game.Services.GetService<SpriteBatch>();

            Texture = Game.Content.Load<Texture2D>(TextureName);
            if (BackTextureName != null)
            {
                BackTexture = Game.Content.Load<Texture2D>(BackTextureName);
            }

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            _sourceRectangle = new Rectangle(0,0, (int) (Value / (Max - Min) * Texture.Width), Texture.Height);
            _currentDestinationRectangle = new Rectangle(DestinationRectangle.X, DestinationRectangle.Y, (int) (Value / (Max - Min) * DestinationRectangle.Width), DestinationRectangle.Height);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
          if (BackTexture != null)
            {
                _spriteBatch.Draw(BackTexture, DestinationRectangle, new Rectangle(0, 0, BackTexture.Width, BackTexture.Height), Color.White);
            }
            _spriteBatch.Draw(Texture, _currentDestinationRectangle, _sourceRectangle, Color.White);
            
            base.Draw(gameTime);
        }
    }
}
