using System;
using Cardio.Phone.Shared.Controls;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Text;
using Cardio.Phone.Shared.Twitter;
using Microsoft.Xna.Framework;
using Fonts = Cardio.Phone.Shared.Fonts;

namespace Cardio.Phone.Shared.Screens
{
    public class TwitterAuthenticationScreen : MenuScreen
    {
        private TextBox _pinTextBox;

        private TextRenderer _authGuide;

        private TextRenderer _tweetedGuide;

        public bool Tweeted { get; private set; }

        private ITwitter _twitter;

        private GrowingMenuEntry _fromClipboard;

        public event EventHandler TweenSent;

        public string TextToSendAsTweet { get; set; }

        public override void LoadContent()
        {
            InitializeButtons();
            InitializeGuides();

            _pinTextBox = new TextBox(ScreenManager.Game)
                              {
                                  Posistion = new Vector2(180, 270),
                                  TextHorizontalMargin = 50,
                                  Width = 350,
                                  Height = 70,
                                  MaxLength = 10,
                                  TextColor = Color.RoyalBlue
                              };

            _pinTextBox.Initialize();

            _twitter = ScreenManager.Game.Services.GetService<ITwitter>();

            base.LoadContent();
        }

        private void InitializeGuides()
        {
            _authGuide = new TextRenderer
                             {
                                 Color = Color.LightSkyBlue,
                                 Font = Fonts.Default,
                                 Position = new Vector2(60, 90),
                                 Text =
                                     "Before Cardio could post on Twitter you need to go through a couple of simple steps:\r\n\r\n" +
                                     "At first, navigate to your browser that will open shortly\r\n" + 
                                     "Then please pass authentication on Twitter and enter received PIN in the text box below.\r\n" + 
                                     "Press 'SUBMIT PIN AND POST TWEET' and wait for a couple of seconds..."
                             };

            _authGuide.Initialize(ScreenManager.SpriteBatch);

            _tweetedGuide = new TextRenderer
                                {
                                    Color = Color.LightSkyBlue,
                                    Font = Fonts.Default,
                                    Position = new Vector2(500, ScreenManager.Game.GraphicsDevice.Viewport.Height - 40),
                                    Text =
                                        "That's it, just press 'BACK' and enjoy the game!"
                                };
            _tweetedGuide.Initialize(ScreenManager.SpriteBatch);
        }

        private void InitializeButtons()
        {
            //_fromClipboard = new GrowingMenuEntry("PASTE FROM CLIPBOARD") { Position = new Vector2(550, 290) };
            //_fromClipboard.Selected += (o, e) =>
            //                           {
            //                               _pinTextBox.Text = Clipboard.GetText();
            //                           };
            //MenuEntries.Add(_fromClipboard);

            var submitPin = new GrowingMenuEntry("SUBMIT PIN AND POST TWEET") { Position = new Vector2(180, 420) };
            submitPin.Selected += (o, e) =>
                                      {
                                          if (!string.IsNullOrEmpty(_pinTextBox.Text))
                                          {
                                              if (_twitter.IsInitialized &&
                                                  _twitter.AuthenticateWith(_pinTextBox.Text) &&
                                                  _twitter.SendTweet(TextToSendAsTweet))
                                              {
                                                  Tweeted = true;
                                                  TweenSent.Fire(this, () => EventArgs.Empty);
                                              }
                                          }
                                      };
            MenuEntries.Add(submitPin);

            var back = new GrowingMenuEntry("BACK") { Position = new Vector2(40, ScreenManager.Game.GraphicsDevice.Viewport.Height - 40) };
            back.Selected += (o, e) => ExitScreen();
            MenuEntries.Add(back);
        }

        public override void  Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _pinTextBox.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            _pinTextBox.Draw(gameTime);
            _authGuide.Draw(gameTime);

            if (Tweeted)
            {
                _tweetedGuide.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

    }
}