using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Projectiles
{
    public class Bullet: BaseBullet
    {
        public float DirectionAngle { get; set; }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            const float speedMultiplier = 0.001f;

            var offset =
                new Vector2(
                    (float)(Speed * Math.Cos(DirectionAngle) * elapsedTime * speedMultiplier),
                    (float)(Speed * Math.Sin(DirectionAngle) * elapsedTime * speedMultiplier));
            WorldPosition += offset;
            var size = Size ?? AutoSize;

            var bot = gameState.Player.Nanobot;
            var hitRectangle = bot.Intersects(new Rectangle((int) WorldPosition.X, (int) WorldPosition.Y,
                (int) size.X, (int) size.Y));
            if (!hitRectangle.IsEmpty)
            {
                bot.Damage(new DamageTakenEventArgs(HitDamage,
                    new Vector2(
                        hitRectangle.X + hitRectangle.Width / 2,
                        hitRectangle.Y + hitRectangle.Height / 2)));
                gameState.Level.RemoveLevelObject(this);
                SoundManager.BotHit.Play();
            }
            var screenPosition = gameState.Camera.GetScreenPosition(WorldPosition, gameState.Camera.MinScale);
            if (screenPosition.X < 0 || screenPosition.Y < 0)
            {
                gameState.Level.RemoveLevelObject(this);
            }

            base.Update(gameState, gameTime);
        }
    }
}
