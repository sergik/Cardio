using Cardio.UI.Core;
using Cardio.UI.Extensions;
using Cardio.UI.Persistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Screens.LevelMenu
{
    public class LevelMenuScreen: MenuScreen
    {
        private enum Tabs
        {
            LevelSelection,
            Shop
        }

        private MenuEntry _backMenu;
        private MenuEntry _levelSelectionMenu;
        private GrowingMenuEntry _shopMenu;
        private GrowingMenuEntry _saveMenu;

        private Tabs _activeTab = Tabs.LevelSelection;

        private bool _gameSaved;
        private Vector2 _gameSavedTextPosition;

        public LevelSelectionScreen LevelSelection { get; private set; }
        public ShopScreen Shop { get; private set; }

        public override void LoadContent()
        {
            _backMenu = new GrowingMenuEntry("BACK");
            MenuEntries.Add(_backMenu);

            _levelSelectionMenu = new GrowingMenuEntry("LEVEL");
            MenuEntries.Add(_levelSelectionMenu);

            _shopMenu = new GrowingMenuEntry("SHOP");
            MenuEntries.Add(_shopMenu);

            _saveMenu = new GrowingMenuEntry("SAVE");
            MenuEntries.Add(_saveMenu);

            LevelSelection = new LevelSelectionScreen(ScreenManager.Game, this);
            Shop = new ShopScreen(ScreenManager.Game);

            _backMenu.Position = new Vector2(120, 16);
            _backMenu.Selected += (s, e) => LoadingScreen.Load(ScreenManager, false, new MainMenuScreen());

            _levelSelectionMenu.Position = new Vector2(240, 16);
            _levelSelectionMenu.Selected += (s, e) => _activeTab = Tabs.LevelSelection;

            _shopMenu.Position = new Vector2(360, 16);
            _shopMenu.Selected += (s, e) => _activeTab = Tabs.Shop;

            _saveMenu.Position = new Vector2(480, 16);
            _saveMenu.Selected +=
                (s, e) =>
                    {
                        SavedGame.Save(SavedGame.DefaultSaveGameFile, ScreenManager.Game.Services.GetService<GameState>());
                        _saveMenu.IsVisible = false;
                        _gameSaved = true;
                    };

            _gameSavedTextPosition = _saveMenu.Position + new Vector2(0, 5);

            LevelSelection.Initialize();
            Shop.Initialize();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(LevelSelection);
            
            base.UnloadContent();
        }

        public override void HandleInput()
        {
            if (_activeTab == Tabs.LevelSelection)
            {
                LevelSelection.HandleInput();
            }
            else if (_activeTab == Tabs.Shop)
            {
                Shop.HandleInput();
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (_activeTab == Tabs.LevelSelection)
            {
                LevelSelection.Update(gameTime);
            }
            else if (_activeTab == Tabs.Shop)
            {
                Shop.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_activeTab == Tabs.LevelSelection)
            {
                LevelSelection.Draw(gameTime);
            }
            else if (_activeTab == Tabs.Shop)
            {
                Shop.Draw(gameTime);
            }

            if (_gameSaved)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.DrawString(Fonts.Default, "GAME SAVED", _gameSavedTextPosition, Color.Silver, 0,
                                                     Vector2.Zero, 0.7f, SpriteEffects.None, 0);
                ScreenManager.SpriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}
