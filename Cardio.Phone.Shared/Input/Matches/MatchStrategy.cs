using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Rhythm;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Input.Touch;
using Cardio.Phone.Shared.Rhythm;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework.Input;

namespace Cardio.Phone.Shared.Input.Matches
{
    public abstract class MatchStrategy
    {
        public BeatMatchingController BeatController { get; private set; }

        protected GameState State { get; private set; }

        public event EventHandler Failed;

        public event EventHandler Matched;

        protected MatchStrategy(GameState state)
        {
            State = state;
            var rhythm = State.Game.Services.GetService<RhythmEngine>();
            BeatController = new BeatMatchingController(rhythm.PatternGenerator)
            {
                IsMatchingTriggered = () => InputManager.IsKeyTriggered(Keys.A) || InputManager.IsKeyTriggered(Keys.S)
            };
        }

        public virtual void Update()
        {
            BeatController.Update();

            if (!BeatController.BeatMatched.HasValue || State.HandledMouseOnThisUpdate)
            {
                return;
            }

            SoundManager.ButtonHit.Play();
            var touch = new ButtonTouch
            {
                TouchType = TouchType.Allowed,
                TouchNumber = BeatController.CurrentBeatNumber,
                PeakCoefficient = BeatController.PatternGenerator.Radius,
                Button =
                    InputManager.IsKeyTriggered(Keys.A)
                        ? ButtonType.Left
                        : ButtonType.Right
            };

            if (TouchIsValid(touch))
            {
                OnMactch(touch);
            }
            else
            {
                OnFail(touch);
            }
        }

        protected virtual bool TouchIsValid(ButtonTouch touch)
        {
            return BeatController.BeatMatched == true;
        }

        protected virtual void OnMactch(ButtonTouch touch)
        {
            Matched.Fire(this, () => EventArgs.Empty);
        }

        protected virtual void OnFail(ButtonTouch touch)
        {
            Failed.Fire(this, () => EventArgs.Empty);
        }
    }
}
