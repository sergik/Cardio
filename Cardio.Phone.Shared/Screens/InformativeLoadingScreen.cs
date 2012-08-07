using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Screens;
using Cardio.Phone.Shared;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Extensions;
using Cardio.Phone.Shared.Twitter;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Screens
{
    public class InformativeLoadingScreen: LoadingScreen
    {
        public static readonly IList<string> Information = new List<string>
        {
            "The most 'bloody' medical operation was performed in 1970.\r\nAiling hemophiliac pacient required 1080 liters of blood.\r\nIt equals to 15 fulfilled baths.",
            "Total weight of all microbes and parasites in the body\r\nof an average human is greater than 1 kg.\r\nSome of them are vital.",
            "Average female heart weights 220g,\r\nmale one - 280. Women are affected by heart-related\r\ndiseases generally in 10 years later than men are.",
            "Breath holding for 30-40 seconds after inhaling\r\nin the morning can help keep your heart beat smooth.",
            "Risk of heart attack or insult is reduced by 50%\r\na year after smoking cessation",
            "80% of harmful microbes\r\nget into our body through the hands.\r\nSo, very useful kind of prophylaxis\r\nof colds and flu is washing the hands."
        };

        private static Random _random = new Random();

        private String _infoText;
        private Vector2 _infoPosition;

        private bool _userClickedProceedButton;

        private GrowingMenuEntry _proceedButton;
        private bool _proceedButtonSelected;

        protected InformativeLoadingScreen(bool loadingIsSlow, GameScreen[] screensToLoad): base(loadingIsSlow, screensToLoad)
        {
            // blank
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public new static void Load(ScreenManager screenManager, bool loadingIsSlow,
            params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            var loadingScreen = new InformativeLoadingScreen(loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen);
        }

        public override void LoadContent()
        {
            _infoText = Information[_random.Next(Information.Count)];

            var viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            _infoPosition = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

            _proceedButton = new GrowingMenuEntry("PLAY")
            {Position = new Vector2(viewport.Width - 150, viewport.Height - 60)};
            _proceedButton.Initialize(ScreenManager.Game);

            //_tweetButton = new GrowingMenuEntry("POST TO TWITTER")
            //                   {
            //                       Position = new Vector2(viewport.Width - 550, viewport.Height - 60),
            //                       MaxScale = 1.1f
            //                   };
            //_tweetButton.Initialize(ScreenManager.Game);

            var twitter = ScreenManager.Game.Services.GetService<ITwitter>();
            //_tweetButton.Selected += (o, e) =>
            //                             {
            //                                 if (!twitter.IsInitialized)
            //                                 {
            //                                     return;
            //                                 }
            //                                 if (twitter.AuthenticationRequired)
            //                                 {
            //                                     GoToTwitterScreen();
            //                                 }
            //                                 else
            //                                 {
            //                                     _tweeted = twitter.SendTweet(_infoText);
            //                                 }
            //                             };

            //_reenterPin = new GrowingMenuEntry("REENTER PIN AND POST TO TWITTER")
            //{
            //    Position = new Vector2(50, viewport.Height - 60),
            //    MaxScale = 1.1F
            //};
            //_reenterPin.Initialize(ScreenManager.Game);
            //_reenterPin.Selected += (o, e) =>
            //                            {
            //                                if (twitter.IsInitialized)
            //                                {
            //                                    GoToTwitterScreen();
            //                                }
            //                            };

            base.LoadContent();
        }

        //private void GoToTwitterScreen()
        //{
        //    var screen = new TwitterAuthenticationScreen
        //                     {TextToSendAsTweet = _infoText};
        //    screen.TweenSent += (sender, args) => _tweeted = true;
        //    ScreenManager.AddScreen(screen);
        //}

        protected override bool CanShowLoadedScreen()
        {
            return _userClickedProceedButton;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _proceedButtonSelected = _proceedButton.EntryArea.Contains(InputManager.CurrentMouseState.X,
                InputManager.CurrentMouseState.Y);
            _proceedButton.Update(null, gameTime);
            //_tweetButton.Update(null,gameTime);
            //_reenterPin.Update(null, gameTime);
            
            if (InputManager.IsButtonClicked(_proceedButton.EntryArea))
            {
                _userClickedProceedButton = true;
            }

            //if (InputManager.IsButtonClicked(_tweetButton.EntryArea))
            //{
            //    _tweetButton.OnSelectEntry();
            //}

            //if (InputManager.IsButtonClicked(_reenterPin.EntryArea))
            //{
            //    _reenterPin.OnSelectEntry();
            //}

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        protected override void DrawWaiting(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            Text.Fonts.DrawCenteredText(ScreenManager.SpriteBatch, Fonts.Default, _infoText, _infoPosition, 1f, Color.White);
            //if (_tweeted)
            //{
            //    ScreenManager.SpriteBatch.DrawString(Fonts.Default, "POSTED", _tweetButton.Position, Color.White);
            //}
            ScreenManager.SpriteBatch.End();

            //if (!_tweeted)
            //{
            //    _tweetButton.Draw(null, gameTime);
            //    _reenterPin.Draw(null, gameTime);
            //}

            if (OtherScreensAreGone)
            {
                _proceedButton.Draw(null, gameTime);
            }
        }
    }
}