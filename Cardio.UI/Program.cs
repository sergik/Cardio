using System;

namespace Cardio.UI
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (CardioGame game = new CardioGame())
            {
                game.Run();
            }
        }
    }
#endif
}

