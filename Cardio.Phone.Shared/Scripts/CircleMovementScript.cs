using System;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Characters;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Scripts
{
    public class CircleMovementScript: GameEntityScript
    {
        private static Random _random = new Random();

        private readonly float _radius;
        private float _angle;
        private readonly float _speed;
        private float _delay;
        
        public DrawableGameObject Target
        {
            get; set;
        }

        public Vector2 InitialPosition
        {
            get; set;
        }


        public CircleMovementScript(float radius, float speed)
        {
            _radius = radius;
            _speed = speed;
            _delay = _random.Next((int)speed);
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            if (_delay >= 0)
            {
                _delay -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                return;
            }

            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _angle += elapsedTime / _speed;
            if (_angle > MathHelper.TwoPi)
            {
                _angle -= MathHelper.TwoPi;
            }

            var dx = (float)(_radius * Math.Cos(_angle));
            var dy = (float)(_radius * Math.Sin(_angle));

            Target.WorldPosition = new Vector2(InitialPosition.X + dx, InitialPosition.Y + dy);
        }

        protected override void OnStart(GameState gameState)
        {
            base.OnStart(gameState);
            InitialPosition = Target.WorldPosition;
        }

        public static CircleMovementScript FromMetadata(CircleScriptMetadata metadata)
        {
            return new CircleMovementScript(metadata.Radius, metadata.Speed);
        }
    }
}
