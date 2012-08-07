using System;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Sounds;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Projectiles
{
    public class Bullet: DrawableGameObject
    {
        public float Speed { get; set; }

        public float DirectionAngle { get; set; }

        public float HitDamage { get; set; }

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

            for (int index = 0; index < gameState.Player.Nanobots.Count; index++)
            {
                var bot = gameState.Player.Nanobots[index];
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
                    break;
                }
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
