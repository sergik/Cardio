using Microsoft.Xna.Framework.Audio;

namespace Cardio.Phone.Shared.Core
{
    public class ComboLevelMelody
    {
        public SoundEffect Melody { get; set; }

        public ComboLevelMelody(SoundEffect melody)
        {
            Melody = melody;
        }
    }
}