using System;
using Cardio.UI.Rhythm;
using Cardio.UI.Extensions;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class LevelFinishedScript: GameEntityScript
    {
        private float _targetMoveTime = 4000;
        private float _currentMoveTime;

        private readonly RhythmEngine _rhythmEngine;

        public EventHandler Stopped;

        public LevelFinishedScript(Game game)
        {
            _rhythmEngine = game.Services.GetService<RhythmEngine>();
        }

        protected override void OnStart(Core.GameState state)
        {
            state.Camera.FocusedAt = null;

            state.AreControlsEnabled = false;

            base.OnStart(state);
        }

        protected override void OnStop(Core.GameState gameState)
        {
            base.OnStop(gameState);

            if (Stopped != null)
            {
                Stopped(this, EventArgs.Empty);
            }
        }

        public override void Update(Core.GameState state, GameTime time)
        {
            if (_currentMoveTime < _targetMoveTime)
            {
                var remainingDistance = state.Camera.Position.X + 2 * state.Camera.ViewportWidth - state.Player.WorldPosition.X;
                var remainingTime = _targetMoveTime - _currentMoveTime;
                var frameTime = (float)time.ElapsedGameTime.TotalMilliseconds;
                state.Player.WorldPosition =
                    new Vector2(state.Player.WorldPosition.X + frameTime / remainingTime * remainingDistance,
                        state.Player.WorldPosition.Y);

                _currentMoveTime += frameTime; 
            }
            else
            {
                Stop(state);
            }

            base.Update(state, time);
        }
    }
}
