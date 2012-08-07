using System;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core.Alive
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
