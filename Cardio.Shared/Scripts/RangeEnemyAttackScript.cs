using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Characters;
using Cardio.UI.Characters.Ranged;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Projectiles;
using Cardio.UI.Sounds;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class RangeEnemyAttackScript : GameEntityScript
    {
        private static Random _random = new Random();
        /// <summary>
        /// The one who shoots.
        /// </summary>
        public IRangeEnemy Combatant { get; set; }

        private readonly List<Bullet> _activeBullets = new List<Bullet>();

        public float ShootStartDelay { get; set; }

        private float _timeAfterLastAttack;

        public RangeEnemyAttackScript(float shootDelay)
        {
            ShootStartDelay = shootDelay;
        }

        protected override void OnStart(GameState state)
        {
            _timeAfterLastAttack = Combatant.AttackInterval - ShootStartDelay;
            base.OnStart(state);
        }

        public override void Update( GameState state, GameTime time)
        {
            if (Combatant.WorldPosition.X - state.Player.WorldPosition.X <= Combatant.AttackRange)
            {
                if (_timeAfterLastAttack >= Combatant.AttackInterval && 
                    Combatant.Health > 0)
                {
                    ShootNewBullet(state);

                    _timeAfterLastAttack -= Combatant.AttackInterval;
                }

                _timeAfterLastAttack += (float)time.ElapsedGameTime.TotalMilliseconds;
            }
            
            if (Combatant.Health == 0 && _activeBullets.Count == 0)
            {
                Stop(state);
            }

            base.Update(state, time);
        }

        private void ShootNewBullet(GameState state)
        {
            var bullet = Combatant.GenerateBullet();


            var width = bullet.Content.Size == null ? bullet.Content.AutoSize.X : bullet.Content.Size.Value.X;
            var height = bullet.Content.Size == null ? bullet.Content.AutoSize.Y : bullet.Content.Size.Value.Y;

            bullet.WorldPosition = new Vector2(Combatant.WorldPosition.X - width, Combatant.WorldPosition.Y + height / 2f);

            int targetIndex = _random.Next(4);
            var target = state.Player.Nanobots[targetIndex].WorldPosition;

            var bulletDirection = target - bullet.WorldPosition;

            float angle = (float)Math.Acos(Vector2.Dot(-1 * Vector2.UnitX, Vector2.Normalize(bulletDirection)));
            bullet.DirectionAngle = MathHelper.Pi - Math.Sign(bulletDirection.Y) * angle;

            _activeBullets.Add(bullet);
            state.Level.AddLevelObject(bullet);

            SoundManager.EnemyAttack01.Play();
        }
    }
}