using Cardio.UI.Characters.Ranged;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework.Content;

namespace Cardio.UI.Characters
{
    public class CloseCombatEnemy: ObstacleGameObject, IAttacker
    {
        public float AttackDamage { get; set; }

        public float MaxVelocity { get; set; }

        public float Velocity { get; set; }

        public float Acceleration { get; set; }

        /// <summary>
        /// A distnace to the target, when enemy starts moving
        /// </summary>
        public float AttackRange { get; set; }

        protected CloseCombatEnemy() {}

        public static void FillWithMetadata(CloseCombatEnemy enemy, CloseCombatEnemyMetadata metadata, ContentManager contentManager)
        {
            enemy.Acceleration = metadata.Acceleration;
            enemy.AttackDamage = metadata.AttackDamage;
            enemy.AttackRange = metadata.AttackRange;
            enemy.MaxVelocity = metadata.MaxVelocity;

            ObstacleGameObject.FillWithMetadata(enemy, metadata, contentManager);

            enemy.AddScript(new CloseCombatEnemyAttackScript(enemy));
        }
    }
}
