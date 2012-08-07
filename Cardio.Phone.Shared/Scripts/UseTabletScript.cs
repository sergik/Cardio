using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Effects;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Scripts
{
    public class UseTabletScript : GameEntityScript
    {
        private const float MinIntensity = 0f;

        private const float MaxIntensity = 0.1f;

        private float _currentFlareTime;
        public float FlareTime { get; set; }

        private readonly UseEffect _useEfffect;

        public float Damage { get; set; }

        public UseTabletScript(float damage)
        {
            FlareTime = 350f;
            Damage = damage;
            _useEfffect = new UseEffect
                              {
                                  Color = new Color(9, 9, 74, (int)(MaxIntensity * 255))
                              };

        }

        protected override void OnStart(Core.GameState gameState)
        {
           
            SoundManager.Tablet.Play();
            gameState.Player.Effects.Add(_useEfffect);
            var playerPosition = gameState.Player.WorldPosition;
            var damageDistance = gameState.Camera.ViewportWidth;
            _useEfffect.Initialize(gameState.Game, gameState.Game.Services.GetService<SpriteBatch>(), gameState.Camera);
            var enemies = gameState.Level.Enemies.Where(e => (e.WorldPosition - playerPosition).X < damageDistance);
            foreach (var enemy in enemies)
            {
                enemy.Damage(new DamageTakenEventArgs(Damage));
            }
            base.OnStart(gameState);
        }

        public override void Update(Core.GameState gameState, GameTime gameTime)
        {
            var alfa = (_currentFlareTime / FlareTime) * (MaxIntensity - MinIntensity) + MinIntensity;
            var prevColor = _useEfffect.Color;
            _useEfffect.Color = new Color(prevColor.R, prevColor.G, prevColor.B, (byte)(255*alfa));

            _currentFlareTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_currentFlareTime >= FlareTime)
            {
                Stop(gameState);
            }

            base.Update(gameState, gameTime);
        }

        protected override void OnStop(Core.GameState gameState)
        {
            base.OnStop(gameState);
            gameState.Player.Effects.Remove(_useEfffect);
        }
    }
}
