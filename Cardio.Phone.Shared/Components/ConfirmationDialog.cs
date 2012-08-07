using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Components
{
    public class ConfirmationDialog<T>: DrawableGameComponent
    {
        public T Item { get; set; }

        public event Func<T, String> NeedDisplayText;

        public event EventHandler Confirm;

        public Texture2D Texture { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public Rectangle DrawRectangle { get; set; }

        public Color DrawColor { get; set; }

        public GrowingMenuEntry CancelButton { get; private set; }
        public GrowingMenuEntry OkButton { get; private set; }


        public ConfirmationDialog(Game game)
            : base(game)
        {
            var width = Game.GraphicsDevice.Viewport.Width;
            var height = Game.GraphicsDevice.Viewport.Height;
            DrawRectangle = new Rectangle(0, 0, width, height);
            Texture = new Texture2D(game.GraphicsDevice, 1, 1);
            Texture.SetData(new[] {new Color(0f, 0f, 0f, 1f)});

            DrawColor = new Color(0, 0, 0, 120);
            OkButton = new GrowingMenuEntry("BUY");
            CancelButton = new GrowingMenuEntry("CANCEL");
            OkButton.Position = new Vector2(width - 550, height - 60);
            CancelButton.Position = new Vector2(width - 300, height - 60);
        }

        public override void Initialize()
        {
            base.Initialize();
            SpriteBatch = Game.Services.GetService<SpriteBatch>();
            OkButton.Initialize(Game);
            CancelButton.Initialize(Game);
        }

        protected virtual string OnNeedDisplayText(T item)
        {
            var handler = NeedDisplayText;
            if(handler != null)
            {
                return handler(item);
            }
            return String.Empty;
        }

        protected virtual void OnConfirm(object sender, EventArgs e)
        {
            var handler = Confirm;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            OkButton.Update(null, gameTime);
            CancelButton.Update(null, gameTime);
            if(OkButton.IsSelected)
            {
                OkButton.OnSelectEntry();
            }
            if(CancelButton.IsSelected)
            {
                CancelButton.OnSelectEntry();
            }
        }        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, DrawRectangle, DrawColor);
            String text = String.Format("Are you sure you want to buy\r\n{0}?", OnNeedDisplayText(Item));
            SpriteBatch.DrawString(Fonts.Tutorial, text, new Vector2(DrawRectangle.X + 120, DrawRectangle.Y + 180),
                                    Color.White);
            SpriteBatch.End();
            CancelButton.Draw(null, gameTime);
            OkButton.Draw(null, gameTime);
        }
    }
}
