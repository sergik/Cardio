using Cardio.UI.Rhythm;
using Cardio.UI.Screens;
using Cardio.UI.Extensions;
using Cardio.UI.Screens.LevelMenu;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class GameOverScript: GameEntityScript
    {
        private float _targetTime = 3000;
        private float _currentTime;

        private readonly ScreenManager _screenManager;
        private readonly RhythmEngine _rhythmEngine;

        public GameOverScript(Game game)
        {
            _screenManager = game.Services.GetService<ScreenManager>();
            _rhythmEngine = game.Services.GetService<RhythmEngine>();
        }

        protected override void OnStart(Core.GameState gameState)
        {
            gameState.AreControlsEnabled = false;
            
            base.OnStart(gameState);
        }

        protected override void OnStop(Core.GameState gameState)
        {
            LoadingScreen.Load(_screenManager, false, new LevelMenuScreen());
            
            base.OnStop(gameState);
        }

        public override void Update(Core.GameState state, GameTime time)
        {
            if (_currentTime < _targetTime)
            {
                _currentTime += (float) time.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                Stop(state);
            }
            
            base.Update(state, time);
        }
    }
}
