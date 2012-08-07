using System;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Core.Alive
{
    public class DieEventArgs : EventArgs
    {
		public Vector2 DeathPoint { get; private set;}

        public DieEventArgs(Vector2 deathPoint)
        {
            DeathPoint = deathPoint;
        }
    }
}
