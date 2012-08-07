using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Characters;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Projectiles;
using Cardio.UI.Scenes.Actions;
using Cardio.UI.Scenes.Scripts;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Scenes
{
    public class EnemyAttackScene: Scene
    {
        private static Random Random = new Random();

        /// <summary>
        /// Enemies in this scene.
        /// </summary>
        public IList<Character> Enemies { get; set; }

        private PlayerShootScript _playerAttackScript;

        private readonly IDictionary<Character, IGameScript> _enemyScripts =
            new Dictionary<Character, IGameScript>();

        public float AttackRange { get; set; }

        private bool _startedShooting;

        public EnemyAttackScene()
        {
            Enemies = new List<Character>();
            AttackRange = 600;
        }

        public override void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            var state = game.Services.GetService<GameState>();

            for (int index = 0; index < Enemies.Count; index++)
            {
                var enemy = Enemies[index];
                enemy.Initialize(game, spriteBatch, camera);

                //var range = enemy as IRangeEnemy;
                //if (range != null)
                //{
                //    _enemyScripts.Add(enemy,
                //        new RangeEnemyAttackScript(game, Random.Next((int) range.AttackInterval)) {Combatant = range});
                //}
                //else if (enemy is CloseCombatEnemy)
                //{
                //    _enemyScripts.Add(enemy, new CloseCombatEnemyAttackScript
                //    {
                //        Combatant = (CloseCombatEnemy) enemy,
                //        Target = state.Player.Nanobots[Random.Next(state.Player.Nanobots.Count())]
                //    });
                //}
            }

            Reactions.Add(PlayerAction.Shoot, ShootReaction);

            Length = 400;

            base.Initialize(game, spriteBatch, camera);
        }

        private void ShootReaction(GameState state)
        {
            if (StartPosition - state.Player.WorldPosition.X > AttackRange)
            {
                return;
            }

            if (_playerAttackScript == null || _playerAttackScript.IsFinished)
            {
                _playerAttackScript = new PlayerShootScript();
                for (int index = 0; index < Enemies.Count; index++)
                {
                    _playerAttackScript.AddTarget(Enemies[index]);
                }

                //state.AddScript(_playerAttackScript);
            }
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            base.Update(gameTime, gameState);

            if (!_startedShooting && (StartPosition - gameState.Player.WorldPosition.X <= AttackRange))
            {
                _startedShooting = true;

                foreach (var microbAttackScript in _enemyScripts.Values)
                {
                    //gameState.AddScript(microbAttackScript);
                }
            }

            float groupHealth = 0f;
            for (int i = 0; i < Enemies.Count; i++)
            {
                groupHealth += Enemies[i].Health;
            }

            IsFinished = groupHealth <= 0;

            for (int i = 0; i < Enemies.Count; i++)
            {
                var enemy = Enemies[i];
                enemy.WorldPosition = new Vector2(StartPosition + enemy.ScenePosition.X, enemy.ScenePosition.Y);
                enemy.Update(gameState, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            for (int index = 0; index < Enemies.Count; index++)
            {
                Enemies[index].Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
