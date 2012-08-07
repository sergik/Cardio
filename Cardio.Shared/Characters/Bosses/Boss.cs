using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Animations;
using Cardio.UI.Characters.Ranged;
using Cardio.UI.Core;
using Cardio.UI.Levels;
using Cardio.UI.Projectiles;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Cardio.UI.Particles;

namespace Cardio.UI.Characters.Bosses
{
    public class Boss : ObstacleGameObject, IRangeEnemy
    {
        private Level _level;

        private bool _generating;

        public int AttackMode
        {
            get; private set;
        }

        public Level Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                GenerateScript.Level = _level;
            }
        }

        public GenerateEnemiesScript GenerateScript
        {
            get; set;
        }

        public float AttackRange
        {
            get; set;
        }

        public float AttackDamage
        {
            get; set;
        }

        public float AttackInterval
        {
            get; set;
        }

        public float BulletSpeed
        {
            get; set;
        }

        public float AttackRateMin
        {
            get; set;
        }

        public float AttackRateMax
        {
            get; set;
        }

        public string BulletContentPath
        {
            get; set;
        }

        public bool IsPreparingForAttack
        {
            get; private set;
        }

        public virtual Bullet GenerateBullet()
        {
            var bullet = new Bullet
            {
                Content = AnimatedObject.FromMetadata(Game.Content.Load<AnimatedObjectMetadata>(BulletContentPath), Game.Content),
                Speed = BulletSpeed,
                HitDamage = AttackDamage
            };
            bullet.Initialize(Game, SpriteBatch, Camera);
            return bullet;
        }

        public void StartAttack()
        {
            IsPreparingForAttack = true;
            AttackMode = RandomHelper.RandomFrom(0, 2);
        }

        public void StopAttack()
        {
            IsPreparingForAttack = false;
            AttackMode = -1;
        }

        public Vector2 GunCoordinates()
        {
            Vector2 offset;
            switch (AttackMode)
            {
                case 0: 
                    offset = new Vector2(-50, 50);
                    break;
                case 1: 
                    offset = new Vector2(25, 200);
                    break;
                case 2:
                    offset = new Vector2(-50, 325);
                    break;
                default:
                    offset = Vector2.Zero;
                    break;
            }
            return WorldPosition + offset;
        }

        public override void Initialize(Game game, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Components.ICamera2D camera)
        {
            base.Initialize(game, spriteBatch, camera);
            GenerateScript.Initialize(game, spriteBatch, camera);
            GenerateScript.WorldPosition = WorldPosition;

            BossAttackScript attackScript = new BossAttackScript(game, this);
            AddScript(attackScript);
            attackScript.Initialize();

            GenerateScript.PreparingForGeneration += new EventHandler((s, e) => 
            {
                _generating = true;
                Content.Animations.FirstOrDefault((a) => a.Name == "EnemyBorn").SetFrame(0);
            });

            GenerateScript.Generated += (s, e) => _generating = false;

            Content.AddAnimationRule("Hands", () => IsAlive);
            Content.AddAnimationRule("BodyPoints", () => IsAlive);
            Content.AddAnimationRule("Mouth", () => (IsAlive && _generating));

            Content.AddAnimationRule("EnemyBorn", () => (IsAlive && _generating));
            Content.AddAnimationRule("TopAttackPreparation", () => AttackMode == 0);
            Content.AddAnimationRule("MidAttackPreparation", () => AttackMode == 1);
            Content.AddAnimationRule("BottomAttackPreparation", () => AttackMode == 2);

            Content.AddAnimationRule("Default", () => (IsAlive && !_generating));

            AddScript(new GroupSwitchNearBossScript(this));
        }

        public override IList<ParticleEffect> ToDeathAnimation()
        {
            var result = new List<ParticleEffect>();
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 1; i <= 10; i++)
            {
                var DeathEffect = BlowEffect.FromMetadata(DeathEffectMetadata, Game.Content);
                DeathEffect.WorldPosition = WorldPosition + new Vector2((random.Next(2) == 1 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.X, (random.Next(2) == 0 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.Y);
                result.Add(DeathEffect);
            }
            //DeathEffect.Initialize(Game, SpriteBatch, Camera);

            return result;
        }

        public override IList<ParticleEffect> ToDamageAnimation()
        {
            var result = new List<ParticleEffect>();
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 1; i <= 2; i++)
            {
                var DamageEffect = BlowEffect.FromMetadata(DamageEffectMetadata, Game.Content);
                DamageEffect.WorldPosition = WorldPosition + new Vector2((random.Next(2) == 1 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.X, (random.Next(2) == 0 ? 1 : -1) * random.Next(200) + Content.BoundingRectangle.Center.Y);
                result.Add(DamageEffect);
            }
            //DeathEffect.Initialize(Game, SpriteBatch, Camera);

            return result;
        }
        
        public static Boss FromMetadata(BossMetadata metadata, ContentManager contentManager)
        {
            var boss = new Boss();
            FillWithMetadata(boss, metadata, contentManager);
            boss.AttackDamage = metadata.AttackDamage;

            boss.GenerateScript = new GenerateEnemiesScript();
            
            foreach(var enemyType in metadata.EnemyTypes)
            {
                boss.GenerateScript.EnemyTypes.Add(enemyType);
            }

            boss.GenerateScript.EnemiesToGenerateMin = metadata.EnemiesToGenerateMin;
            boss.GenerateScript.EnemiesToGenerateMax = metadata.EnemiesToGenerateMax;
            boss.GenerateScript.GenerateEnemiesIntervalMin = TimeSpan.FromSeconds(metadata.GenerateEnemiesIntervalMin);
            boss.GenerateScript.GenerateEnemiesIntervalMax = TimeSpan.FromSeconds(metadata.GenerateEnemiesIntervalMax);
            boss.GenerateScript.GenerationStartDistance = 600;

            boss.AttackRange = metadata.AttackRange;
            boss.AttackDamage = metadata.AttackDamage;
            boss.AttackInterval = metadata.AttackInterval;
            boss.AttackRateMax = metadata.AttackRateMax;
            boss.AttackRateMin = metadata.AttackRateMin;
            boss.BulletSpeed = metadata.BulletSpeed;
            boss.BulletContentPath = metadata.BulletContentPath;
            boss.AttackMode = -1;
            boss._generating = false;

            boss.AddScript(boss.GenerateScript);
            return boss;
        }
    }
}
