using System;
using Cardio.UI.Rhythm.BeatGenerators;

namespace Cardio.UI.Rhythm
{
    public class BeatMatchingController
    {
        private float _radius;

        private float _previousRadius;

        public BeatPatternGenerator PatternGenerator { get; private set; }

        public float AcceptableDerivationRadius { get; set; }

        public int CurrentBeatNumber { get { return PatternGenerator.CurrentBeatIndex; } }

        public Func<bool> IsMatchingTriggered { get; set; }

        public bool? BeatMatched { get; private set; }

        public BeatMatchingController(BeatPatternGenerator patternGenerator)
        {
            PatternGenerator = patternGenerator;

            AcceptableDerivationRadius = 200f;
        }

        public void Update()
        {
            if (IsMatchingTriggered == null)
            {
                BeatMatched = null;
                return;
            }

            _radius = PatternGenerator.Radius;

            // the condition below becomes true when we come into the radius of the new beat i. e. before actual beat sound
            if (_previousRadius > AcceptableDerivationRadius && _radius <= AcceptableDerivationRadius)
            {
                //CurrentBeatNumber++;
            }

            _previousRadius = _radius;

            var isTriggered = IsMatchingTriggered();
            if (isTriggered)
            {
                BeatMatched = _radius <= AcceptableDerivationRadius;
            }
            else
            {
                BeatMatched = null;
            }
        }

    }
}
