using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cardio.UI.Controls
{
    public class TextBox : Control
    {
        #region Fields

        private TimeSpan _lastPressedTime;

        private float _textScale;

        private Texture2D _unfocusedBackground;

        private Texture2D _focusedBackground;

        private Vector2 _textPosition;

        private readonly StringBuilder _textBuilder;

        #endregion

        #region Properties

        public string Text
        {
            get
            {
                return _textBuilder.ToString();
            }
            set
            {
                _textBuilder.Clear();
                _textBuilder.Append(value);
            }
        }

        public bool IsAtFocus { get; private set; }

        public TimeSpan PressureTimeInverval { get; set; }

        public string FocusedTextureName { get; private set; }

        public string UnfocusedTextureName { get; private set; }

        public int TextHorizontalMargin { get; set; }

        public int TextVerticalMargin { get; set; }

        public SpriteFont Font { get; set; }

        public Color TextColor { get; set; }

        public int MaxLength { get; set; }

        protected Dictionary<Keys, Action> SpecificActions { get; private set; }

        protected Dictionary<Keys, string> KeyMappings { get; private set; }

        #endregion

        public TextBox(Game game)
            : this(game, @"Textures\UI\TextboxActive", @"Textures\UI\Textbox")
        {
        }

        public TextBox(Game game, string focusedTexture, string unfocusedTexture) : base(game)
        {
            FocusedTextureName = focusedTexture;
            UnfocusedTextureName = unfocusedTexture;
            TextVerticalMargin = 15;
            TextHorizontalMargin = 30;
            TextColor = Color.CadetBlue;
            PressureTimeInverval = TimeSpan.FromMilliseconds(200);
            SpecificActions = new Dictionary<Keys, Action>();
            KeyMappings = new Dictionary<Keys, string>();
            _textBuilder = new StringBuilder();
        }

        public override void Initialize()
        {
            _focusedBackground = Game.Content.Load<Texture2D>(@"Textures\UI\TextboxActive");
            _unfocusedBackground = Game.Content.Load<Texture2D>(@"Textures\UI\Textbox");
            
            if (Font == null)
            {
                Font = Fonts.Default;
            }
            Vector2 upperCaseTextSize = Font.MeasureString("F");
            _textScale = (Height - 2*TextVerticalMargin)/upperCaseTextSize.Y;
            _textPosition = new Vector2(Posistion.X + TextHorizontalMargin, Posistion.Y + Height - TextVerticalMargin - upperCaseTextSize.Y * _textScale);

            RegisterSpecificActions();
            RegisterMappings();

            base.Initialize();
        }

        protected virtual void RegisterSpecificActions()
        {
            SpecificActions.Add(Keys.Back, () =>
                                               {
                                                   if (_textBuilder.Length > 0)
                                                   {
                                                       _textBuilder.Remove(_textBuilder.Length - 1, 1);
                                                   }
                                               });
        }

        protected virtual void RegisterMappings()
        {
            KeyMappings.Add(Keys.D0, "0");
            KeyMappings.Add(Keys.D1, "1");
            KeyMappings.Add(Keys.D2, "2");
            KeyMappings.Add(Keys.D3, "3");
            KeyMappings.Add(Keys.D4, "4");
            KeyMappings.Add(Keys.D5, "5");
            KeyMappings.Add(Keys.D6, "6");
            KeyMappings.Add(Keys.D7, "7");
            KeyMappings.Add(Keys.D8, "8");
            KeyMappings.Add(Keys.D9, "9");

            KeyMappings.Add(Keys.NumPad0, "0");
            KeyMappings.Add(Keys.NumPad1, "1");
            KeyMappings.Add(Keys.NumPad2, "2");
            KeyMappings.Add(Keys.NumPad3, "3");
            KeyMappings.Add(Keys.NumPad4, "4");
            KeyMappings.Add(Keys.NumPad5, "5");
            KeyMappings.Add(Keys.NumPad6, "6");
            KeyMappings.Add(Keys.NumPad7, "7");
            KeyMappings.Add(Keys.NumPad8, "8");
            KeyMappings.Add(Keys.NumPad9, "9");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsMouseButtonTriggered(x => x.LeftButton) || InputManager.IsMouseButtonTriggered(x => x.RightButton))
            {
                IsAtFocus= InputManager.IsButtonClicked(new Rectangle((int) Posistion.X, (int) Posistion.Y, Width, Height));
            }
            if (IsAtFocus)
            {
                 var keys = Keyboard.GetState().GetPressedKeys();
                if ( keys.Length > 0 &&(gameTime.TotalGameTime - _lastPressedTime) >= PressureTimeInverval)
                {
                    var specificKeys = keys.Where(key => SpecificActions.ContainsKey(key));
                    var usualKeys = keys.Except(specificKeys);

                    foreach (var key in specificKeys)
                    {
                        SpecificActions[key]();
                    }

                    foreach (var key in keys)
                    {
                        if (KeyMappings.ContainsKey(key) && Text.Length < MaxLength)
                        {
                            _textBuilder.Append(KeyMappings[key]);
                        }
                    }

                    _lastPressedTime = gameTime.TotalGameTime;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            var texture = IsAtFocus ? _focusedBackground : _unfocusedBackground;
            SpriteBatch.Draw(texture, new Rectangle((int)Posistion.X, (int) Posistion.Y, Width, Height), Color.White);

            SpriteBatch.DrawString(Font, _textBuilder, _textPosition, TextColor, 0, Vector2.Zero, _textScale, SpriteEffects.None, 0);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}