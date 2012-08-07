using Microsoft.Xna.Framework.Audio;

namespace SoundSyncTest.Rhythm
{
    /// <summary>
    /// This class handles the playback of the single beat pattern.
    /// It also exposes some properties (like current beat state/position),
    /// so the rhythm engine components can consume this information to perform some beat-synced actions.
    /// </summary>
    public sealed class BeatPatternPlayer
    {
        public bool IsEnabled { get; set; }
        
        public SoundEffect BeatSound { get; set; }

        public BeatPattern Pattern { get; set; }

        public bool BeatPlayedOnLastUpdate { get; private set; }

        public float BeatPosition { get; private set; }

        public long BeatsPlayed { get; private set; }

        public float Radius { get; private set; }

        private float _targetPlayTime;
        private float _beatsPerMinute;

        public float BeatsPerMinute
        {
            get { return _beatsPerMinute; }
            set
            {
                if (value < 1f)
                {
                    value = 1f;
                }

                _beatsPerMinute = value;
                _targetPlayTime = 1000f / (value / 60f);
            }
        }

        private float _currentPlayTime;

        public BeatPatternPlayer()
        {
            BeatsPerMinute = 120;
            IsEnabled = true;
        }

        public void Update(long elapsedMilliseconds)
        {
            if (!IsEnabled)
            {
                return;
            }

            var haveToPlay = (elapsedMilliseconds != 0L && (_currentPlayTime == 0.0f || _currentPlayTime >= _targetPlayTime));

            if (haveToPlay)
            {
                BeatSound.Play();
                BeatsPlayed++;
                _currentPlayTime %= _targetPlayTime;
            }

            Radius = _currentPlayTime <= _targetPlayTime / 2f 
                ? _currentPlayTime
                : _targetPlayTime - _currentPlayTime;

            _currentPlayTime += elapsedMilliseconds;
            BeatPlayedOnLastUpdate = haveToPlay;
            BeatPosition = _currentPlayTime / _targetPlayTime;
        }
    }
}