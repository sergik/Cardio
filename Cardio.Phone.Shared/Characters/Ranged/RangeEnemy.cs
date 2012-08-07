using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Projectiles;
using Cardio.Phone.Shared.Animations;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Characters.Ranged
{
    public class RangeEnemy: ObstacleGameObject, IRangeEnemy
    {
        public float AttackInterval { get; set; }
        
        public float AttackDamage { get; set; }
        
        public float BulletSpeed { get; set; }

        public float AttackRange { get; set; }

        public string BulletAssetName { get; set; }

        public bool IsShooting { get; private set; }

        protected RangeEnemy()
        {
            AttackInterval = 3000;
            BulletSpeed = 500;
            AttackDamage = 20;
            AttackRange = 800;
        }

        public virtual Bullet GenerateBullet()
        {
            var bullet = new Bullet
            {
                Content = AnimatedObject.FromMetadata(Game.Content.Load<AnimatedObjectMetadata>(BulletAssetName), Game.Content),
                Speed = BulletSpeed,
                HitDamage = AttackDamage
            };
            bullet.Initialize(Game, SpriteBatch, Game.Services.GetService<GameState>().Camera);
            return bullet;
        }

        public static RangeEnemy FromMetadata(RangeEnemyMetadata metadata, ContentManager contentManager)
        {
            var enemy = new RangeEnemy();
            FillWithMetadata(enemy, metadata, contentManager);
            return enemy;
        }

        public static void FillWithMetadata(RangeEnemy enemy, RangeEnemyMetadata metadata, ContentManager contentManager)
        {
            ObstacleGameObject.FillWithMetadata(enemy, metadata, contentManager);

            enemy.AttackDamage = metadata.AttackDamage;
            enemy.AttackInterval = metadata.AttackInterval;
            enemy.AttackRange = metadata.AttackRange;
            enemy.BulletSpeed = metadata.BulletSpeed;
            enemy.BulletAssetName = metadata.BulletAssetName;

            if(!String.IsNullOrEmpty(metadata.CircleScriptAssetName))
            {
                CircleMovementScript circleScript = CircleMovementScript.FromMetadata(contentManager.Load<CircleScriptMetadata>(metadata.CircleScriptAssetName));
                circleScript.Target = enemy;
                enemy.AddScript(circleScript);
            }

            enemy.AddScript(new RangeEnemyAttackScript(300) { Combatant = enemy });
        }
    }
}
