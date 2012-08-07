using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Animations
{
    /// <summary>
    /// Describes a single animation, created from the texture with multiple frames.
    /// Provides basic framing and drawing logic.
    /// </summary>
    public sealed class Animation: ContentObject
    {
        /// <summary>
        /// Describes the region from the source animation texture that should be drawn at this frame.
        /// </summary>
        private Rectangle _sourceRectangle;

        private float _elapsedTime;

        public IList<Func<bool>> Rules { get; set; }

        public string Name { get; set; }

        public Texture2D Texture { get; set; }

        /// <summary>
        /// Dimensions of the single frame of the animation.
        /// </summary>
        public Point FrameDimensions { get; set; }

        /// <summary>
        /// Amount of frames in this texture file.
        /// </summary>
        public int FramesPerRow { get; set; }

        /// <summary>
        /// Time interfal (in milliseconds) between two sequential animation frames.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Offset from the draw position. 
        /// Can be used with composite animation objects that contain multiple independent animations, drawn at different positions.
        /// </summary>
        public Vector2 SourceOffset { get; set; }

        /// <summary>
        /// Desired draw size. If the value is not specified, the animation frame is drawn unscaled, 
        /// otherwise, the appropriate scaling is applied (based on <see cref="FrameDimensions"/> value).
        /// </summary>
        public Vector2? Size { get; set;}

        /// <summary>
        /// Gets or sets current animation frame.
        /// </summary>
        public int CurrentFrame { get; private set; }

        /// <summary>
        /// True if the animation should be replayed when it is ended.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// If is not null, allows to hold on the last animation frame for the specified amount of time.
        /// </summary>
        public float LastFrameHoldOnTime { get; set; }

        private float _currentHoldOnTime;

        public bool HasFinishedPlay { get; private set; }

        public Animation()
        {
            IsLooped = true;
            Rules = new List<Func<bool>>();
        }

        public void SetFrame(int newFrame)
        {
            CurrentFrame = newFrame;
            _elapsedTime = 0;
            _currentHoldOnTime = 0;
            HasFinishedPlay = false;
        }

        public void Update(float elapsedMilliseconds)
        {
            if (HasFinishedPlay)
            {
                return;
            }

            if (LastFrameHoldOnTime > 0 && CurrentFrame >= FramesPerRow - 1)
            {
                if (_currentHoldOnTime >= LastFrameHoldOnTime)
                {
                    CurrentFrame = 0;
                    _elapsedTime = 0;
                    _currentHoldOnTime = 0;

                    if (!IsLooped)
                    {
                        HasFinishedPlay = true;
                    }

                    return;
                }

                CurrentFrame = FramesPerRow - 1;
                _currentHoldOnTime += elapsedMilliseconds;
                return;
            }

            if (!IsLooped && CurrentFrame >= FramesPerRow - 1)
            {
                CurrentFrame = 0;
                HasFinishedPlay = true;
                return;
            }

            _elapsedTime += elapsedMilliseconds;

            if (_elapsedTime > Interval)
            {
                CurrentFrame++;
                _elapsedTime -= Interval;
            }

            if (IsLooped && LastFrameHoldOnTime <= 0)
            {
                CurrentFrame %= FramesPerRow;
            }

            _sourceRectangle = new Rectangle(CurrentFrame * FrameDimensions.X, 0, FrameDimensions.X, FrameDimensions.Y);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (HasFinishedPlay)
            {
                return;
            }

            var scaleVector = (Size.HasValue)
                ? new Vector2(Size.Value.X / FrameDimensions.X, Size.Value.Y / FrameDimensions.Y)
                : Vector2.One;
            spriteBatch.Draw(Texture, position + SourceOffset * scaleVector, _sourceRectangle, Color.White, 0f, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
        }

        public static Animation FromMetadata(AnimationMetadata metadata, ContentManager contentManager)
        {
            var animation = new Animation
            {
                Name = metadata.Name,
                Texture = contentManager.Load<Texture2D>(metadata.TextureName),
                FramesPerRow = metadata.FramesPerRow,
                Interval = metadata.Interval,
                SourceOffset = metadata.SourceOffset,
                Size = (metadata.Size.X <= 0.001 || metadata.Size.Y <= 0.001) ? (Vector2?) null : metadata.Size,
                LastFrameHoldOnTime = metadata.LastFrameHoldOnTime,
                IsLooped = metadata.IsLooped
            };

            animation.FrameDimensions =  new Point(animation.Texture.Width / animation.FramesPerRow, animation.Texture.Height);

            return animation;
        }
    }
}