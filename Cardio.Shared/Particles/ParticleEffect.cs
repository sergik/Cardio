using System;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Particles
{
    public abstract class ParticleEffect : DrawableGameObject
    {
        public event EventHandler Finished;

        public void InvokeFinished(EventArgs e)
        {
            var handler = Finished;
            if (handler != null) handler(this, e);
        }
    }
}
