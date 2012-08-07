
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Animations;
using Cardio.UI.Characters.Bosses;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Projectiles;
using Cardio.UI.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.UI.Extensions;

namespace Cardio.UI.Scripts
{
    public class BossAttackScript : GameEntityScript
    {
        private Game _game;
        private SpriteBatch _spritebatch;
        private ICamera2D _camera;

        private float _timeAfterLastAttack;
        private float _intervalForNextAttack;

        private Boss _boss;

        private readonly List<Bullet> _activeBullets = new List<Bullet>();

        public float RemainingPreparationTime
        {
            get
            {
                return _boss.AttackInterval - (_intervalForNextAttack - _timeAfterLastAttack);
            }
        }

        public BossAttackScript(Game game, Boss boss)
        {
            _game = game;
            _boss = boss;
        }

        public void Initialize()
        {
            _camera = _game.Services.GetService<GameState>().Camera;
            _spritebatch = _game.Services.GetService<SpriteBatch>();
            _intervalForNextAttack = RandomHelper.GetRandomFloatFromInterval(_boss.AttackRateMin, _boss.AttackRateMax) + _boss.AttackInterval;
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);

            if (_boss.WorldPosition.X - gameState.Player.WorldPosition.X <= _boss.AttackRange)
            {
                _timeAfterLastAttack += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_intervalForNextAttack - _timeAfterLastAttack < _boss.AttackInterval && !_boss.IsPreparingForAttack)
                {
                    _boss.StartAttack();
                }

                if (_timeAfterLastAttack >= _intervalForNextAttack &&
                    _boss.Health > 0)
                {
                    ShootNewBullet(gameState);
                    _timeAfterLastAttack = 0;
                    _intervalForNextAttack = RandomHelper.GetRandomFloatFromInterval(_boss.AttackRateMin, _boss.AttackRateMax) + _boss.AttackInterval;
                    _boss.StopAttack();
                }

            }

            if (_boss.Health == 0 && _activeBullets.Count == 0)
            {
                Stop(gameState);
            }
        }

        private void ShootNewBullet(GameState state)
        {
            var bullet = _boss.GenerateBullet();

            bullet.WorldPosition = _boss.GunCoordinates();

            // target point is somewhere between 2 nanobots in the center)

            var twoInTheMiddle = state.Player.Nanobots.OrderBy(bot => bot.WorldPosition.Y).Skip(1).Take(2);
            var target = (twoInTheMiddle.First().WorldPosition + twoInTheMiddle.Last().WorldPosition)/2;

            var bulletDirection = target - bullet.WorldPosition;

            float angle = (float) Math.Acos(Vector2.Dot(-1*Vector2.UnitX, Vector2.Normalize(bulletDirection)));
            bullet.DirectionAngle = MathHelper.Pi - Math.Sign(bulletDirection.Y)*angle;

            _activeBullets.Add(bullet);
            state.Level.AddLevelObject(bullet);
            SoundManager.EnemyAttack01.Play();

            if (state.Player.CanEvadeAttack)
            {
                state.Player.AddScript(new EvadeBossAttackScript(state, _boss.AttackMode, 400, 800));
            }
        }

        
    }
}
