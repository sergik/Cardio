using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Input.Touch;

namespace Cardio.Phone.Shared.Input.Matches
{
    public class TouchSequenceMatchStrategy : MatchStrategy
    {
        private const float IncreseMatchCoefficient = 0.7f;

        private const float DecreaseMatchCoefficient = 0.5f;

        private int _beatIndexOnPreviousUpdate;

        private readonly List<ButtonType> _touchesPattern;

        private int _lastMatchedTouchIndex = -1;

        private readonly float _coeffDelta;

        public float MatchCoefficient{get; set;}

        public override void Update()
        {
            base.Update();

            // call Fail if user didn't press anything during a beat
            if (_beatIndexOnPreviousUpdate != BeatController.CurrentBeatNumber && _beatIndexOnPreviousUpdate != _lastMatchedTouchIndex)
            {
                OnFail(null);
            }
            _beatIndexOnPreviousUpdate = BeatController.CurrentBeatNumber;
        }

        public TouchSequenceMatchStrategy(GameState state, List<ButtonType> touchesPattern) : base(state)
        {
            _touchesPattern = touchesPattern;
            _coeffDelta = 1.0f/touchesPattern.Count;
        }

        protected override bool TouchIsValid(ButtonTouch touch)
        {
            return base.TouchIsValid(touch) && 
                   touch.TouchNumber > _lastMatchedTouchIndex &&
                   _touchesPattern[touch.TouchNumber] == touch.Button;
        }

        protected override void OnMactch(ButtonTouch touch)
        {
            _lastMatchedTouchIndex = touch.TouchNumber;

            MatchCoefficient += _coeffDelta * (1 - touch.PeakCoefficient / BeatController.AcceptableDerivationRadius * IncreseMatchCoefficient);

            //System.Diagnostics.Debug.WriteLine(string.Format("Match!\r\nCoefficient: {0}\r\nLast touch index: {1}", MatchCoefficient, _lastMatchedTouchIndex ));

            base.OnMactch(touch);
        }

        protected override void  OnFail(ButtonTouch touch)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("Fail!\r\nCoefficient: {0}\r\nLast touch index: {1}", MatchCoefficient, _lastMatchedTouchIndex));
            MatchCoefficient -= _coeffDelta*DecreaseMatchCoefficient;
            base.OnFail(touch);
        }
    }
}