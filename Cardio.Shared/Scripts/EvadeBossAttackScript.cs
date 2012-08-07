using System.Linq;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class EvadeBossAttackScript : GameEntityScript
    {
        private GameState _state;

        private int _bossAttackMode;

        private float _movingTime;

        private float _transitionTime;

        public EvadeBossAttackScript(GameState state, int bossAttackMode, float movingTime, float transitionTime)
        {
            _state = state;
            _transitionTime = transitionTime;
            _movingTime = movingTime;
            _bossAttackMode = bossAttackMode;
        }

        protected override void OnStart(GameState gameState)
        {
            GenerateNanobotsEvasions();
            base.OnStart(gameState);

            Stop(gameState);
        }

        protected override void OnStop(GameState gameState)
        {
            _state.Player.CanEvadeAttack = false;
            base.OnStop(gameState);
        }

        private void GenerateNanobotsEvasions()
        {
            var botsByY = _state.Player.Nanobots.OrderBy(nano => nano.WorldPosition.Y).ToList();
            switch (_bossAttackMode)
            {
                case 0:
                    botsByY[0].AddScript(new EvasionScript(botsByY[0], new Vector2(10, -60), _movingTime, _transitionTime));
                    botsByY[1].AddScript(new EvasionScript(botsByY[1], new Vector2(30, -120), _movingTime, _transitionTime));
                    botsByY[2].AddScript(new EvasionScript(botsByY[2], new Vector2(-10, -140), _movingTime, _transitionTime));
                    botsByY[3].AddScript(new EvasionScript(botsByY[3], new Vector2(-200, -190), _movingTime, _transitionTime));
                    break;

                case 1:
                    botsByY[0].AddScript(new EvasionScript(botsByY[0], new Vector2(50, -40), _movingTime, _transitionTime));
                    botsByY[1].AddScript(new EvasionScript(botsByY[1], new Vector2(-30, -120), _movingTime, _transitionTime));
                    botsByY[2].AddScript(new EvasionScript(botsByY[2], new Vector2(-10, 40), _movingTime, _transitionTime));
                    botsByY[3].AddScript(new EvasionScript(botsByY[3], new Vector2(-10, 30), _movingTime, _transitionTime));
                    break;

                case 2:
                    botsByY[0].AddScript(new EvasionScript(botsByY[0], new Vector2(40, -70), _movingTime, _transitionTime));
                    botsByY[1].AddScript(new EvasionScript(botsByY[1], new Vector2(50, -80), _movingTime, _transitionTime));
                    botsByY[2].AddScript(new EvasionScript(botsByY[2], new Vector2(-30, 60), _movingTime, _transitionTime));
                    botsByY[3].AddScript(new EvasionScript(botsByY[3], new Vector2(-30, 60), _movingTime, _transitionTime));
                    break;
            }
        }
    }
}
