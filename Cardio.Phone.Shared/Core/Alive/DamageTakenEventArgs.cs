using System;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core.Alive
{
    public class DamageTakenEventArgs: EventArgs
    {
        public float Damage { get; private set;}

		public Vector2 DamageTakenPoint { get; private set;}

        public DamageTakenEventArgs(float damage)
        {
            Damage = damage;
        }

		public DamageTakenEventArgs(float damage, Vector2 damageTakenPoint) : this (damage)
		{
			DamageTakenPoint = damageTakenPoint;
		}
    }
}