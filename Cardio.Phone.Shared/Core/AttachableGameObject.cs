using System;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Core
{
    public class AttachableGameObject : ContentObject
    {
        public ICamera2D Camera;
        public DrawableGameObject Target { get; set; }

        public Vector2 TargetOffset { get; set; }
        public SpriteBatch SpriteBatch
        {
            get; private set;
        }

        private AnimatedObject _content;
        public AnimatedObject Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        public virtual void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            if (Target == null)
            {
                throw new InvalidOperationException("You have to specify target before calling Initialize.");
            }

            SpriteBatch = spriteBatch;
            Camera = camera;

            if (!String.IsNullOrEmpty(AssetName))
            {
                Content = AnimatedObject.FromMetadata(game.Content.Load<AnimatedObjectMetadata>(AssetName), game.Content);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Content != null)
            {
                Content.Update(gameTime);
                _content.WorldPosition = Target.WorldPosition + TargetOffset;
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Content != null)
            {
                var position = Target.WorldPosition + TargetOffset;
                if (Content.Size != null)
                {
                    position.Y -= Content.Size.Value.Y;
                }
                else
                {
                    position.Y -= Content.AutoSize.Y;
                }
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
        }
    }
}
