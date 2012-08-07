using System;
using System.Collections.Generic;
using Cardio.UI.Characters;
using Cardio.UI.Characters.Bosses;
using Cardio.UI.Characters.Ranged;
using Cardio.UI.Thrombi;
using Microsoft.Xna.Framework.Content;

namespace Cardio.UI
{
    public static class EnemyMapper
    {
        private static readonly Dictionary<string, Func<ContentManager, ObstacleGameObject>> LoadersMap =
            new Dictionary<string, Func<ContentManager, ObstacleGameObject>>();

        static EnemyMapper()
        {
            LoadersMap.Add("Microb", x => Microb.FromMetadata(x.Load<RangeEnemyMetadata>(@"Characters\Enemies\Microb"), x));
            LoadersMap.Add("AmorphicMicrob", x => Microb.FromMetadata(x.Load<RangeEnemyMetadata>(@"Characters\Enemies\AmorphicMicrob"), x));
            LoadersMap.Add("Glowing", x => Glowing.FromMetadata(x.Load<CloseCombatEnemyMetadata>(@"Characters\Enemies\Glowing"), x));
            LoadersMap.Add("Thrombus", x => Thrombus.FromMetadata(x.Load<ThrombusMetadata>(@"Characters\Obstacles\Thrombus"), x));
            LoadersMap.Add("Boss1", x => Boss.FromMetadata(x.Load<BossMetadata>(@"Characters\Bosses\Boss1"), x));
            LoadersMap.Add("Enemy1", x => Microb.FromMetadata(x.Load<RangeEnemyMetadata>(@"Characters\Enemies\Enemy1"), x));
            LoadersMap.Add("Enemy2", x => Microb.FromMetadata(x.Load<RangeEnemyMetadata>(@"Characters\Enemies\Enemy2"), x));
            LoadersMap.Add("Enemy3", x => Microb.FromMetadata(x.Load<RangeEnemyMetadata>(@"Characters\Enemies\Enemy3"), x));
            LoadersMap.Add("Enemy4", x => Microb.FromMetadata(x.Load<RangeEnemyMetadata>(@"Characters\Enemies\Enemy4"), x));
            LoadersMap.Add("Thrombus1", x => Thrombus.FromMetadata(x.Load<ThrombusMetadata>(@"Characters\Obstacles\Thrombus1"), x));
            LoadersMap.Add("Thrombus2", x => Thrombus.FromMetadata(x.Load<ThrombusMetadata>(@"Characters\Obstacles\Thrombus2"), x));
        }

        public static ObstacleGameObject GetEnemy(string characterName, ContentManager manager)
        {
            Func<ContentManager, ObstacleGameObject> getter;
            LoadersMap.TryGetValue(characterName, out getter);
            if (getter != null)
            {
                return getter(manager);
            }
            return null;
        }

        public static Boss GetBoss(string bossName, ContentManager manager)
        {
            return GetEnemy(bossName, manager) as Boss;
        }

    }
}
