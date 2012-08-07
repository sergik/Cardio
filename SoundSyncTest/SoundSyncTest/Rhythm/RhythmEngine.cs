using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SoundSyncTest.Rhythm
{
    public class RhythmEngine: GameComponent
    {
        private readonly BeatPatternPlayer _patternPlayer;

// ReSharper disable UnaccessedField.Local
        /// <summary>
        /// This timer instance is stored to prevent the GC from killing the timer thread.
        /// </summary>
        private Timer _timer;
// ReSharper restore UnaccessedField.Local

        public BeatPatternPlayer PatternPlayer { get { return _patternPlayer; } }

        private Stopwatch _stopwatch;

        private long _lastTime = 15;

        private readonly Queue<DelayedRhythmEntry> _playQueue = new Queue<DelayedRhythmEntry>();
        private readonly Queue<DelayedRhythmEntry> _stopQueue = new Queue<DelayedRhythmEntry>();

        public RhythmEngine(Game game) : base(game)
        {
            _patternPlayer = new BeatPatternPlayer {BeatsPerMinute = 120};
        }

        public void PlayOnNextBeat(SoundEffectInstance sound)
        {
            var beat = _patternPlayer.BeatsPlayed + 1;
            _playQueue.Enqueue(new DelayedRhythmEntry(beat, sound));
        }

        public void PlayOnThisBeat(SoundEffectInstance sound)
        {
            var beat = _patternPlayer.BeatsPlayed;
            _playQueue.Enqueue(new DelayedRhythmEntry(beat, sound));
        }

        public void StopOnNextBeat(SoundEffectInstance sound)
        {
            var beat = _patternPlayer.BeatsPlayed + 1;
            _stopQueue.Enqueue(new DelayedRhythmEntry(beat, sound));
        }

        public override void Initialize()
        {
            _patternPlayer.BeatSound = Game.Content.Load<SoundEffect>(@"Sounds\Heartbeat1");
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_stopwatch == null)
            {
                _stopwatch = Stopwatch.StartNew();
                _timer = new Timer(OnTick, null, 0, 5);
            }

            base.Update(gameTime);
        }

        private void OnTick(object state)
        {
            var now = _stopwatch.ElapsedMilliseconds;
            var milliseconds = now - _lastTime;

            _patternPlayer.Update(milliseconds);

            var beat = _patternPlayer.BeatsPlayed;

            if (_patternPlayer.BeatPlayedOnLastUpdate)
            {
                while (_playQueue.Count > 0)
                {
                    var item = _playQueue.Peek();
                    if (item.PlayAt < beat)
                    {
                        break;
                    }

                    item.Sound.Play();
                    _playQueue.Dequeue();
                }

                while (_stopQueue.Count > 0)
                {
                    var item = _stopQueue.Peek();
                    if (item.PlayAt < beat)
                    {
                        break;
                    }
                    item.Sound.Stop();
                    _stopQueue.Dequeue();
                }
            }

            _lastTime = now;
        }
    }
}
