using System;
using Cardio.UI.Animations;
using Cardio.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Core
{
    public class ThinkCloud : ContentObject
    {
        private SpriteBatch _spriteBatch;
        private ICamera2D _camera;

        public Animation Animation { get; set; }

        public bool IsVisible { get; set; }

        public DrawableGameObject Target { get; set; }

        public Vector2 TargetOffset { get; set; }

        public ThinkCloud(string assetName)
        {
            AssetName = assetName;
        }

        public void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            if (Target == null)
            {
                throw new InvalidOperationException("You have to specify think cloud target before calling Initialize.");
            }

            _spriteBatch = spriteBatch;
            _camera = camera;

            Animation = Animation.FromMetadata(game.Content.Load<AnimationMetadata>(AssetName), game.Content);
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                Animation.Update((float )gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (IsVisible)
            {
                var position = Target.WorldPosition + TargetOffset;
                if (Animation.Size != null)
                {
                    position.Y -= Animation.Size.Value.Y;
                }
                else
                {
                    position.Y -= Animation.FrameDimensions.Y;
                }

                _spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null, null, null, null,
                        _camera.Transform);
                Animation.Draw(_spriteBatch, position);
                _spriteBatch.End(); 
            }
        }
    }
}
