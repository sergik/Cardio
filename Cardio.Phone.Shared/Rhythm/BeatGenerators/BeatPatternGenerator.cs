using Microsoft.Xna.Framework.Audio;

namespace Cardio.Phone.Shared.Rhythm.BeatGenerators
{
    public class BeatPatternGenerator
    {
        private readonly object _syncRoot = new object();

        private float _radius;

        protected float TargetPlayTime { get; set; }

        protected bool NearestBeatIndexUpdated { get; set; }

        protected float CurrentPlayTime { get; set; }

        public BeatPatternGenerator(float intervalBetweenBeats)
        {
            IsEnabled = true;
            TargetPlayTime = intervalBetweenBeats;
        }

        public bool IsEnabled { get; set; }

        public SoundEffectInstance BeatSound { get; set; }

        public bool BeatPlayedOnLastUpdate { get; protected set; }

        public int BeatsPlayed { get; protected set; }

        public int CurrentBeatIndex { get; set; }

        public float Radius
        {
            get
            {
                lock (_syncRoot)
                {
                    return _radius;
                }
            }

            set
            {
                lock (_syncRoot)
                {
                    _radius = value;
                }
            }
        }

        public virtual void Update(long elapsedMilliseconds)
        {
            if (!IsEnabled)
            {
                return;
            }

            var haveToPlay = HaveToPlay(elapsedMilliseconds);

            if (haveToPlay)
            {
                Play();
            }

            UpdateState(elapsedMilliseconds, haveToPlay);
        }

        protected virtual void UpdateState(long elapsedMilliseconds, bool haveToPlay)
        {
            Radius = CurrentPlayTime <= TargetPlayTime / 2f
                         ? CurrentPlayTime
                         : TargetPlayTime - CurrentPlayTime;

            CurrentPlayTime += elapsedMilliseconds;

            if (CurrentPlayTime > TargetPlayTime / 2 && !NearestBeatIndexUpdated)
            {
                CurrentBeatIndex++;
                NearestBeatIndexUpdated = true;
            }
            BeatPlayedOnLastUpdate = haveToPlay;
        }

        protected virtual bool HaveToPlay(long elapsedMilisec)
        {
            return elapsedMilisec != 0L && (CurrentPlayTime == 0.0f || CurrentPlayTime >= TargetPlayTime);
        }

        protected virtual void Play()
        {
            BeatSound.Play();
            BeatsPlayed++;
            NearestBeatIndexUpdated = false;
            CurrentPlayTime %= TargetPlayTime;
        }
    }
}