using Microsoft.Xna.Framework.Content;

namespace Cardio.UI.Characters
{
    public class Glowing: CloseCombatEnemy
    {
        protected Glowing() {}

        public static Glowing FromMetadata(CloseCombatEnemyMetadata metadata, ContentManager contentManager)
        {
            var glowing = new Glowing();
            FillWithMetadata(glowing, metadata, contentManager);

            glowing.Content.AddAnimationRule("Default", () => glowing.IsAlive);
            glowing.Content.AddAnimationRule("Blow", () => !glowing.IsAlive);

            return glowing;
        }
    }
}
