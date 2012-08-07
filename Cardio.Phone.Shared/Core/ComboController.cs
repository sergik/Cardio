using System.Collections.Generic;
using Cardio.Phone.Shared.Rhythm;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Cardio.Phone.Shared.Core
{
    public class ComboController : IPausable
    {
        private readonly int _maxComboLevel;

        private RhythmEngine _rhythmEngine;

        private SoundEffectInstance _comboUpSound;
        private SoundEffectInstance _comboBreakerSound;

        private readonly IDictionary<int, ComboLevelMelody> _melodies = new Dictionary<int, ComboLevelMelody>();

        public int CurrentComboLevel { get; private set; }

        public bool IsPaused { get; private set; }

        public void Pause()
        {
            if (!IsPaused)
            {
                if (_rhythmEngine != null)
                {
                    _rhythmEngine.Pause();
                }
                IsPaused = true;
            }
        }

        public void Resume()
        {
            if (IsPaused)
            {
                if (_rhythmEngine != null)
                {
                    _rhythmEngine.Resume();
                }
                IsPaused = false;
            }
        }

        public ComboController(int maxComboLevel)
        {
            _maxComboLevel = maxComboLevel;
        }

        public void Initialize(Game game)
        {
            _comboUpSound = game.Content.Load<SoundEffect>("Sounds\\Combo").CreateInstance();
            _comboBreakerSound = game.Content.Load<SoundEffect>(@"Sounds\ComboBreaker").CreateInstance();
            _rhythmEngine = game.Services.GetService<RhythmEngine>();

            _melodies.Clear();

            _melodies.Add(0, new ComboLevelMelody(game.Content.Load<SoundEffect>("Sounds\\Melody1Level1")));
            _melodies.Add(8, new ComboLevelMelody(game.Content.Load<SoundEffect>("Sounds\\Melody1Level2")));

            PlayMelody(true);
        }

        public void Update(GameTime gameTime)
        {
            
        }

        private void PlayMelody(bool stopIfNotExists)
        {
            // Call ChangeMelody only when melody is actually changed
            ComboLevelMelody melody;
            if (_melodies.TryGetValue(CurrentComboLevel, out melody) )
            {
                if (melody.Melody != _rhythmEngine.MelodyPlayer.Melody)
                {
                    _rhythmEngine.MelodyPlayer.ChangeMelody(melody.Melody);
                }
            }
            else if (stopIfNotExists)
            {
                _rhythmEngine.MelodyPlayer.RemoveMelody(false);
            }
        }

        public void IncreaseComboLevel()
        {
            if (CurrentComboLevel < _maxComboLevel)
            {
                CurrentComboLevel++;
                PlayMelody(false);
            }
        }

        public void Reset()
        {
            CurrentComboLevel = 0;
            PlayMelody(true);
        }
    }
}