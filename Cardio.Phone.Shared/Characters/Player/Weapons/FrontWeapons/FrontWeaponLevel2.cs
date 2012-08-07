using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Projectiles;

namespace Cardio.Phone.Shared.Characters.Player.Weapons.FrontWeapons
{
    public class FrontWeaponLevel2: FrontWeaponLevel1
    {
        public override IEnumerable<BaseBullet> GenerateBullets(Microsoft.Xna.Framework.Game game)
        {
            var bullets = base.GenerateBullets(game);
            var player = game.Services.GetService<GameState>().Player;
            var bulletPosition = player.WorldPosition + player.Nanobot.GroupPosition + BulletGenerationPosition;
            var bullet = GetBullet(game, bulletPosition);
            (bullet as CosBullet).StartAngel = (float)Math.PI;
            var result = bullets.ToList();
            result.Add(bullet);
           
            return result;
        }
    }
}
