using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Scripts
{
    public class MoveNanobotScript : GameEntityScript
    {
        private Nanobot _target;

        private Vector2 _offset;

        private float _time;

        private Vector2 _delta;

        private float _currentMovingTime;

        public MoveNanobotScript(Nanobot target, Vector2 offset, float time)
        {
            _target = target;
            _time = time;
            _offset = offset;
        }

        protected override void OnStart(GameState gameState)
        {
            _delta = _offset/_time;
            base.OnStart(gameState);
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            _target.GroupPosition = _target.GroupPosition + _delta * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _currentMovingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_currentMovingTime >= _time)
            {
                Stop(gameState);
            }
            base.Update(gameState, gameTime);
        }
    }
}
