using Microsoft.Xna.Framework.Audio;

namespace Cardio.Phone.Shared.Rhythm
{
    /// <summary>
    /// Describes the rhythm engine queue item.
    /// </summary>
    public class DelayedRhythmEntry
    {
        public long PlayAt { get; set; }

        public SoundEffectInstance Sound { get; set; }

        public DelayedRhythmEntry(long playAt, SoundEffectInstance sound)
        {
            PlayAt = playAt;
            Sound = sound;
        }
    }
}