using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Input.Touch;

namespace Cardio.Phone.Shared.Input.Matches
{
    public class DefaultMatchStrategy : MatchStrategy
    {
        private readonly ITouchService _touchService;

        public DefaultMatchStrategy(GameState state)
            : base(state)
        {
            _touchService = state.Game.Services.GetService<ITouchService>();
        }

        /// <summary>
        /// Returns false if it's out of the beat radius, if touchese were performed not on sequential beats, if 4-touches combination cannot be executed
        /// </summary>
        /// <param name="touch"></param>
        /// <returns></returns>
        protected override bool TouchIsValid(ButtonTouch touch)
        {
            bool beatMatched = base.TouchIsValid(touch);
            if (beatMatched && !State.ReactionProgress.IsReactionInProgress)
            {
                _touchService.Push(touch);

                if (!_touchService.ContainsValidSequence())
                {
                    return false;
                }

                if (_touchService.Touches.Count == 4)
                {
                    bool validInput = State.Level.HandleInput(State, _touchService.Touches);
                    _touchService.Reset();
                    return validInput;
                }
                return true;
            }
            return false;
        }

        protected override void OnFail(ButtonTouch touch)
        {
            _touchService.Reset();
            base.OnFail(touch);
        }
    }
}