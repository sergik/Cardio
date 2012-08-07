using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Cardio.UI.Sounds
{
    public static class SoundManager
    {
        public static SoundEffect EnemyAttack01 { get; private set; }

        public static SoundEffect MenuClick { get; private set; }

        public static SoundEffect Pick { get; private set; }

        public static SoundEffect BotHit { get; private set; }

        public static SoundEffect EnemyBlow { get; private set; }

        public static SoundEffect ObstacleBreak { get; private set; }

        public static SoundEffectInstance BotAttack { get; private set; }

        public static SoundEffect ButtonHit { get; private set; }

        public static SoundEffect ShieldActivate { get; private set; }

        public static SoundEffect ShieldDeactivate { get; private set; }

        public static void Initialize(Game game)
        {
            var content = game.Content;

            EnemyAttack01 = content.Load<SoundEffect>("Sounds\\EnemyAttack01");
            MenuClick = content.Load<SoundEffect>(@"Sounds\MenuClick");
            Pick = content.Load<SoundEffect>(@"Sounds\Pick");
            BotHit = content.Load<SoundEffect>(@"Sounds\BotHit");
            EnemyBlow = content.Load<SoundEffect>(@"Sounds\EnemyBlow");
            ObstacleBreak = content.Load<SoundEffect>(@"Sounds\ObstacleBreak");

            BotAttack = content.Load<SoundEffect>(@"Sounds\BotAttack").CreateInstance();
            BotAttack.IsLooped = true;

            ButtonHit = content.Load<SoundEffect>(@"Sounds\ButtonHit");
            ShieldActivate = content.Load<SoundEffect>(@"Sounds\ShieldActivate");
            ShieldDeactivate = content.Load<SoundEffect>(@"Sounds\ShieldDeactivate");
        }

        public static void PauseAll()
        {
            if (BotAttack.State == SoundState.Playing)
            {
                BotAttack.Pause();
            }
        }

        public static void ResumeAll()
        {
            if (BotAttack.State == SoundState.Paused)
            {
                BotAttack.Resume();
            }
        }
    }
}
