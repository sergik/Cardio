using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Levels;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Screens.LevelMenu
{
    public class LevelSelectionScreen: DrawableGameComponent
    {
        private readonly MenuScreen _host;

        public LevelSelector LevelSelector
        {
            get; set;
        }

        public LevelSelectionScreen(Game game, MenuScreen host) : base(game)
        {
            _host = host;
        }

        protected override void LoadContent()
        {
            LevelSelector =
                LevelSelector.FromMetadata(
                    Game.Content.Load<LevelSelectorMetadata>(@"Levels\LevelsSelection"), Game.Content);
            LevelSelector.Position = new Vector2(120, 80);
            LevelSelector.Initialize(Game, _host);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            LevelSelector.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            LevelSelector.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void HandleInput()
        {
            LevelSelector.HandleInput();
        }
    }
}
