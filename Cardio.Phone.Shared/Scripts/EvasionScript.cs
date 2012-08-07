using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Scripts
{
    public class EvasionScript : GameEntityScript
    {
        private Nanobot _target;

        private Vector2 _offset;

        private float _time;

        private float _transitionTime;
        
        private float _currentTransitionTime;

        private MoveNanobotScript _movingToScript;

        public EvasionScript(Nanobot target, Vector2 offset, float time, float transitionTime)
        {
            _target = target;
            _time = time;
            _offset = offset;
            _transitionTime = transitionTime;
        }

        protected override void OnStart(GameState gameState)
        {
            _target.AddScript(_movingToScript = new MoveNanobotScript(_target, _offset, _time));
            base.OnStart(gameState);
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (_movingToScript.IsFinished)
            {
                _currentTransitionTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_currentTransitionTime >= _transitionTime)
                {
                    _target.AddScript(new MoveNanobotScript(_target, -_offset, _time));
                    Stop(gameState);
                }
            }
            base.Update(gameState, gameTime);
        }
    }
}