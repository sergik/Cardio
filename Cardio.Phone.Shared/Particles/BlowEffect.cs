using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Particles;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Particles
{
    public enum PartsMovingLaw
    {
        Simple,
        Circle
    }

    public class BlowEffect : ParticleEffect
    {
        private static readonly Random Random = new Random();

        private static readonly Dictionary<PartsMovingLaw, Func<Vector2, Vector2>> Laws = new Dictionary<PartsMovingLaw, Func<Vector2, Vector2>>();

        static BlowEffect()
        {
            Laws.Add(PartsMovingLaw.Circle, (sourceOffset) =>
            {

                var normal = new Vector2();
                var k = -sourceOffset.X / sourceOffset.Y;
                var b = sourceOffset.Y + sourceOffset.X * sourceOffset.X / sourceOffset.Y;
                var x2 = (sourceOffset.Y > 0) ? sourceOffset.X + 10 : sourceOffset.X - 10;
                var y2 = k * x2 + b;

                normal.X = x2 - sourceOffset.X;
                normal.Y = y2 - sourceOffset.Y;

                return sourceOffset + normal * sourceOffset.Length() / 3;

            });

            Laws.Add(PartsMovingLaw.Simple, (sourceOffset) =>
            {
                return sourceOffset;
            });
        }

        private float _holdTime;
        private float _currentTime;

        private BlowType _type;

        private bool _isFinished;

        protected List<Animation> _movingParts;
        protected float _elapsedTime = 0;

        public int PartVelocity { get; private set; }
        public PartsMovingLaw Law { get; private set; }
        public BlowEffectMetadata MetaData { get; private set; }

        public List<Animation> MovingParts 
        {
            get
            {
                return _movingParts;
            }
            private set
            {
                _movingParts = value;
            }
        }

        public static BlowEffect FromMetadata(BlowEffectMetadata metadata, ContentManager contentManager)
        {
            return FillWithMetadata(new BlowEffect(), metadata, contentManager);
        }

        public static BlowEffect FillWithMetadata(BlowEffect blow, BlowEffectMetadata metadata, ContentManager contentManager)
        {
            blow.MovingParts = new List<Animation>();
            blow.PartVelocity = metadata.PartVelocity;
            blow.Law = metadata.Law;
            blow._type = metadata.Type;
            blow.MetaData = metadata;
  
            var contentMetaData = new AnimatedObjectMetadata() 
            {
                Size = new Vector2(0, 0),
                CollisionRectangle = new Rectangle(0, 0, 0, 0),
                Animations = new List<AnimationMetadata>()
            };
            var animations = new List<Animation>();
            var partsCount = Random.Next(metadata.MaxMovingParts - metadata.MinMovingParts) + metadata.MinMovingParts;
            AnimatedObject content = null;
            AnimationMetadata centerAnimationMetadata;

            if (metadata.CenterTextureName != string.Empty)
            {
                centerAnimationMetadata = new AnimationMetadata()
                {
                    TextureName = metadata.CenterTextureName,
                    Name = "Center",
                    FramesPerRow = metadata.CenterFrameCount,
                    Interval = metadata.CenterInterval,
                    IsLooped = metadata.IsCenterLooped,
                    SourceOffset = new Vector2(0, 0),
                    LastFrameHoldOnTime = 0,
                    Size = new Vector2(0, 0)
                };

                contentMetaData.Animations.Add(centerAnimationMetadata);
                content = AnimatedObject.FromMetadata(contentMetaData, contentManager);

                blow.Content = content;
                blow.Content.AddAnimationRule("Center", () => !blow._isFinished);
            }

            for (var i = 1; i <= partsCount; i++)
            {

                float radius = Random.Next(metadata.MovingPartsDispertion);
                float randomX = Random.Next(metadata.MovingPartsDispertion - 1) * (Random.Next(3) > 1 ? -1 : 1);
                float randomY = (float)Math.Sqrt(radius * radius - randomX * randomX) * (Random.Next(3) > 1 ? -1 : 1);

                var partAnimationMetaData = new AnimationMetadata()
                {
                    TextureName = metadata.MovingPartsTextureName,
                    Name = "Part_" + i,
                    FramesPerRow = metadata.MovingPartsFramesCount,
                    Interval = Random.Next(metadata.MovingPartsIntervalTop - metadata.MovingPartsIntervalBottom) + metadata.MovingPartsIntervalBottom,
                    IsLooped = metadata.AreMovingPartsLooped,
                    SourceOffset = new Vector2(randomX, randomY),
                    LastFrameHoldOnTime = 0,
                    Size = new Vector2(0, 0)
                };

                if (blow.Content == null)
                {
                    contentMetaData.Animations.Add(partAnimationMetaData);
                    content = AnimatedObject.FromMetadata(contentMetaData, contentManager);
                    blow.Content = content;

                    continue;
                }

                var partAnimation = Animation.FromMetadata(partAnimationMetaData, contentManager);

                blow.Content.Animations.Add(partAnimation);
                blow.MovingParts.Add(partAnimation);
                blow.Content.AddAnimationRule(partAnimation.Name, () => !blow._isFinished);
            }

            return blow;
        }

        private BlowEffect()
        {
            PartVelocity = 10;
        }

        public override void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            base.Initialize(game, spriteBatch, camera);

            

            _isFinished = false;

            Content.Animations.ForEach(a =>
            {

                float radius = Random.Next(MetaData.MovingPartsDispertion);
                float randomX = Random.Next(MetaData.MovingPartsDispertion - 1) * (Random.Next(3) > 1 ? -1 : 1);
                float randomY = (float)Math.Sqrt(radius * radius - randomX * randomX) * (Random.Next(3) > 1 ? -1 : 1);

                a.SourceOffset = new Vector2(randomX, randomY);
                a.SetFrame(0);
            });

            _currentTime = 0;
  
            _holdTime = Content.Animations.Max(x => x.Interval * x.FramesPerRow + x.LastFrameHoldOnTime);
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (_isFinished)
            {
                return;
            }

            if (_currentTime >= _holdTime)
            {
                InvokeFinished(EventArgs.Empty);
                _isFinished = true;
            }

            for (int index = 0; index < MovingParts.Count; index++)
            {
                var part = MovingParts[index];
                Vector2 shift = Laws[Law](part.SourceOffset);

                shift.Normalize();
                shift *= PartVelocity;
                part.SourceOffset += shift;
            }


            _currentTime += gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameState, gameTime);
        }


        public override void Unload(Game game)
        {
            if (_type != BlowType.None)
            {
                var cache = game.Services.GetService<GameCache>();
                cache.AddBlow(_type, this);
            }
        }
    }
}
