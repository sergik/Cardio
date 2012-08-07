using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Projectiles;
using Cardio.Phone.Shared.Weapons;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Characters.Player.Weapons.FrontWeapons
{
    public class FrontWeaponLevel1: Weapon
    {
        public override IEnumerable<BaseBullet> GenerateBullets(Game game)
        {
            var bullets = new List<BaseBullet>();
            var player = game.Services.GetService<GameState>().Player;
            var bulletPosition = player.WorldPosition + player.Nanobot.GroupPosition + BulletGenerationPosition;
            var bullet = GetBullet(game, bulletPosition);
            bullets.Add(bullet);
            return bullets;
        }


        public override BaseBullet GetBullet(Game game, Vector2 position)
        {
            var bullet = GetBulletFromCache(game);
            if (bullet == null)
            {
                bullet = new CosBullet
                             {
                                 Content =
                                     AnimatedObject.FromMetadata(
                                         game.Content.Load<AnimatedObjectMetadata>(BulletAssetName), game.Content),
                                 Speed = BulletSpeed,
                                 HitDamage = Damage,
                                 WorldPosition = position,
                                 StartPoint = position,
                                 AssetName = BulletAssetName,
                             };
            }
            bullet.WorldPosition = position;
            bullet.Initialize(game, SpriteBatch, game.Services.GetService<GameState>().Camera);

            return bullet;
        }

        public override BaseBullet GetBulletFromCache(Game game)
        {
            var cache = game.Services.GetService<GameCache>();
            var bullet = cache.Cache.OfType<CosBullet>().FirstOrDefault(b => b.AssetName == BulletAssetName);
            if (bullet != null)
            {
                cache.Remove(bullet);
            }
            return bullet;
        }
    }
}
