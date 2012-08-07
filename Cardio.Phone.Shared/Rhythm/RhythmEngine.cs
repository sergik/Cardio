using System.Collections.Generic;
using System.Threading;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Rhythm;
using Cardio.Phone.Shared.Rhythm.BeatGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Cardio.Phone.Shared.Rhythm
{
    public class RhythmEngine: GameComponent, IPausable
    {
        private BeatPatternGenerator _patternGenerator;

// ReSharper disable UnaccessedField.Local
        /// <summary>
        /// This timer instance is stored to prevent the GC from killing the timer thread.
        /// </summary>
        private Timer _timer;
// ReSharper restore UnaccessedField.Local

        public BeatPatternGenerator PatternGenerator 
        { 
            get
            {
                return _patternGenerator;
            } 
            set
            {
                _patternGenerator = value;
            }
        }

        public RhythmMelodyPlayer MelodyPlayer { get; private set; }

        //private Stopwatch _stopwatch;

        private long _lastTime = 15;

        public bool IsPaused { get; private set; }

        private readonly Queue<DelayedRhythmEntry> _playQueue = new Queue<DelayedRhythmEntry>();
        private readonly Queue<DelayedRhythmEntry> _stopQueue = new Queue<DelayedRhythmEntry>();

        public RhythmEngine(Game game) : base(game)
        {
            _patternGenerator = new BeatPatternGenerator(500);
            MelodyPlayer = new RhythmMelodyPlayer(this);
        }

        public void PlayOnNextBeat(SoundEffectInstance sound)
        {
            var beat = _patternGenerator.BeatsPlayed + 1;
            _playQueue.Enqueue(new DelayedRhythmEntry(beat, sound));
        }

        public void PlayOnThisBeat(SoundEffectInstance sound)
        {
            var beat = _patternGenerator.BeatsPlayed;
            _playQueue.Enqueue(new DelayedRhythmEntry(beat, sound));
        }

        public void StopOnNextBeat(SoundEffectInstance sound)
        {
            var beat = _patternGenerator.BeatsPlayed + 1;
            _stopQueue.Enqueue(new DelayedRhythmEntry(beat, sound));
        }

        public void Pause()
        {
            IsPaused = true;
            MelodyPlayer.Pause();
        }

        public void Resume()
        {
            IsPaused = false;
            MelodyPlayer.Resume();
        }

        public override void Initialize()
        {
            _patternGenerator.BeatSound = Game.Content.Load<SoundEffect>(@"Sounds\Heartbeat1").CreateInstance();
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
        //    if (_stopwatch == null)
        //    {
        //        _stopwatch = Stopwatch.StartNew();
        //        _timer = new Timer(OnTick, null, 0, 5);
        //    }

            base.Update(gameTime);
        }

        private void OnTick(object state)
        {
            if (IsPaused)
            {
                ProcessStopQueue(0L);
                return;
            }

            //var now = _stopwatch.ElapsedMilliseconds;
            //var milliseconds = now - _lastTime;

            //_patternGenerator.Update(milliseconds);


            var beat = _patternGenerator.BeatsPlayed;

            if (_patternGenerator.BeatPlayedOnLastUpdate)
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

                ProcessStopQueue(beat);
            }

           // _lastTime = now;
        }

        private void ProcessStopQueue(long beat)
        {
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
    }
}
