using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Scripts
{
    public class TwitchingScript : GameEntityScript
    {
         private float _lastTime;

        private readonly float _width;
        private readonly float _height;

        public TwitchingScript(DrawableGameObject target, float width, float height)
        {
            Target = target;
            StartPosition = target.WorldPosition;
            _width = width;
            _height = height;

            SlowDownRatio = 24f;
            RatioX = .3f;
            RatioY = .9f;
        }

        public DrawableGameObject Target{ get; set;}

        public Vector2 StartPosition { get; set; }

        public float SlowDownRatio { get; set; }

        public float RatioX { get; set; }

        public float RatioY { get; set; }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);
            Target.WorldPosition = GetNewPosition(gameTime);
        }

        public Vector2 GetNewPosition(GameTime gameTime)
        {
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            var newTime = (_lastTime + elapsedTime) % 360;
            _lastTime = newTime;
            var radians = MathHelper.ToRadians(newTime);

            var resultPosition = new Vector2();
            resultPosition.X = StartPosition.X - (float)Math.Sin(radians) * _width * RatioX / SlowDownRatio;
            resultPosition.Y = StartPosition.Y - (float)Math.Cos(radians) * _height * RatioY / SlowDownRatio;

            return resultPosition;
        }
    }
}
