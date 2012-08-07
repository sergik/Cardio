using Microsoft.Xna.Framework.Audio;

namespace Cardio.UI.Core
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