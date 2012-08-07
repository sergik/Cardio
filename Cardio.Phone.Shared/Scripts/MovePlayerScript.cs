using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Levels;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Scripts
{
    public class MovePlayerScript: GameEntityScript
    {
        private Level _level;
        public const float PlayerSpeed = 2.5f;
        public float MovingSpeed { get; set;}

        public MovePlayerScript()
        {
            MovingSpeed = PlayerSpeed;
        }

        protected override void OnStart(GameState state)
        {
            state.Player.IsMoving = true;
            _level = state.Level;
            base.OnStart(state);
        }

        public override void Update(GameState state, GameTime gameTime)
        {
            MovePlayerIfPossible(gameTime, state);

            if (state.Level.GetDistanceForClosestEnemy(state.Player.WorldPosition) < state.Camera.ViewportWidth - 100 && state.Player.Handled)
            {
                state.Player.IsShooting = true;
            }
            else
            {
                state.Player.IsShooting = false;
            }
        }

        private void MovePlayerIfPossible(GameTime gameTime, GameState state)
        {
            var stopPosition = _level.GetNextStopPosition(state.Player.WorldPosition);
            if (!stopPosition.HasValue || stopPosition.Value.X - state.Player.WorldPosition.X> GameState.ScreenLength - 300)
            {
                state.Player.WorldPosition = new Vector2(state.Player.WorldPosition.X + MovingSpeed,
                                                         state.Player.WorldPosition.Y);
            }
        }
    }
}