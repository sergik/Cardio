using System;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Particles;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Particles
{
    public class Particle: DrawableGameObject
    {
        public float Lifetime { get; set; }

        public bool IsAlive { get; private set; }

        public IParticleBehavior Behavior { get; set; }

        private float _livedFor;

        public event EventHandler Died;

        public Particle()
        {
            Lifetime = 400;
            IsAlive = true; 
        }

        public void InvokeDied()
        {
            var handler = Died;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (_livedFor >= Lifetime)
            {
                IsAlive = false;
                InvokeDied();
            }

            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            _livedFor += elapsedTime;

            if (Behavior != null)
            {
                WorldPosition = Behavior.GetNewPosition(WorldPosition, elapsedTime);
            }

            base.Update(gameState, gameTime);
        }
    }
}
