using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Projectiles;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Weapons
{
    public class MainWeaponLevel2 : Weapon
    {
        public override IEnumerable<BaseBullet> GenerateBullets(Game game)
        {
            var bullets = new List<BaseBullet>();
            var player = game.Services.GetService<GameState>().Player;
            var bullet1 = GetBullet(game,
                                    player.WorldPosition + player.Nanobot.GroupPosition +
                                    Vector2.Add(BulletGenerationPosition, new Vector2(0, 5)));
            bullets.Add(bullet1);

            var bullet2 = GetBullet(game,
                                    player.WorldPosition + player.Nanobot.GroupPosition +
                                    Vector2.Add(BulletGenerationPosition, new Vector2(0, -5)));
            bullets.Add(bullet2);

            var bullet3 = GetBullet(game,
                                    player.WorldPosition + player.Nanobot.GroupPosition +
                                    Vector2.Add(BulletGenerationPosition, new Vector2(0, -15)));
            bullets.Add(bullet3);

            return bullets;
        }
    }
}
