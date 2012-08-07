using System;
using System.Collections.Generic;

namespace Cardio.Engine.Sounds
{
    public class SoundState
    {
        public SoundState(String name, IList<Sound> sounds)
        {
            Name = name;
            Sounds = sounds;
        }

        public String Name { get; set; }

        public IList<Sound> Sounds { get; set; }
    }
}
