using Cardio.UI.Core;

namespace Cardio.UI.Scripts
{
    public class Script
    {
        public bool IsStarted { get; protected set; }

        public virtual void Start(GameState gameState)
        {
            IsStarted = true;
            OnStart(gameState);
        }

        protected virtual void OnStart(GameState gameState)
        {
            // blank
        }
    }
}