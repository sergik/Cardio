using Cardio.UI.Effects.Bloom;
using Cardio.UI.Extensions;

namespace Cardio.UI.Scripts
{
    public class HealPlayerScript: GameEntityScript
    {
        public float MinSaturation { get; set; }

        public float MaxSaturation { get; set; }

        public float MinIntensity { get; set; }

        public float MaxIntensity { get; set; }

        private float _currentFlareTime;
        public float FlareTime { get; set; }

        private BloomComponent _bloomComponent;

        public float HealBy { get; set; }

        public HealPlayerScript(float healBy)
        {
            MinSaturation = 0f;
            MaxSaturation = 3f;

            MaxIntensity = 0.5f;
            MaxIntensity = 1.0f;

            FlareTime = 350f;
            HealBy = healBy;

        }

        protected override void OnStart(Core.GameState gameState)
        {
            _bloomComponent = gameState.Game.Services.GetService<BloomComponent>();

            for (int index = 0; index < gameState.Player.Nanobots.Count; index++)
            {
                var bot = gameState.Player.Nanobots[index];
                bot.Heal(HealBy);
            }

            base.OnStart(gameState);
        }

        public override void Update(Core.GameState gameState, Microsoft.Xna.Framework.GameTime gameTime)
        {
            var coeff = (_currentFlareTime / FlareTime - 0.5f) * 2f;
            _bloomComponent.Settings.BloomSaturation = Ease(MinSaturation, MaxSaturation, coeff);
            _bloomComponent.Settings.BloomIntensity = Ease(MinIntensity, MaxIntensity, coeff);
            _bloomComponent.Settings.BlurAmount = Ease(2f, 10f, coeff);

            _currentFlareTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_currentFlareTime >= FlareTime)
            {
                Stop(gameState);
            }

            base.Update(gameState, gameTime);
        }

        private static float Ease(float min, float max, float timeCoeff)
        {
            var k = max - min;
            return max - k * timeCoeff * timeCoeff;
        }
    }
}
