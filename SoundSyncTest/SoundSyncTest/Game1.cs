using Cardio.UI;
using Cardio.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoundSyncTest.Rhythm;

namespace SoundSyncTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly RhythmEngine _rhythmEngine;
        private readonly RhythmMelodyPlayer _melodyPlayer;

        private SoundEffect _melody;

        private float _comboLevel = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _rhythmEngine = new RhythmEngine(this);
            Components.Add(_rhythmEngine);

            _melodyPlayer = new RhythmMelodyPlayer(_rhythmEngine);

        }

        protected override void Initialize()
        {
            InputManager.Initialize();
            Fonts.Initialize(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _melody = Content.Load<SoundEffect>(@"Sounds\\LongMelody");
            _melodyPlayer.ChangeMelody(_melody, long.MaxValue);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }


            _rhythmEngine.Update(gameTime);

            _melodyPlayer.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
