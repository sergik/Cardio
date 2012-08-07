using System;
using Cardio.UI.Animations;
using Cardio.UI.Components;
using Cardio.UI.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Core
{
    public class DrawableGameObject: GameEntity, ICollidable
    {
        public SpriteBatch SpriteBatch { get; private set; }

        private AnimatedObject _content;
        public AnimatedObject Content
        {
            get { return _content; }
            set 
            {
                _content = value;
                if (Content != null)
                {
                    Content.WorldPosition = _worldPosition;
                }
            }
        }

        public Sound Sound { get; set; }

        private Vector2 _worldPosition;

        public Vector2 WorldPosition
        {
            get 
            { 
                if (Content != null)
                {
                    return Content.WorldPosition;
                }
                return _worldPosition;
            }
            set
            {
                _worldPosition = value;
                if (Content != null)
                {
                    Content.WorldPosition = value;
                }
            }
        }

        public Vector2 AutoSize { get; set; }

        public String AssetName { get; set; }

        public Vector2? Size { get; set;}

        public Game Game { get; private set;}

        private bool _isCollisionRectangleSet;
        private Rectangle _collisionRectangle;
        public Rectangle CollisionRectangle
        {
            get { return _collisionRectangle; }
        }

        public Rectangle WorldCollisionRectangle
        {
            get
            {
                var rect = _collisionRectangle;
                rect.Offset((int) WorldPosition.X, (int) WorldPosition.Y);
                return rect;
            }
        }

        protected bool IsInitialized { get; private set; }

        protected ICamera2D Camera { get; private set; }

        public DrawableGameObject(string assetName)
        {
            AssetName = assetName;
        }

        public DrawableGameObject()
        {
        }

        public virtual void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            Game = game;
            SpriteBatch = spriteBatch;

            LoadContent();
            Camera = camera;

            AutoSize = Content.AutoSize;

            if (!_isCollisionRectangleSet)
            {
                _collisionRectangle = Content.CollisionRectangle;
                _isCollisionRectangleSet = true;
            }


            IsInitialized = true;
        }

        protected virtual void LoadContent()
        {
            if (Content != null)
            {
                return;
            }
            if (!String.IsNullOrEmpty(AssetName))
            {
                Content = AnimatedObject.FromMetadata(Game.Content.Load<AnimatedObjectMetadata>(AssetName), Game.Content);
                Content.WorldPosition = _worldPosition;
            }
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);

            if (Size.HasValue)
            {
                Content.Size = Size;
            }

            Content.WorldPosition = WorldPosition;
            Content.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Camera == null)
            {
                SpriteBatch.Begin();
            }
            else
            {
                SpriteBatch.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null, null, null, null,
                Camera.Transform);
            }
            
            Content.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        public Rectangle Intersects(Rectangle rect)
        {
            return Rectangle.Intersect(WorldCollisionRectangle, rect);
        }

        public bool Contains(Point point)
        {
            return WorldCollisionRectangle.Contains(point);
        }
    }
}
