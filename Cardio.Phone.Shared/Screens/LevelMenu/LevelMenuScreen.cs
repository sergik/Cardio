using Cardio.Phone.Shared.Persistance;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Screens;
using Cardio.Phone.Shared;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Screens.LevelMenu
{
    public class LevelMenuScreen: MenuScreen
    {
        public enum Tabs
        {
            LevelSelection,
            Shop
        }

        private MenuEntry _levelSelectionMenu;
        private GrowingMenuEntry _shopMenu;
        private GrowingMenuEntry _exitMenu;

        private Tabs _activeTab = Tabs.LevelSelection;

        public LevelSelectionScreen LevelSelection { get; private set; }
        public ShopScreen Shop { get; private set; }

        public Tabs? SelectedTab { get; set; }

        public int? ShopSelectedCategory { get; set; }


        public override void LoadContent()
        {
            _levelSelectionMenu = new GrowingMenuEntry("LEVEL");
            MenuEntries.Add(_levelSelectionMenu);

            _shopMenu = new GrowingMenuEntry("SHOP");
            MenuEntries.Add(_shopMenu);

            _exitMenu = new GrowingMenuEntry("EXIT");
            MenuEntries.Add(_exitMenu);

            LevelSelection = new LevelSelectionScreen(ScreenManager.Game, this);
            Shop = new ShopScreen(ScreenManager.Game);
            Shop.SelectedCategory = ShopSelectedCategory;

            _levelSelectionMenu.Position = new Vector2(120, 16);
            _levelSelectionMenu.Selected += (s, e) => _activeTab = Tabs.LevelSelection;

            _shopMenu.Position = new Vector2(240, 18);
            _shopMenu.Selected += (s, e) => _activeTab = Tabs.Shop;
           
            _exitMenu.Position = new Vector2(360, 18);
            _exitMenu.Selected += (s, e) => ScreenManager.Game.Exit();

            LevelSelection.Initialize();
            Shop.Initialize();

            SoundManager.MenuMelody.Play();

            if(SelectedTab.HasValue)
            {
                _activeTab = SelectedTab.Value;
            }

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(LevelSelection);
            SoundManager.MenuMelody.Pause();
            
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

            base.Draw(gameTime);
        }
    }
}
