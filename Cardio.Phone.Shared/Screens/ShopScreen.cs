using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Screens;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Screens
{
    public class ShopScreen : GameScreen
    {
        private Game _game;

        public int? SelectedCategory { get; set; }

        public LevelShop ShopComponent
        {
            get; set;
        }

        public ShopScreen(Game game)
        {
            _game = game;
        }

        public void Initialize()
        {
            ShopComponent = LevelShop.FromMetadata(_game.Content.Load<LevelShopMetadata>(@"Shop\LevelShop1"));
            ShopComponent.SelectedTabIndex = SelectedCategory;
            ShopComponent.Position = new Vector2(50, 110);
            ShopComponent.Initialize(_game);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            ShopComponent.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ShopComponent.Draw(gameTime);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            ShopComponent.HandleInput();
        }
    }
}
