using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Sounds;

namespace Cardio.Phone.Shared.Scripts
{
    public class UsePlayerShieldScript: GameEntityScript
    {
        public float ShieldActiveTime { get; set; }

        private float _currentTime;

        public UsePlayerShieldScript()
        {
            ShieldActiveTime = 8000;
        }

        public override void Update(Core.GameState state, Microsoft.Xna.Framework.GameTime time)
        {
            if (_currentTime >= ShieldActiveTime)
            {
                Stop(state);
            }

            _currentTime += (float) time.ElapsedGameTime.TotalMilliseconds;

            base.Update(state, time);
        }

        protected override void OnStart(Core.GameState state)
        {
            
            state.Player.Nanobot.ActivateShield();

            SoundManager.ShieldActivate.Play();

            base.OnStart(state);
        }

        protected override void OnStop(Core.GameState state)
        {
            SoundManager.ShieldDeactivate.Play();
            state.Player.Nanobot.DeactivateShields();

            base.OnStop(state);
        }
    }
}
