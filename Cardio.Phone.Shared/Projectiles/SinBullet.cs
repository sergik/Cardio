using System;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Projectiles
{
    public class SinBullet : BaseBullet
    {
        public Vector2 StartPoint { get; set; }

        private double _multyplierY = 5;
        private double _multyplierX = 50;

        public float StartAngel { get; set; }


        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);
            
            WorldPosition = Vector2.Add(WorldPosition, new Vector2(Speed, (float)(Math.Cos((WorldPosition.X - StartPoint.X) / _multyplierX + StartAngel) * _multyplierY)));
            //var level = gameState.Level;
            //var size = Size ?? AutoSize;
            //var enemyies = level.IntersectedObstacle(new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, (int)size.X, (int)size.Y)).ToList();
            //if (enemyies.Count > 0)
            //{
            //    var enemy = enemyies[0];
            //    if (enemy != null)
            //    {
            //        gameState.Player.Nanobot.FrontWeapon.RemoveBullet(this);
            //        enemy.Damage(
            //            new DamageTakenEventArgs(HitDamage,
            //                                     enemy.WorldPosition));
            //    }
            //}
        }
    }
}
