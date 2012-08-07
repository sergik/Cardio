using System;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scenes.Scripts
{
    public abstract class GameScript: IGameScript
    {
        protected bool IsStarted { get; set; }

        public bool IsFinished
        {
            get;
            protected set;
        }

        public event EventHandler Stopped;

        public void InvokeStopped(EventArgs e)
        {
            var handler = Stopped;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void Stop(GameState state)
        {
            IsFinished = true;
            InvokeStopped(EventArgs.Empty);
        }

        public virtual void Start(GameState state)
        {
            IsFinished = false;
            IsStarted = true;
        }

        public virtual void Update(GameTime time, GameState state)
        {
            
        }

    }
}