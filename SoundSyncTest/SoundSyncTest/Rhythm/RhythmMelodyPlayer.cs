using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SoundSyncTest.Rhythm
{
    /// <summary>
    /// Handles the synced playback of the single rhythm melody.
    /// </summary>
    public class RhythmMelodyPlayer
    {
        private readonly RhythmEngine _engine;

        private readonly IDictionary<SoundEffect, RhythmMelodyMetadata> _effectInstanceCache =
            new Dictionary<SoundEffect, RhythmMelodyMetadata>();

        private long _currentBeatCount;

        private long _targetBeatCount;

        private long _lastBeatCount;

        private RhythmMelodyMetadata _melodyMetadata;

        public SoundEffect Melody { get; private set; }

        public RhythmMelodyPlayer(RhythmEngine engine)
        {
            _engine = engine;
        }

        public void ChangeMelody(SoundEffect newMelody, long beatCount)
        {
            if (Melody == newMelody)
            {
                return;
            }

            if (Melody != null)
            {
                _engine.StopOnNextBeat(_melodyMetadata.Active);
            }

            _melodyMetadata = GetMelodyFromCacheOrCreate(newMelody);

            Melody = newMelody;
            if (Melody != null)
            {
                _engine.PlayOnNextBeat(_melodyMetadata.Active);
            }

            _currentBeatCount = 0L;
            _targetBeatCount = beatCount;
            _lastBeatCount = _engine.PatternPlayer.BeatsPlayed;
        }

        public void RemoveMelody()
        {
            if (Melody != null)
            {
                _engine.StopOnNextBeat(_melodyMetadata.Active);

                Melody = null;
                _melodyMetadata = null;
                _currentBeatCount = 0l;
                _targetBeatCount = 0l;
                _lastBeatCount = 0l;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Melody == null)
            {
                return;
            }

            var beats = _engine.PatternPlayer.BeatsPlayed;
            _currentBeatCount += beats - _lastBeatCount;

            if (_currentBeatCount >= _targetBeatCount)
            {
                _melodyMetadata.Swap();
                _engine.PlayOnNextBeat(_melodyMetadata.Active);
                _currentBeatCount %= _targetBeatCount;
            }

            _lastBeatCount = beats;
        }

        private RhythmMelodyMetadata GetMelodyFromCacheOrCreate(SoundEffect effect)
        {
            RhythmMelodyMetadata cached;
            if (!_effectInstanceCache.TryGetValue(effect, out cached))
            {
                cached = new RhythmMelodyMetadata(effect.CreateInstance(), effect.CreateInstance());
                _effectInstanceCache[effect] = cached;
            }

            return cached;
        }

    }
}
