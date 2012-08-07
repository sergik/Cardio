using Cardio.UI.Core;
using Cardio.UI.Extensions;
using Cardio.UI.Input.Matches;
using Cardio.UI.Input.Touch;
using Cardio.UI.Screens;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Input
{
    public class WindowsInputHandler: DrawableGameComponent
    {
        private readonly LevelScreen _host;

        public MatchStrategy MatchStrategy { get; set; }

        public WindowsInputHandler(Game game, LevelScreen host) : base(game)
        {
            _host = host;
        }

        public override void Initialize()
        {
            Game.Services.GetService<ITouchService>();
            Game.Services.GetService<GameState>();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsExitPressed())
            {
                _host.Pause();
                _host.ScreenManager.AddScreen(new PauseMenuScreen());
                return;
            }

            if (MatchStrategy != null)
            {
                MatchStrategy.Update();
            }

            base.Update(gameTime);
        }
    }
}
