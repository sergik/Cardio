using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Extensions;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Particles
{
    public abstract class ParticleEffect : DrawableGameObject
    {
        public event EventHandler Finished;

        public void InvokeFinished(EventArgs e)
        {
            var handler = Finished;
            if (handler != null) handler(this, e);
        }

        public abstract void Unload(Game game);
    }
}
