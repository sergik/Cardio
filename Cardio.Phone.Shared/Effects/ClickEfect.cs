using System;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Characters;
using Cardio.Phone.Shared.Characters.Ranged;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Effects
{
    public class ClickEfect : DrawableGameObject
    {
        private float _changingTime;
        private Vector2 _curentSize;

        public Vector2 StartSize { get; set; }

        public Vector2 EndSize { get; set; }

        public float TimeAlive { get; set; }

        public Action Ended;

        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);
            var deltaSize = StartSize - EndSize;
            _curentSize = StartSize -  deltaSize*_changingTime/TimeAlive;
            _changingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            WorldPosition = new Vector2(WorldPosition.X + MovePlayerScript.PlayerSpeed, WorldPosition.Y);
            if(_changingTime > TimeAlive)
            {
                OnEnded();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Content.Animations[0].Size = _curentSize;
            base.Draw(gameTime);
        }

        public void OnEnded()
        {
            if (Ended != null)
            {
                Ended();
            }
        }

        public static ClickEfect FromMetadata(ClickEffectMetadata metadata, ContentManager contentManager)
        {
            var clickEfect = new ClickEfect();
            FillWithMetadata(clickEfect, metadata, contentManager);
            return clickEfect;
        }

        public static void FillWithMetadata(ClickEfect clickEfect, ClickEffectMetadata metadata, ContentManager contentManager)
        {
            clickEfect.Content = AnimatedObject.FromMetadata(contentManager.Load<AnimatedObjectMetadata>(metadata.ContentPath), contentManager);
            clickEfect.Content.AddAnimationRule("Default", () => true);

            clickEfect.EndSize = metadata.EndSize;
            clickEfect.StartSize = metadata.StartSize;
            clickEfect.TimeAlive = metadata.TimeAlive;
        }
    }
}
