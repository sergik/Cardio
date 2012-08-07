using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI
{
    public static class Fonts
    {
        public static SpriteFont Default { get; private set; }

        public static SpriteFont Tutorial { get; private set; }

        public static void Initialize(Game game)
        {
            Default = game.Content.Load<SpriteFont>(@"Fonts\MenuFont");
            Tutorial = game.Content.Load<SpriteFont>("Fonts\\Tutorial");
        }
    }
}
