using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Animations
{
    /// <summary>
    /// Describes a game object with a set of animations and animation rules.
    /// </summary>
    public sealed class AnimatedObject
    {
        /// <summary>
        /// Registered animations for this object.
        /// To enable some rule, you have to register its animations in this container.
        /// </summary>
        public List<Animation> Animations
        {
            get; set;
        }

        private IList<Animation> CurrentAnimationState
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the position of this component in the world.
        /// </summary>
        public Vector2 WorldPosition { get; set; }

        /// <summary>
        /// It equals to the summary object size, with all animation sizes and offsets.
        /// </summary>
        public Vector2 AutoSize { get; set; }

        /// <summary>
        /// The desired render size of this object.
        /// If this value is null, <see cref="AutoSize"/> is used.
        /// </summary>
        public Vector2? Size { get; set; }

        public Rectangle BoundingRectangle { get; set; }

        public Rectangle CollisionRectangle { get; set; }

        private AnimatedObject()
        {
            Animations = new List<Animation>();
            CurrentAnimationState = new List<Animation>();
        }

        public void AddAnimationRule(String animationName, Func<bool> rule)
        {
            for (int i = 0; i < Animations.Count; i++)
            {
                if (String.Compare(Animations[i].Name, animationName, true) == 0)
                {
                    Animations[i].Rules.Add(rule);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateState();

            var currentAnimationState = CurrentAnimationState;

            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < currentAnimationState.Count; i++)
            {
                var animation = currentAnimationState[i];
                if (!animation.Size.HasValue)
                {
                    if (Size.HasValue)
                    {
                        var ratio = Size / AutoSize;
                        animation.Size = new Vector2(animation.FrameDimensions.X, animation.FrameDimensions.Y) * ratio;
                    }
                    else
                    {
                        animation.Size = null;
                    }
                }
                animation.Update(elapsedTime);
            }
        }

        private void UpdateState()
        {
            CurrentAnimationState.Clear();

            for (int animationIndex = 0; animationIndex < Animations.Count; animationIndex++ )
            {
                var animation = Animations[animationIndex];
                if (animation.Rules != null)
                {
                    bool allAreMatched = true;
                    for (int ruleIndex = 0; ruleIndex < animation.Rules.Count; ruleIndex++)
                    {
                        var rule = animation.Rules[ruleIndex];
                        if (!rule())
                        {
                            allAreMatched = false;
                            break;
                        }
                    }

                    if (allAreMatched)
                    {
                        CurrentAnimationState.Add(animation);
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var currentAnimationState = CurrentAnimationState;
            if (currentAnimationState == null)
            {
                return;
            }

            for (int index = 0; index < currentAnimationState.Count; index++)
            {
                currentAnimationState[index].Draw(spriteBatch, WorldPosition);
            }
        }

        public void SetFrame(int frameNumber)
        {
            for (int index = 0; index < Animations.Count; index++)
            {
                Animations[index].SetFrame(frameNumber);
            }
        }

        public Rectangle GetCompositeFrameSize()
        {
            if (!Animations.Any())
            {
                throw new InvalidOperationException("Unable to determine the frame size. There are no registered animations.");
            }

            var minH = Animations.Min(x => x.SourceOffset.X);
            var minV = Animations.Min(x => x.SourceOffset.Y);
            var maxH = Animations.Max(x => x.SourceOffset.X + x.FrameDimensions.X);
            var maxV = Animations.Max(x => x.SourceOffset.Y + x.FrameDimensions.Y);

            return new Rectangle((int)minH, (int)minV, (int)(maxH - minH), (int)(maxV - minV));
        }

        public static AnimatedObject FromMetadata(AnimatedObjectMetadata metadata, ContentManager contentManager)
        {
            var animatedObject = new AnimatedObject
            {
                Animations = metadata.Animations.Select(x => Animation.FromMetadata(x, contentManager)).ToList(),
                Size = (metadata.Size.X <= 0.001 || metadata.Size.Y <= 0.001) ? (Vector2?) null : metadata.Size
            };

            animatedObject.BoundingRectangle = animatedObject.GetCompositeFrameSize();
            animatedObject.AutoSize = new Vector2(animatedObject.BoundingRectangle.Width, animatedObject.BoundingRectangle.Height);
            animatedObject.CollisionRectangle = metadata.CollisionRectangle.X <= 0.001 ||
                metadata.CollisionRectangle.Y <= 0.001
                ? animatedObject.BoundingRectangle
                : metadata.CollisionRectangle;

            return animatedObject;
        }
    }
}
