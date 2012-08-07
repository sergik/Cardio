using System;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Backgrounds
{
    public abstract class ScrollingBackground
    {
        protected Game Game { get; private set; }

        private SpriteBatch _spriteBatch;
        private ICamera2D _camera;

        public Texture2D Texture { get; set; }

        public virtual int Position { get; set; }

        private Rectangle _primarySourceRectangle;
        private Rectangle _primaryDestinationRectangle;
        private Rectangle? _additionalSourceRectangle;
        private Rectangle? _additionalDestinationRectangle;

        public int VirtualWidth { get; set; }
        public int VirtualHeight { get; set; }

        protected ScrollingBackground()
        {
            VirtualWidth = 2000;
            VirtualHeight = 800;
        }

        public void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            Game = game;
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        public void Update(GameTime gameTime)
        {
            var primaryStart = Position % VirtualWidth;
            if (primaryStart < 0)
            {
                primaryStart = VirtualWidth - Math.Abs(primaryStart);
            }

            var primaryWidth = VirtualWidth - primaryStart;
            if (primaryWidth >= VirtualWidth)
            {
                primaryWidth = VirtualWidth;
                _additionalSourceRectangle = null;
                _additionalDestinationRectangle = null;
            }
            else
            {
                _additionalSourceRectangle = new Rectangle(0, 0, VirtualWidth - primaryWidth, VirtualHeight);
                _additionalDestinationRectangle = new Rectangle(primaryWidth % VirtualWidth, 0, _additionalSourceRectangle.Value.Width, _additionalSourceRectangle.Value.Height);
            }

            _primarySourceRectangle = new Rectangle(primaryStart, 0, primaryWidth, VirtualHeight);

            _primaryDestinationRectangle = new Rectangle((int) (_camera.Position.X - VirtualWidth / 2f), (int) (_camera.Position.Y - VirtualHeight / 2f), _primarySourceRectangle.Width, _primarySourceRectangle.Height);

            if (_additionalDestinationRectangle != null)
            {
                var rect = _additionalDestinationRectangle.Value;
                rect.X += _primaryDestinationRectangle.X;
                rect.Y += _primaryDestinationRectangle.Y;
                _additionalDestinationRectangle = rect;
            }

            if (_additionalSourceRectangle != null)
            {
                _additionalSourceRectangle = TranslateToTextureCoordinates(_additionalSourceRectangle.Value);
            }

            _primarySourceRectangle = TranslateToTextureCoordinates(_primarySourceRectangle);
        }

        private Rectangle TranslateToTextureCoordinates(Rectangle input)
        {
            var xRatio = (float) Texture.Width / VirtualWidth;
            var yRatio = (float) Texture.Height / VirtualHeight;

            var result = new Rectangle((int) (input.X * xRatio), (int) (input.Y * yRatio), (int) (input.Width * xRatio), (int) (input.Height * yRatio));
            return result;
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        _camera.Transform);
            _spriteBatch.Draw(Texture, _primaryDestinationRectangle, _primarySourceRectangle, Color.White);

            if (_additionalSourceRectangle.HasValue && _additionalDestinationRectangle.HasValue)
            {
                _spriteBatch.Draw(Texture, _additionalDestinationRectangle.Value, _additionalSourceRectangle.Value, Color.White);
            }

            _spriteBatch.End();
        }
    }
}
