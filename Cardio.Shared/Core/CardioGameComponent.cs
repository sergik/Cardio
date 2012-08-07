using Cardio.UI.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Core
{
    public class CardioGameComponent: DrawableGameComponent
    {
        protected GameState GameState { get; private set; }

        protected SpriteBatch SpriteBatch { get; private set; }

        public CardioGameComponent(Game game) : base(game)
        {
            SpriteBatch = game.Services.GetService<SpriteBatch>();
            GameState = game.Services.GetService<GameState>();
        }

    }
}
