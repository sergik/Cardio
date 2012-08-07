using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Sounds;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class PlayerShootScript: GameEntityScript
    {
        protected IList<IAlive> Targets { get; set; }

        private readonly IList<IAlive> _actualTargets;

        protected float CurrentShootingTime { get; set; }

        private float _damagePerTime;

        public float ShootingTime { get; set; }

        public PlayerShootScript()
            : this(Enumerable.Empty<IAlive>()) { }

        public PlayerShootScript(IEnumerable<IAlive> targets)
        {
            Targets = targets.ToList();
            _actualTargets = new List<IAlive>(Enumerable.Repeat<IAlive>(null, 4));
            ShootingTime = 2000;

            UpdateActualTargets();
        }

        private void UpdateActualTargets()
        {
            var aliveTargets = Targets.Where(x => x.IsAlive).ToList();

            for (int i = 0; i < _actualTargets.Count; i++)
            {
                _actualTargets[i] = aliveTargets[i % aliveTargets.Count];
            }
        }

        public void AddTarget(IAlive target)
        {
            Targets.Add(target);
        }

        protected override void OnStart(GameState state)
        {
            state.Player.Confused += (o, e) => Stop(state);

            state.Player.IsShooting = true;
            CurrentShootingTime = 0;
            _damagePerTime = state.Player.AttackDamage/ShootingTime / _actualTargets.Count;

            state.RegisterBlockingHandler(this);
            SoundManager.BotAttack.Play();
            base.OnStart(state);
        }

        protected override void OnStop(GameState state)
        {
            state.Player.IsShooting = false;
            CurrentShootingTime = 0;
            state.ReleaseBlockingHandler(this);
            SoundManager.BotAttack.Stop(true);
            base.OnStop(state);
        }

        public override void Update( GameState state, GameTime gameTime)
        {
            bool atLeastOneEnemyDied = false;

            for (int index = 0; index < _actualTargets.Count; index++)
            {
                var enemy = _actualTargets[index];
                enemy.Damage(
                    new DamageTakenEventArgs((float) (_damagePerTime * gameTime.ElapsedGameTime.TotalMilliseconds),
                        (enemy as DrawableGameObject).WorldPosition));
                if (!enemy.IsAlive)
                {
                    atLeastOneEnemyDied = true;
                }
            }

            bool allDead = true;
            for (int i = 0; i < Targets.Count; i++ )
            {
                if (Targets[i].IsAlive)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                Stop(state);
                return;
            }

            if (atLeastOneEnemyDied)
            {
                UpdateActualTargets();
            }
            
            CurrentShootingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentShootingTime > ShootingTime)
            {
                Stop(state);
            }
        }
    }
}