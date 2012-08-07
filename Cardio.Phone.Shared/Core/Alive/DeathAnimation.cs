using System;
using System.Linq;
using Cardio.Phone.Shared.Components;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Core.Alive
{
    public class DeathAnimation: DrawableGameObject
    {
        private float _holdTime;
        private float _currentTime;

        private bool _isFinished;

        public event EventHandler Finished;

        public void InvokeFinished(EventArgs e)
        {
            var handler = Finished;
            if (handler != null) handler(this, e);
        }

        public override void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            base.Initialize(game, spriteBatch, camera);

            _holdTime = Content.Animations.Max(x => x.Interval * x.FramesPerRow + x.LastFrameHoldOnTime);
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (_isFinished)
            {
                return;
            }

            if (_currentTime >= _holdTime)
            {
                InvokeFinished(EventArgs.Empty);
                _isFinished = true;
            }

            _currentTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            
            base.Update(gameState, gameTime);
        }
    }
}
