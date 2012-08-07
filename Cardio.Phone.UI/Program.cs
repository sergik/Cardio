// ReSharper disable EmptyNamespace
namespace Cardio.Phone.UI
// ReSharper restore EmptyNamespace
{
#if WINDOWS || XBOX
    static class Program
    {
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

