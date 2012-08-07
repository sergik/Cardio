using System;

namespace Cardio.Phone.Shared.Screens
{
    public class ScreenLoadedEventArgs: EventArgs
    {
        public GameScreen Screen { get; private set; }

        public ScreenLoadedEventArgs(GameScreen screen)
        {
            Screen = screen;
        }
    }
}