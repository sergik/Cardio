using Cardio.UI.Extensions;
using Cardio.UI.Rhythm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Heartbeat
{
    public class HeartbeatComponent: DrawableGameComponent
    {
        private Texture2D _heartTexture;

        private Color _tintColor;

        private SpriteBatch _spriteBatch;

        public Point Position { get; set; }

        public Point Size { get; set; }

        private Vector2 _origin;

        private RhythmEngine _rhythmEngine;

        public HeartbeatComponent(Game game) : base(game)
        {
            Position = new Point(36, 36);
            Size = new Point(48, 48);
        }

        public override void Initialize()
        {
            _spriteBatch = Game.Services.GetService<SpriteBatch>();
            _rhythmEngine = Game.Services.GetService<RhythmEngine>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _heartTexture = Game.Content.Load<Texture2D>(@"Textures\Heart02");
            _origin = new Vector2(_heartTexture.Width / 2f, _heartTexture.Height / 2f);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _rhythmEngine.Update(gameTime);

            var radius = _rhythmEngine.PatternGenerator.Radius;
            _tintColor = radius <= 140f
                ? Color.Red
                : Color.White;

        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_heartTexture, new Vector2(Position.X, Position.Y), null, _tintColor, 0f, _origin, 1f, SpriteEffects.None, 0f);
            _spriteBatch.End();
        }

        public void Pause()
        {
            _rhythmEngine.Pause();
        }

        public void Resume()
        {
            _rhythmEngine.Resume();
        }
    }
}
