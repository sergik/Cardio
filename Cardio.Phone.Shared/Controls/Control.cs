using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Controls
{
    public class Control : DrawableGameComponent
    {
        protected SpriteBatch SpriteBatch { get; set; }

        public Vector2 Posistion { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Control(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteBatch = Game.Services.GetService<SpriteBatch>();
            base.Initialize();
        }
    }
}
