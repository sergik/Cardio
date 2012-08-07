using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Projectiles
{
    public class NanobotBullet: BaseBullet
    {
        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);
            WorldPosition = Vector2.Add(WorldPosition, new Vector2(Speed, 0));
            //var size = Size ?? AutoSize;
            //var enemyies = level.IntersectedObstacle(new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, (int)size.X, (int)size.Y)).ToList();
            //if (enemyies.Count > 0)
            //{
            //    var enemy = enemyies[0];
            //    if (enemy != null)
            //    {
            //        enemy.Damage(
            //            new DamageTakenEventArgs(HitDamage,
            //                                     enemy.WorldPosition));
            //        gameState.Player.Nanobot.Gun.RemoveBullet(this);
            //        Unload(Game);
            //    }
            //}
        }
    }
}
