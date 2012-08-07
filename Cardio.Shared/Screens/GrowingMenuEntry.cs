using System;
using Cardio.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.UI.Extensions;

namespace Cardio.UI.Screens
{
    public class GrowingMenuEntry : MenuEntry
    {
        private float _scaleDelta;

        private float _currentScale;

        private const float DefaultScale = 1;

        public float MaxScale { get; set; }

        public float GrowingTime { get; set; }

        public bool IsSelected
        {
            get; set;
        }

        public bool IsVisible { get; set; }

        public bool IsDisabled { get; set; }

        public GrowingMenuEntry(string text) : base(text)
        {
            MaxScale = 1.2f;
            GrowingTime = 120;
            IsVisible = true;
        }

        public override void Initialize(Game game)
        {
            _scaleDelta = (MaxScale - DefaultScale)/GrowingTime;
            base.Initialize(game);
        }

        public override void Update(MenuScreen screen, GameTime gameTime)
        {
            IsSelected = EntryArea.Contains(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y) &&
                         !IsDisabled;

            float direction = IsSelected ? 1 : -1;
            _currentScale += direction * _scaleDelta * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _currentScale = _currentScale > MaxScale
                                ? MaxScale
                                : _currentScale < DefaultScale ? DefaultScale : _currentScale;
        }

        public override void Draw(MenuScreen screen, GameTime gameTime)
        {
            if (IsVisible)
            {
                // Draw the selected entry in teal, otherwise white.
            Color color = IsSelected ? Color.Teal : Color.White;

                // Draw text, centered on the middle of each line.
                var spriteBatch = Game.Services.GetService<SpriteBatch>();
                spriteBatch.Begin();
                if ((Font != null) && !String.IsNullOrEmpty(Text))
                {
                    spriteBatch.DrawString(Font, Text, Position, color, 0, Vector2.Zero, _currentScale,
                                           SpriteEffects.None,
                                           0);
                }

                spriteBatch.End();
            }
        }
        
    }
}