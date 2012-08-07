using Cardio.UI.Screens.LevelMenu;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Screens
{
    public class PauseMenuScreen: MenuScreen
    {
        public PauseMenuScreen()
        {
            var newGameMenu = new GrowingMenuEntry("MENU")
                                  {
                                      Position = new Vector2(300, 150),
                                  };
            newGameMenu.Selected +=
                (o, e) => LoadingScreen.Load(ScreenManager, true, new LevelMenuScreen());
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