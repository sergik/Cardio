using System;
using System.IO;
using Cardio.Phone.Shared.Persistance;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Screens.LevelMenu;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Screens
{
    /// <summary>
    /// This class doesn't work on mobile. Use <see cref="AlternativeMenuScreen"/> instead
    /// </summary>
    public class MainMenuScreen: MenuScreen
    {
        private Texture2D _background;

        public MainMenuScreen()
        {
            //var newGameMenu = new GrowingMenuEntry("NEW GAME")
            //{
            //    Position = new Vector2(120, 40),
            //};
            
            //newGameMenu.Selected += (o, e) =>
            //{
            //    // Reset GameState if it was changed by previous games or by loading from file.
            //    ScreenManager.Game.Services.GetService<GameState>().Reset();
            //    LoadingScreen.Load(ScreenManager, false, new LevelMenuScreen());
            //};

            //MenuEntries.Add(newGameMenu);


            //var demoLevelMenu = new GrowingMenuEntry("CONTINUE SAVED GAME")
            //{
            //    Position = new Vector2(120, 120),
            //    IsVisible = File.Exists(SavedGame.DefaultSaveGameFile)
            //};

            //demoLevelMenu.Selected += ContinueSavedGame;
            //MenuEntries.Add(demoLevelMenu);

            //var exitMenuItem = new GrowingMenuEntry("EXIT")
            //{
            //    Position = new Vector2(120, 200),
            //};

            //exitMenuItem.Selected += (o, e) => ScreenManager.Game.Exit();
            //MenuEntries.Add(exitMenuItem);

            var newGameMenu = new GrowingMenuEntry("Press here to continue...")
            {
                Position = new Vector2(230, 230),
            };

            newGameMenu.Selected += (o, e) =>
            {
                // Reset GameState if it was changed by previous games or by loading from file.
                //ScreenManager.Game.Services.GetService<GameState>().Reset();
                var state = ScreenManager.Game.Services.GetService<GameState>();
                SavedGame.Load(SavedGame.DefaultSaveGameFile, state);
                LoadingScreen.Load(ScreenManager, false, new ComixScreen());
            };

            MenuEntries.Add(newGameMenu);


        }

        public override void LoadContent()
        {
            ScreenManager.Game.IsMouseVisible = true;
            _background = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundTouch");

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

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.Draw(_background, new Rectangle(0, 0, 800, 480), Color.White);

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
