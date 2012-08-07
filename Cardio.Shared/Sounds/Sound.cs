using System;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Cardio.UI.Sounds
{
    public class Sound : ContentObject
    {
        private SoundEffect _sound;
        private SoundEffectInstance _soundInstance;

        private string _soundName;

        public Sound(string soundName)
        {
            _soundName = soundName;
        }

        public bool IsLooped
        {
            get
            {
                return _soundInstance.IsLooped;
            }
            set
            {
                _soundInstance.IsLooped = value;
            }
        }
        
        public TimeSpan Duration
        {
            get 
            {
                return _sound.Duration;
            }
        }

        public void Initialize(Game game)
        {
            _sound = game.Content.Load<SoundEffect>(_soundName);
            _soundInstance = _sound.CreateInstance();
        }

        public void Play()
        {
            if (_soundInstance.State != SoundState.Playing)
            {
                _soundInstance.Play();
            }
        }

        public void Stop()
        {
            _soundInstance.Stop();
        }

        public void Pause()
        {
            _soundInstance.Pause();
        }

        public void Resume()
        {
            _soundInstance.Resume();
        }
    }
}
