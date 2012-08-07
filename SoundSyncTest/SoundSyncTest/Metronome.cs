using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SoundSyncTest
{
    public class Metronome: DrawableGameComponent
    {
        private SoundEffect _beatSound;
        private SoundEffect _melodySound;
        private SoundEffect _sound01;

        private float _melodyTime = 8000f;
        private float _currentMelodyTime;

        private float _beatsPerMinute;

        private float _playTime;
        private float _targetPlayTime;

        private long _lastTime;
        private Stopwatch _stopwatch;

        public bool PlayBzzOnNextBeat { get; set; }

        public Metronome(Game game) : base(game)
        {
            _beatsPerMinute = 120.0f;
            _targetPlayTime = 1000f/(_beatsPerMinute/60f);

        }

        private void OnTick(object state)
        {
            var now = _stopwatch.ElapsedMilliseconds;
            var milliseconds = now - _lastTime;
            var haveToPlay = (milliseconds != 0l && (_playTime == 0.0f || _playTime >= _targetPlayTime));

            if (haveToPlay)
            {
                _beatSound.Play();
                _playTime %= _targetPlayTime;

                if (_currentMelodyTime > _melodyTime)
                {
                    _melodySound.Play();
                    _currentMelodyTime %= _melodyTime;
                }

                if (PlayBzzOnNextBeat)
                {
                    _sound01.Play();
                    PlayBzzOnNextBeat = false;
                }
            }

            _playTime += milliseconds;
            _currentMelodyTime += milliseconds;

            _lastTime = now;
        }

        protected override void LoadContent()
        {
            _beatSound = Game.Content.Load<SoundEffect>(@"Sounds\MenuClick");
            _melodySound = Game.Content.Load<SoundEffect>(@"Sounds\Melody");
            _sound01 = Game.Content.Load<SoundEffect>(@"Sounds\ShieldActivate");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_stopwatch == null)
            {
                var timer = new Timer(OnTick, null, 0, 5);
                _stopwatch = Stopwatch.StartNew();
            }

            base.Update(gameTime);
        }
    }
}
