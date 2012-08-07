using Microsoft.Xna.Framework.Audio;

namespace SoundSyncTest.Rhythm
{
    /// <summary>
    /// Due to some XNA sound API limitations, 
    /// for the gap-less looped melody playback (with beat syncing) within a rhythm engine, 
    /// we have to create 2 instances of the given sound effect.
    /// When the first instance ends, the second one starts, when it ends, the first one starts again and so on.
    /// This class provides stores those two instances and allows you to swap between them.
    /// </summary>
    public class RhythmMelodyMetadata
    {
        private readonly SoundEffectInstance _mainInstance;

        private readonly SoundEffectInstance _backCopy;

        public SoundEffectInstance Active { get; private set; }

        public RhythmMelodyMetadata(SoundEffectInstance mainInstance, SoundEffectInstance backCopy)
        {
            _mainInstance = mainInstance;
            _backCopy = backCopy;

            Active = _backCopy;
        }

        public void Swap()
        {
            Active = Active == _mainInstance ? _backCopy : _mainInstance;
        }
    }
}