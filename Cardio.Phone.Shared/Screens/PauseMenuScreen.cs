using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Levels;
using Cardio.Phone.Shared.Screens.LevelMenu;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Screens
{
    public class PauseMenuScreen: MenuScreen
    {
        public Game Game { get; set; }

        public PauseMenuScreen()
        {
            //var level = Game.Services.GetService<GameState>().Level;
            
            var newGameMenu = new GrowingMenuEntry("MENU")
                                  {
                                      Position = new Vector2(300, 150),
                                  };
            newGameMenu.Selected +=
                (o, e) => 
                    {
                        LoadingScreen.Load(ScreenManager, true, new LevelMenuScreen());
                        //level.Melody.Stop();
                    };
            MenuEntries.Add(newGameMenu);

            var resumeGameMenu = new GrowingMenuEntry("RESUME GAME")
                                     {
                                         Position = new Vector2(300, 230),
                                     };
            resumeGameMenu.Selected += (o, e) => ExitScreen();
            MenuEntries.Add(resumeGameMenu);
        }
    }
}