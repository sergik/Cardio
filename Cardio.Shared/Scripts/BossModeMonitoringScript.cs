using Cardio.UI.Characters.Bosses;
using Cardio.UI.Core;
using Cardio.UI.Extensions;
using Cardio.UI.Input;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class BossModeMonitoringScript : GameEntityScript
    {
        private readonly BossModeService _bossService;

        private readonly float _bossModeActivationDistance;

        public BossModeMonitoringScript(Game game, Boss boss, float bossModeActivationDistance)
        {
            _bossService = game.Services.GetService<BossModeService>();
            _bossService.Boss = boss;
            _bossModeActivationDistance = bossModeActivationDistance;
            _bossService.Boss.Die += (o, e) => _bossService.SwithBossModeOff();
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (_bossService.Boss.WorldPosition.X - gameState.Player.WorldPosition.X <= _bossModeActivationDistance)
            {
                _bossService.SwitchBossModeOn();
                Stop(gameState);
            }

            base.Update(gameState, gameTime);
        }
    }
}