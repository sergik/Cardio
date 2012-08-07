using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.UI.Extensions;

namespace Cardio.UI.Screens
{
    public class ShopScreen : GameScreen
    {
        private Game _game;

        public LevelShop ShopComponent
        {
            get; set;
        }

        public ShopScreen(Game game)
        {
            _game = game;
        }

        public void Initialize()
        {
            ShopComponent = LevelShop.FromMetadata(_game.Content.Load<LevelShopMetadata>(@"Shop\LevelShop1"));
            ShopComponent.Position = new Vector2(50, 100);
            ShopComponent.Initialize(_game);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            ShopComponent.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ShopComponent.Draw(gameTime);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            ShopComponent.HandleInput();
        }
    }
}
