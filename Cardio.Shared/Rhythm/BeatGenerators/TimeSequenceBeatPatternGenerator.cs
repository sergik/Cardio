using System.Collections.Generic;

namespace Cardio.UI.Rhythm.BeatGenerators
{
    public class TimeSequenceBeatPatternGenerator : BeatPatternGenerator
    {
        private readonly List<float> _intervals;

        public TimeSequenceBeatPatternGenerator(List<float> intervals) : base(0)
        {
            _intervals = intervals;
        }

        protected override void Play()
        {
            base.Play();
            if (BeatsPlayed >= _intervals.Count)
            {
                BeatsPlayed = 0;
            }
        }

        protected override bool HaveToPlay(long elapsedMilisec)
        {
            return elapsedMilisec != 0L && CurrentPlayTime >= TargetPlayTime;
        }

        public override void Update(long elapsedMilliseconds)
        {
            if (!IsEnabled)
            {
                return;
            }
            TargetPlayTime = _intervals[BeatsPlayed];
            var haveToPlay = HaveToPlay(elapsedMilliseconds);

            if (haveToPlay)
            {
                Play();
            }

            UpdateState(elapsedMilliseconds, haveToPlay);
        }

        protected override void  UpdateState(long elapsedMilliseconds, bool haveToPlay)
        {
            base.UpdateState(elapsedMilliseconds, haveToPlay);

            if (CurrentBeatIndex >= _intervals.Count)
            {
                CurrentBeatIndex = 0;
            }
        }
    }
}