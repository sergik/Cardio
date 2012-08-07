using System;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Components
{
    public class Camera2D: ICamera2D
    {
        private Vector2 _position;

        protected float _viewportWidth;
        public float ViewportWidth
        {
            get { return _viewportWidth; }
        }

        protected float _viewportHeight;
        public float ViewportHeight
        {
            get { return _viewportHeight; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Viewport
        {
            get {return new Vector2(_viewportWidth, _viewportHeight);}
        }

        public float Rotation { get; set; }

        public Vector2 Origin { get; set; }

        public float MaxScale { get; set; }

        public float MinScale { get; set; }

        private float _scale;
        public float Scale
        {
            get { return _scale; }
            set
            {
                value = Math.Max(MinScale, value);
                value = Math.Min(MaxScale, value);

                _scale = value;
            }
        }

        public Vector2 ScreenCenter { get; protected set; }

        public Matrix Transform { get; set; }

        public IPositioned FocusedAt { get; set; }

        public Vector2 FocusedAtOffset { get; set;}

        public float MoveSpeed { get; set; }

        private Game _game;

        public Camera2D()
        {
            MaxScale = 2f;
            MinScale = 0f;
            Scale = 1f;
            MoveSpeed = 2.6f;
        }

        public void Initialize(Game game)
        {
            _viewportWidth = game.GraphicsDevice.Viewport.Width;
            _viewportHeight = game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(_viewportWidth / 2, _viewportHeight / 2);

            _game = game;
        }

        public void Update(GameTime gameTime)
        {
            // Create the Transform used by any
            // spritebatch process
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Scale, Scale, Scale));

            Origin = ScreenCenter / Scale;

            _viewportWidth = _game.GraphicsDevice.Viewport.Width / Scale;
            _viewportHeight = _game.GraphicsDevice.Viewport.Height / Scale;

            // Move the Camera to the position that it needs to go
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (FocusedAt != null)
            {
                _position.X += (FocusedAt.WorldPosition.X + FocusedAtOffset.X - Position.X) * MoveSpeed * delta;
                _position.Y += (FocusedAt.WorldPosition.Y + FocusedAtOffset.Y - Position.Y) * MoveSpeed * delta; 
            }
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen

            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }

        public Point GetScreenPosition(Vector2 worldPosition)
        {
            return GetScreenPosition(worldPosition, Scale);
        }

        public Point GetScreenPosition(Vector2 worldPosition, float atScale)
        {
            var offset = (worldPosition - Position) * atScale;
            return new Point((int)(_game.GraphicsDevice.Viewport.Width / 2f + offset.X),
                (int)(_game.GraphicsDevice.Viewport.Height / 2f + offset.Y));
        }

        public Rectangle GetScreenRectangle(Rectangle worldRectangle)
        {
            var offsetX = (int) ((worldRectangle.X - Position.X) * Scale + _game.GraphicsDevice.Viewport.Width / 2f);
            var offsetY = (int) ((worldRectangle.Y - Position.Y) * Scale + _game.GraphicsDevice.Viewport.Height / 2f);
            var width = (int) (worldRectangle.Width * Scale);
            var height = (int) (worldRectangle.Height * Scale);

            return new Rectangle(offsetX, offsetY, width, height);
        }
    }
}
