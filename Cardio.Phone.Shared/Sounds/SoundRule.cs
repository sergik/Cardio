using System;
using System.Collections.Generic;

namespace Cardio.Phone.Shared.Sounds
{
    public class SoundRule
    {
        public Func<bool> Condition { get; set; }

        public SoundRule(Func<bool> condition, IList<Sound> sounds)
        {
            Condition = condition;
            Sounds = sounds;
        }

        public IList<Sound> Sounds { get; set; }
    }
}
