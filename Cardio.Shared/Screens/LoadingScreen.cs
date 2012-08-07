using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Screens
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    public class LoadingScreen: GameScreen
    {
        private readonly bool _loadingIsSlow;
        protected bool OtherScreensAreGone { get; private set; }

        private readonly GameScreen[] _screensToLoad;

        private Texture2D _loadingTexture;
        private Vector2 _loadingPosition;

        public static event EventHandler<ScreenLoadedEventArgs> ScreenLoaded;

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        protected LoadingScreen(bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            _loadingIsSlow = loadingIsSlow;
            _screensToLoad = screensToLoad;
            
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
            params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            var loadingScreen = new LoadingScreen(loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen);
            InvokeScreenLoaded(new ScreenLoadedEventArgs(screensToLoad[0]));
        }

        public override void LoadContent()
        {
            var content = ScreenManager.Game.Content;
            _loadingTexture = content.Load<Texture2D>(@"Textures\Screens\Loading");
            content.Load<Texture2D>(@"Textures\Screens\FadeScreen");
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            _loadingPosition = new Vector2(
                (float) ((viewport.X + Math.Floor((double) viewport.Width - _loadingTexture.Width)) / 2f),
                (float) ((viewport.Y + Math.Floor((double) viewport.Height - _loadingTexture.Height)) / 2f));

            base.LoadContent();
        }

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (OtherScreensAreGone && CanShowLoadedScreen())
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in _screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        protected virtual bool CanShowLoadedScreen()
        {
            return true;
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public sealed override void Draw(GameTime gameTime)
        {
            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (_loadingIsSlow)
            {
                DrawWaiting(gameTime);
            }

            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if (ScreenState == ScreenState.Active && (ScreenManager.GetScreens().Length == 1))
            {
                OtherScreensAreGone = true;
            }

        }

        protected virtual void DrawWaiting(GameTime gameTime)
        {
            ScreenManager.Game.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(_loadingTexture, _loadingPosition, null, Color.White, 0f,
                Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        private static void InvokeScreenLoaded(ScreenLoadedEventArgs e)
        {
            var handler = ScreenLoaded;
            if (handler != null)
            {
                handler(null, e);
            }
        }
    }
}