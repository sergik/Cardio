using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Thrombi;
using Cardio.UI.Scenes.Actions;
using Cardio.UI.Scenes.Scripts;
using Cardio.UI.Scripts;
using Cardio.UI.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Scenes
{
    public class ObstacleScene: Scene
    {
        public UI.Thrombi.Thrombus Thrombus { get; set; }

        private readonly TextRenderer _textRenderer;

        private PlayerShootScript _shootScript;

        public ObstacleScene()
        {
            _textRenderer = new TextRenderer();
        }

        public override void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            Thrombus.Initialize(game, spriteBatch, camera);
            Thrombus.WorldPosition = new Vector2(StartPosition + Thrombus.CollisionRectangle.Width / 2f + 40f, -60);
            

            _textRenderer.Initialize(spriteBatch);

            Reactions.Add(PlayerAction.Shoot, Shoot);

            base.Initialize(game, spriteBatch, camera);
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            Thrombus.Update(gameState, gameTime);

            IsFinished = Thrombus.Health <= 0;

            base.Update(gameTime, gameState);
        }

        public override void Draw(GameTime gameTime)
        {
            Thrombus.Draw(gameTime);
            
            base.Draw(gameTime);
        }

        private void Shoot(GameState state)
        {
            if (_shootScript == null || _shootScript.IsFinished)
            {
                _shootScript = new PlayerShootScript();
                _shootScript.AddTarget(Thrombus);
                //state.AddScript(_shootScript);
            }
        }
    }
}
