using Cardio.UI.Actions;
using Cardio.UI.Core;
using Cardio.UI.Extensions;
using Cardio.UI.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.GUI
{
    public class InputSequenceDisplay: DrawableGameComponent
    {
        public Point SequenceDisplayCenter { get; set; }

        public float LetterVerticalMargin { get; set; }

        private ITouchService _touchService;
        
        private Texture2D _leftButtonTexture;
        private Texture2D _rightButtonTexture;

        private ProgressBar _reactionBar;

        private GameState _gameState;

        private const int LetterCount = 4;

        private float _oneTouchWidth;

        private SpriteBatch _spriteBatch;

        public InputSequenceDisplay(Game game) : base(game)
        {
            // some default values
            SequenceDisplayCenter = new Point(Game.GraphicsDevice.Viewport.Width/2,
                                                Game.GraphicsDevice.Viewport.Height - 40);
            LetterVerticalMargin = 5;
        }

        public override void Initialize()
        {
            _touchService = Game.Services.GetService<ITouchService>();
            _spriteBatch = Game.Services.GetService<SpriteBatch>();
            _gameState = Game.Services.GetService<GameState>();

            _reactionBar = new ProgressBar(Game, @"Textures\UI\PressSequenceBarFiller", @"Textures\UI\PressSequenceBarEmpty")
            {
                Max = ReactionProgressComponent.InitialProgressValue
            };
            _reactionBar.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _leftButtonTexture = Game.Content.Load<Texture2D>("Textures\\UI\\A");
            _rightButtonTexture = Game.Content.Load<Texture2D>("Textures\\UI\\S");

            _reactionBar.DestinationRectangle = new Rectangle(SequenceDisplayCenter.X - _reactionBar.BackTexture.Width / 2,
                                                              SequenceDisplayCenter.Y - _reactionBar.BackTexture.Height / 2,
                                                              _reactionBar.BackTexture.Width,
                                                              _reactionBar.BackTexture.Height);

            _oneTouchWidth = _reactionBar.DestinationRectangle.Width / (float) LetterCount;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameState.ReactionProgress.IsReactionInProgress)
            {
                _reactionBar.Value = _gameState.ReactionProgress.RemainingReactionProgress;
            }
            else
            {
                _reactionBar.Value = (_reactionBar.Max - _reactionBar.Min)/4*_touchService.Touches.Count;
            }
            _reactionBar.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
             _reactionBar.Draw(gameTime);

             if (!_gameState.ReactionProgress.IsReactionInProgress)
             {

                 _spriteBatch.Begin();
                 var seq = _touchService.Touches;
                 for (int i = 0; i < seq.Count; i++)
                 {
                     var texture = seq[i].Button == ButtonType.Left ? _leftButtonTexture : _rightButtonTexture;

                     var scale = (_reactionBar.BackTexture.Height - 2*LetterVerticalMargin)/texture.Height/2;

                     var rect = new Rectangle(0, 0, (int) (texture.Width*scale), (int) (texture.Height*scale));

                     rect.Offset(_reactionBar.DestinationRectangle.X + (int) ((i + 0.5)*_oneTouchWidth - rect.Width/2f),
                                 _reactionBar.DestinationRectangle.Y +
                                 (int) (_reactionBar.DestinationRectangle.Height/2f - rect.Height/2f));

                     _spriteBatch.Draw(texture, rect, Color.White);
                 }

                 _spriteBatch.End();
             }

            base.Draw(gameTime);
        }
    }
}
