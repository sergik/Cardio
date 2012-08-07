using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework.Audio;

namespace Cardio.Phone.Shared.Rhythm
{
    /// <summary>
    /// Handles the synced playback of the single rhythm melody.
    /// </summary>
    public class RhythmMelodyPlayer : IPausable
    {
        private readonly RhythmEngine _engine;

        private readonly IDictionary<SoundEffect, SoundEffectInstance> _effectInstanceCache =
            new Dictionary<SoundEffect, SoundEffectInstance>();

        private SoundEffectInstance _melodyInstance;

        public SoundEffect Melody { get; private set; }

        public RhythmMelodyPlayer(RhythmEngine engine)
        {
            _engine = engine;
        }

        public void ChangeMelody(SoundEffect newMelody)
        {
            if (Melody == newMelody)
            {
                return;
            }

            if (Melody != null)
            {
                _engine.StopOnNextBeat(_melodyInstance);
            }

            Melody = newMelody;
            if (Melody != null)
            {
                _melodyInstance = GetMelodyFromCacheOrCreate(newMelody);
                _engine.PlayOnNextBeat(_melodyInstance);
            }
        }

        public void RemoveMelody(bool stopNow)
        {
            if (Melody != null)
            {
                if (stopNow)
                {
                    _melodyInstance.Stop();
                }
                else
                {
                    _engine.StopOnNextBeat(_melodyInstance);
                }

                Melody = null;
                _melodyInstance = null;
            }
        }

        private SoundEffectInstance GetMelodyFromCacheOrCreate(SoundEffect effect)
        {
            SoundEffectInstance cached;
            if (!_effectInstanceCache.TryGetValue(effect, out cached))
            {
                cached = effect.CreateInstance();
                cached.IsLooped = true;
                _effectInstanceCache[effect] = cached;
            }

            return cached;
        }

        public bool IsPaused { get; set; }

        public void Pause()
        {
            if (!IsPaused)
            {
                if (_melodyInstance != null)
                {
                    _melodyInstance.Pause();
                }
                IsPaused = true;
            }
        }

        public void Resume()
        {
            if (IsPaused)
            {
                if (_melodyInstance != null)
                {
                    _melodyInstance.Resume();
                }
                IsPaused = false;
            }
        }
    }
}
