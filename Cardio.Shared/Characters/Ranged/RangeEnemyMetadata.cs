using Microsoft.Xna.Framework;

namespace Cardio.UI.Characters.Ranged
{
    public class RangeEnemyMetadata: ObstacleMetadata
    {
        public float AttackInterval { get; set; }

        public float AttackDamage { get; set; }

        public float BulletSpeed { get; set; }

        public float AttackRange { get; set; }

        public string BulletAssetName { get; set; }

        public string CircleScriptAssetName { get; set; }
    }

}
