using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Characters.Bosses;

namespace Cardio.Phone.Shared.Scripts
{
    public class GroupSwitchNearBossScript : GameEntityScript
    {
        private Boss _boss;

        public GroupSwitchNearBossScript(Boss boss)
        {
            _boss = boss;
        }

        protected override void OnStart(Core.GameState gameState)
        {
            _boss.Die += (o, e) =>
                             {
                                 gameState.Player.CanSwitch = true;
                                 Stop(gameState);
                             };
            base.OnStart(gameState);
        }

        public override void Update(Core.GameState gameState, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_boss.WorldPosition.X - gameState.Player.WorldPosition.X <= gameState.Player.AttackRange)
            {
                gameState.Player.CanSwitch = false;
            }
            base.Update(gameState, gameTime);
        }
    }
}
