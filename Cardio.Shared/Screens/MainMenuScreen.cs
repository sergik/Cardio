using System;
using System.IO;
using Cardio.UI.Core;
using Cardio.UI.Input;
using Cardio.UI.Extensions;
using Cardio.UI.Persistance;
using Cardio.UI.Screens.LevelMenu;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Screens
{
    public class MainMenuScreen: MenuScreen
    {
        public MainMenuScreen()
        {
            var newGameMenu = new GrowingMenuEntry("NEW GAME")
            {
                Position = new Vector2(120, 40),
            };
            
            newGameMenu.Selected += (o, e) =>
            {
                // Reset GameState if it was changed by previous games or by loading from file.
                ScreenManager.Game.Services.GetService<GameState>().Reset();
                LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen());
            };

            MenuEntries.Add(newGameMenu);


            var demoLevelMenu = new GrowingMenuEntry("CONTINUE SAVED GAME")
            {
                Position = new Vector2(120, 120),
                IsVisible = File.Exists(SavedGame.DefaultSaveGameFile)
            };

            demoLevelMenu.Selected += ContinueSavedGame;
            MenuEntries.Add(demoLevelMenu);

            var exitMenuItem = new GrowingMenuEntry("EXIT")
            {
                Position = new Vector2(120, 200),
            };

            exitMenuItem.Selected += (o, e) => ScreenManager.Game.Exit();
            MenuEntries.Add(exitMenuItem);
        }

        public override void LoadContent()
        {
            ScreenManager.Game.IsMouseVisible = true;

            base.LoadContent();
        }

        private void ContinueSavedGame(object sender, EventArgs e)
        {
            var state = ScreenManager.Game.Services.GetService<GameState>();
            SavedGame.Load(SavedGame.DefaultSaveGameFile, state);
            LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen());
        }

        public override void HandleInput()
        {
            if (InputManager.IsExitPressed())
            {
                ScreenManager.Game.Exit();
                return;
            }

            base.HandleInput();
        }
    }
}
