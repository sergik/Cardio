using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Tutorial
{
    public class TutorialScriptActivationScript: GameEntityScript
    {
        public GameEntityScript Script { get; set; }

        public float StartPosition { get; set; }

        public override void Update(Core.GameState gameState, GameTime gameTime)
        {
            if (!IsFinished && gameState.Player.WorldPosition.X >= StartPosition)
            {
                gameState.AddScript(Script);
                Stop(gameState);
            }

            base.Update(gameState, gameTime);
        }
    }
}
