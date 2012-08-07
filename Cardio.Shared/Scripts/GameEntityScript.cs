using System;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class GameEntityScript: Script
    {
        public bool IsFinished { get; protected set; }

        public event EventHandler Finished;

        public void InvokeFinished(EventArgs e)
        {
            EventHandler handler = Finished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override sealed void Start(GameState gameState)
        {
            IsStarted = true;
            IsFinished = false;
            OnStart(gameState);
        }

        public void Stop(GameState gameState)
        {
            IsFinished = true;
            OnStop(gameState);
            InvokeFinished(EventArgs.Empty);
        }

        protected virtual void OnStop(GameState gameState)
        {
            // blank
        }

        public virtual void Update(GameState gameState, GameTime gameTime)
        {
            
        }
    }
}
