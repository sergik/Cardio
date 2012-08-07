using System;

namespace SoundSyncTest.Rhythm
{
    public class BeatMatchingController
    {
        private readonly BeatPatternPlayer _patternPlayer;

        public float AcceptableBeatRadius { get; set; }

        public Func<bool> IsMatchingTriggered { get; set; }

        public bool? BeatMatched { get; private set; }

        public BeatMatchingController(BeatPatternPlayer patternPlayer)
        {
            _patternPlayer = patternPlayer;

            AcceptableBeatRadius = 130f;
        }

        public void Update()
        {
            if (IsMatchingTriggered == null)
            {
                BeatMatched = null;
                return;
            }

            var radius = _patternPlayer.Radius;

            var isTriggered = IsMatchingTriggered();
            if (isTriggered)
            {
                BeatMatched = radius <= AcceptableBeatRadius;
            }
            else
            {
                BeatMatched = null;
            }
        }
    }
}
