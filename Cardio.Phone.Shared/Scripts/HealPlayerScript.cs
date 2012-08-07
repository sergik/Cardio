using Cardio.Phone.Shared.Effects;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Scripts
{
    public class HealPlayerScript: GameEntityScript
    {
        private const float MinIntensity = 0f;

        private const float MaxIntensity = 0.3f;

        private float _currentFlareTime;
        public float FlareTime { get; set; }

        private UseEffect _useEfffect;

        public float HealBy { get; set; }

        public HealPlayerScript(float healBy)
        {
            FlareTime = 350f;
            HealBy = healBy;
            _useEfffect = new UseEffect
                              {
                                  Color = new Color(74, 9, 9, (int)(MaxIntensity * 255))
                              };

        }

        protected override void OnStart(Core.GameState gameState)
        {
            var bot = gameState.Player.Nanobot;
            bot.Heal(HealBy);
            SoundManager.Heal.Play();
            gameState.Player.Effects.Add(_useEfffect);
            _useEfffect.Initialize(gameState.Game, gameState.Game.Services.GetService<SpriteBatch>(), gameState.Camera);

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
