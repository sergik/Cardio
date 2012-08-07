using Microsoft.Xna.Framework;

namespace Cardio.UI
{
    /// <summary>
    /// Simple class to calculate the frame-per-seconds rate of your game.
    /// </summary>
    public sealed class Framerate : DrawableGameComponent
    {
        private double _currentFramerate;
        private float _deltaFpsTime;
        private string _displayFormat;
        private bool _showDecimals;

        #region Instance Properties

        /// <summary>
        /// Gets the current framerate.
        /// </summary>
        /// <remarks>
        /// The 'Enabled' property must have been set to true to retrieve values greater than zero.
        /// </remarks>
        public double Current
        {
            get { return _currentFramerate; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the time step must be fixed or not.
        /// </summary>
        /// <remarks>
        /// If set to true, the game will target the desired constant framerate set in your main class ('Game1', by default).
        /// </remarks>
        public bool IsFixedTimeStep
        {
            get { return Game.IsFixedTimeStep; }
            set
            {
                if (Game != null)
                    Game.IsFixedTimeStep = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the framerate will display decimals on screen or not.
        /// </summary>
        public bool ShowDecimals
        {
            get { return _showDecimals; }
            set { _showDecimals = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the decimal part of the framerate value must be display as fixed format (or as double format, otherwise).
        /// </summary>
        /// <remarks>
        /// The 'ShowDecimals' property must be set to true in order to set the proper format.
        /// </remarks>
        public bool FixedFormatDisplay
        {
            get { return _displayFormat == "F"; }
            set { _displayFormat = value ? "F" : "R"; }
        }

        #endregion

        /// <summary>
        /// Parameterless constructor for this class.
        /// </summary>
        public Framerate(Game game) : base(game)
        {}

        public override void Initialize()
        {
            _currentFramerate = 0;

            base.Initialize();
        }

        /// <summary>
        /// Called when the gamecomponent needs to be updated.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // The time since Update() method was last called.
            var elapsed = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            // Ads the elapsed time to the cumulative delta time.
            _deltaFpsTime += elapsed;

            // If delta time is greater than a second: (a) the framerate is calculated, (b) it is marked to be drawn, and (c) the delta time is adjusted, accordingly.
            if (_deltaFpsTime > 1000)
            {
                _currentFramerate = 1000 / elapsed;
                _deltaFpsTime -= 1000;
            }
        }

        /// <summary>
        /// Called when the gamecomponent needs to be drawn.
        /// </summary>
        /// <remarks>
        /// Currently, the framerate is shown in the window's title of the game.
        /// </remarks>
        public override void Draw(GameTime gameTime)
        {
            // If the framerate can be drawn, it is shown in the window's title of the game.

            string currentFramerateString = _showDecimals
                ? _currentFramerate.ToString(_displayFormat)
                : ((int) _currentFramerate).ToString("D");

            Game.Window.Title = "FPS: " + currentFramerateString;
        }
    }
}