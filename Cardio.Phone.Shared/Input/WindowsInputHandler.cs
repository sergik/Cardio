using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Input.Matches;
using Cardio.Phone.Shared.Input.Touch;
using Cardio.Phone.Shared.Screens;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Input
{
    public class WindowsInputHandler: DrawableGameComponent
    {
        private readonly LevelScreen _host;

        public MatchStrategy MatchStrategy { get; set; }

        private Game _game;

        public WindowsInputHandler(Game game, LevelScreen host) : base(game)
        {
            _host = host;
            _game = game;
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
                _host.ScreenManager.AddScreen(new PauseMenuScreen
                                                  {
                                                      Game = _game
                                                  });
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
