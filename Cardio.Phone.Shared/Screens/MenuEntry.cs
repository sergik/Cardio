using System;
using Cardio.Phone.Shared;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Screens
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public class MenuEntry
    {
        protected Game Game
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the font used to draw this menu entry.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Gets or sets the position of this menu entry.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// A description of the function of the button.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional texture drawn with the text.
        /// </summary>
        /// <remarks>If present, the text will be centered on the texture.</remarks>
        public Texture2D Texture { get; set; }

        public Rectangle EntryArea { get; set; }

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        public virtual void OnSelectEntry()
        {
            var handler = Selected;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            SoundManager.MenuClick.Play();
        }

        public MenuEntry()
        {
        }

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public MenuEntry(string text)
        {
            Text = text;
        }

        public virtual void Initialize(Game game)
        {
            Font = Fonts.Default;

            var textSize = Font.MeasureString(Text);
            EntryArea = Texture != null
                            ? new Rectangle((int) Position.X, (int) Position.Y - 5,
                                            (int) Math.Max(textSize.X, Texture.Width),
                                            (int) Math.Max(textSize.Y, Texture.Height) + 5)
                            : new Rectangle((int) Position.X, (int) Position.Y, (int) textSize.X, (int) textSize.Y);
            Game = game;

        }

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(MenuScreen screen, GameTime gameTime)
        { }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, GameTime gameTime)
        {
            // Draw the selected entry in teal, otherwise white.
            //Color color = isSelected ? Color.Teal : Color.White;
            var color = Color.White;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;

            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
                    Vector2.Zero, 1f, SpriteEffects.None, 0f);
                if ((Font != null) && !String.IsNullOrEmpty(Text))
                {
                    Vector2 textSize = Font.MeasureString(Text);
                    Vector2 textPosition = Position + new Vector2(
                        (float)Math.Floor((Texture.Width - textSize.X) / 2),
                        (float)Math.Floor((Texture.Height - textSize.Y) / 2));
                    spriteBatch.DrawString(Font, Text, textPosition, color);
                }
            }
            else if ((Font != null) && !String.IsNullOrEmpty(Text))
            {
                spriteBatch.DrawString(Font, Text, Position, color);
            }
        }

        public virtual bool IsClicked(MenuScreen host)
        {
            Point size;
            if (Texture != null)
            {
                size = new Point(Texture.Width, Texture.Height);
            }
            else
            {
                var measuredSize = Font.MeasureString(Text);
                size = new Point((int)measuredSize.X, (int)measuredSize.Y);
            }

            // the hit bounds are the entire width of the screen, and the height of the entry
            // with some additional padding above and below.
            var rect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                size.X + 2,
                size.Y + 2);

            return InputManager.IsButtonClicked(rect);
        }
    }
}