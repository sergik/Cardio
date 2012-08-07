using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Characters.Bosses;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Levels;
using Cardio.Phone.Shared.Scripts;

namespace Cardio.Phone.Shared.Actions
{
    public class ActionReaction
    {
        public Func<GameState, bool> CanBeIvoked { get; set; }

        public Action<GameState> Invoke { get; set; }

        public int Duration { get; set; }

        public static ActionReaction ShootReaction(Level level, GameState state)
        {
            Func<Level, IEnumerable<IPlayerAttackTarget>> findEnemies = l => l.CustomLevelObjects
                                                                                 .OfType<IPlayerAttackTarget>()
                                                                                 .Where(
                                                                                     enemy => enemy.Health > 0 &&
                                                                                              enemy.WorldPosition.X <=
                                                                                              state.Player.WorldPosition
                                                                                                  .X +
                                                                                              state.Player.AttackRange).
                                                                                 OrderBy(
                                                                                     target =>
                                                                                     target.PlayerAttackPriority);
            const int duration = 1500;
            return new ActionReaction
                       {
                           Duration = duration,
                           CanBeIvoked = gameState => findEnemies(gameState.Level).Any(),
                           Invoke = gameState =>
                                        {
                                            var enemiesInRange = findEnemies(gameState.Level);
                                            gameState.Player.AddScript(new PlayerShootScript(enemiesInRange)
                                                                           {
                                                                               ShootingTime =
                                                                                   duration +
                                                                                   gameState.ReactionProgress.
                                                                                       ReactionInertia
                                                                           });
                                        }
                       };
        }

        public static ActionReaction MoveReaction(Level level, GameState state)
        {
            return null;
        }

        public static ActionReaction UseActiveItem(GameState state, int index)
        {
            return new ActionReaction
            {
                CanBeIvoked = gameState => gameState.Inventory.ActiveItemExistsAt(index),
                Invoke = gameState => state.Inventory.UseItemFromSlot(index, gameState)
            };
        }

        public static ActionReaction EvadeReaction(GameState state)
        {
            Func<GameState, Boss> findBoss =
                gameState => gameState.Level.CustomLevelObjects.OfType<Boss>().FirstOrDefault(
                    boss =>
                    boss.WorldPosition.X - gameState.Player.WorldPosition.X <=
                    gameState.Player.AttackRange && boss.IsPreparingForAttack);

            const int duration = 1700;
            return new ActionReaction
                       {
                           CanBeIvoked = gameState => findBoss(gameState) != null,
                           Duration = duration,
                           Invoke =
                               gameState => gameState.Player.CanEvadeAttack = true
                       };
    }
    }
}
