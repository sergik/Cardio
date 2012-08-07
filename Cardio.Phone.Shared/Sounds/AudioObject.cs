using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Sounds
{
    public class AudioObject
    {
        private Game _game;

        private readonly IList<SoundRule> Rules;

        private IList<Sound> CurrentSoundState
        {
            get; set;
        }

        public Boolean IsInitialized
        {
            get { return _game != null; }
        }

        public AudioObject()
        {
            Rules = new List<SoundRule>();
        }

        public void AddState(SoundRule rule)
        {
            Rules.Add(rule);
        }

        public void Initialize(Game game)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Component is already initialized");
            }

            _game = game;

            for (int index = 0; index < Rules.Count; index++)
            {
                var state = Rules[index];
                for (int i = 0; i < state.Sounds.Count; i++)
                {
                    var sound = state.Sounds[i];
                    sound.Initialize(_game);
                }
            }
        }

        public void UpdateState()
        {
            StopAll();
            var sounds = Rules.Where(x => x.Condition()).SelectMany(x => x.Sounds).ToList();
            CurrentSoundState = sounds;
        }

        public void Play()
        {
            foreach (var sound in CurrentSoundState)
            {
                sound.Play();
            }
        }

        public void Stop()
        {
            foreach (var sound in CurrentSoundState)
            {
                sound.Stop();
            }
        }

        public void StopAll()
        {
            for (int index = 0; index < Rules.Count; index++)
            {
                var soundRule = Rules[index];
                for (int i = 0; i < soundRule.Sounds.Count; i++)
                {
                    var sound = soundRule.Sounds[i];
                    sound.Stop();
                }
            }
        }
    }
}
