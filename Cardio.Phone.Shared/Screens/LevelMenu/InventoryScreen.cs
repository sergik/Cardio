using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Characters.Player;
using Cardio.Phone.Shared.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Screens.LevelMenu
{
    public class InventoryScreen: DrawableGameComponent
    {
        private readonly GameScreen _host;
        public IList<Nanobot> Nanobots { get; private set; }

        public Vector2 Offset { get; set; }

        public IList<InventoryItem> AvailableItems { get; private set; }

        public float RowMargin { get; set; }

        public float ColumnMargin { get; set; }

        public Point ItemRenderSize { get; set; }

        private String _infoText =
            @"DURING THEIR TRAVEL, YOUR BOTS WILL COLLECT VARIOUS KINDS OF BIOLOGICAL MATERIAL, FORMATIONS AND ELEMENTS.

THANKS TO THE MODERN HIGH TECHNOLOGIES, THE BOTS CAN ADAPT BETTER TO THE PACIENT'S BODY BY CONSUMING THAT MATERIALS AND IMPROVING THEIR CHARACTERISTICS ACCORDINGLY

HERE YOU WILL SPEND COLLECTED MATERIAL ON UPGRADES AND NEW ABILITIES";

        private Texture2D _infoTexture;
        private Rectangle _infoTextureRectangle = new Rectangle(360, 48, 400, 400);

        public InventoryScreen(Game game, GameScreen host) : base(game)
        {
            _host = host;

            AvailableItems = new List<InventoryItem>();
            Nanobots = new List<Nanobot>();
            for (int i = 0; i < 4; i++ )
            {
                Nanobots.Add(new Nanobot());
            }

            RowMargin = 16;
            ColumnMargin = 16;
            ItemRenderSize = new Point(32, 32);
            Offset = new Vector2(120, 64);
        }

        public override void Initialize()
        {
            //AvailableItems.Add(TestData.BuildInventoryMedkit(_host.ScreenManager.Game.Content));

            //int index = 0;
            //foreach (var nanobot in Nanobots)
            //{
            //    nanobot.Initialize(_host.ScreenManager.Game, _host.ScreenManager.SpriteBatch, null);
            //    nanobot.CarriedItem = TestData.BuildInventoryMedkit(_host.ScreenManager.Game.Content);
            //    var requiredHeight = 80;
            //    var scale = requiredHeight / nanobot.AutoSize.Y;
            //    nanobot.Size = nanobot.AutoSize * scale;

            //    nanobot.WorldPosition = new Vector2(Offset.X, Offset.Y + index * (requiredHeight + RowMargin));

            //    index++;
            //}
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _infoTexture = Game.Content.Load<Texture2D>(@"Textures\Tutorial\Inventory");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
            
            base.Update(gameTime);
        }

        public void HandleInput()
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var nanobot in Nanobots)
            {
                nanobot.Draw(gameTime);

                if (nanobot.CarriedItem != null)
                {
                    _host.ScreenManager.SpriteBatch.Begin();
                    var botSize = nanobot.Size ?? nanobot.AutoSize;
                    var rect = new Rectangle(
                        (int) (nanobot.WorldPosition.X + ColumnMargin + botSize.X),
                        (int) (nanobot.WorldPosition.Y + (botSize.Y - ItemRenderSize.Y) / 2),
                        ItemRenderSize.X,
                        ItemRenderSize.Y);
                    //_host.ScreenManager.SpriteBatch.Draw(nanobot.CarriedItem.Texture, rect, Color.White);
                    _host.ScreenManager.SpriteBatch.End();
                }
            }

            _host.ScreenManager.SpriteBatch.Begin();
            //_host.ScreenManager.SpriteBatch.DrawString(Fonts.Default, _infoText, new Vector2(_infoTextureRectangle.X, _infoTextureRectangle.Y), Color.White);
            _host.ScreenManager.SpriteBatch.Draw(_infoTexture, _infoTextureRectangle, Color.White);
            _host.ScreenManager.SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
