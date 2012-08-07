using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Screens.LevelMenu;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Screens
{
    public class ConfirmationScreen: GameScreen
    {
        private ConfirmationDialog<LevelShopItem> Dialog { get; set; }

        private Game _game;

        public LevelShopItem Item { get; set; }

        public int? ShopSelectedCategory { get; set; }

        public ConfirmationScreen(Game game)
        {
            _game = game;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            
            Dialog = new ConfirmationDialog<LevelShopItem>(_game);
            Dialog.Initialize();
            Dialog.DrawColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            Dialog.NeedDisplayText += i => i.ItemTitle;
            Dialog.Item = Item;

            Dialog.CancelButton.Selected += (s, e) => LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen
                                                                                                   {
                                                                                                       SelectedTab = LevelMenuScreen.Tabs.Shop,
                                                                                                       ShopSelectedCategory = ShopSelectedCategory
                                                                                                   });
            Dialog.OkButton.Selected += (s, e) =>
                                            {
                                                Item.OnBuy();
                                                LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen
                                                                                             {
                                                                                                 SelectedTab = LevelMenuScreen.Tabs.Shop,
                                                                                                 ShopSelectedCategory = ShopSelectedCategory
                                                                                             });
                                            };

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            Dialog.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Dialog.Draw(gameTime);
        }
    }
}
