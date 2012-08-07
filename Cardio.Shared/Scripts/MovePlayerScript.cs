using Cardio.UI.Core;
using Cardio.UI.Scenes.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class MovePlayerScript: GameEntityScript
    {
        private float _currentDistance;
        public float Distance { get; set;}

        private float _currentMoveTime;
        public float MoveTime { get; set; }

        public MovePlayerScript()
        {
            Distance = 300;
            MoveTime = 2000;
        }

        protected override void OnStart(GameState state)
        {
            state.Player.Confused += (o, e) => Stop(state);

            state.Player.IsMoving = true;
            state.RegisterBlockingHandler(this);
            base.OnStart(state);
        }

        public override void Update(GameState state, GameTime gameTime)
        {
            if (!state.Level.NextStop.HasValue || state.Player.WorldPosition.X < state.Level.NextStop.Value)
            {
                MovePlayerIfPossible(gameTime, state);
            }
            else
            {
                state.Player.Confuse();
                Stop(state);
            }
        }

        protected override void OnStop(GameState state)
        {
            _currentDistance = 0;
            IsFinished = true;
            state.Player.IsMoving = false;
            state.ReleaseBlockingHandler(this);

            base.OnStop(state);
        }

        private void MovePlayerIfPossible(GameTime gameTime, GameState state)
        {
            if (_currentDistance < Distance)
            {
                var remainingDistance = Distance - _currentDistance;
                var remainingTime = MoveTime - _currentMoveTime;
                var delta = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                var shift = remainingDistance / remainingTime * delta;
                _currentDistance += shift;
                _currentMoveTime += delta;
                state.Player.WorldPosition = new Vector2(state.Player.WorldPosition.X + shift, state.Player.WorldPosition.Y);
            }
            else
            {
                Stop(state);
            }
        }
    }
}