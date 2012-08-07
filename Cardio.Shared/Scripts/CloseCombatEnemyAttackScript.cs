using System;
using Cardio.UI.Characters;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class CloseCombatEnemyAttackScript: GameEntityScript
    {
        private static readonly Random Random = new Random();

        public CloseCombatEnemy Combatant { get; set; }

        public DrawableGameObject Target { get; set; }

        public CloseCombatEnemyAttackScript(CloseCombatEnemy combatant)
        {
            Combatant = combatant;
        }

        public override void Update(GameState state, GameTime time)
        {
            if (Combatant.WorldPosition.X - Target.WorldPosition.X > Combatant.AttackRange)
            {
                return;
            }

            for (int index = 0; index < state.Player.Nanobots.Count; index++)
            {
                var bot = state.Player.Nanobots[index];
                if (bot.Intersects(Combatant.WorldCollisionRectangle) != Rectangle.Empty)
                {
                    bot.Damage(new DamageTakenEventArgs(Combatant.AttackDamage, Combatant.WorldPosition));
                    Combatant.Health = 0;
                }
            }

            if (Combatant.Health == 0)
            {
                Stop(state);
            }

            var velocityDiff = Combatant.Acceleration * time.ElapsedGameTime.Milliseconds;
            var vector = new Vector2(Target.WorldCollisionRectangle.Center.X - Combatant.WorldCollisionRectangle.Center.X,
                    Target.WorldCollisionRectangle.Center.Y - Combatant.WorldCollisionRectangle.Center.Y);
            vector.Normalize();

            Combatant.Velocity += velocityDiff;
            if (Combatant.Velocity > Combatant.MaxVelocity)
            {
                Combatant.Velocity = Combatant.MaxVelocity;
            }

            Combatant.WorldPosition += vector * Combatant.Velocity;
   
            base.Update(state, time);
        }

        protected override void OnStart(GameState gameState)
        {
            Target = gameState.Player.Nanobots[Random.Next(gameState.Player.Nanobots.Count)];
            
            base.OnStart(gameState);
        }
    }
}
