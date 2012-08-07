using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cardio.UI.Characters.Bosses
{
    public class BossMetadata : ObstacleMetadata
    {
        public float AttackDamage
        {
            get;
            set;
        }

        public List<String> EnemyTypes
        {
            get;
            set;
        }

        public int EnemiesToGenerateMin
        {
            get;
            set;
        }

        public int EnemiesToGenerateMax
        {
            get;
            set;
        }

        public int GenerateEnemiesIntervalMin
        {
            get;
            set;
        }

        public int GenerateEnemiesIntervalMax
        {
            get;
            set;
        }

        public float AttackRange
        {
            get;
            set;
        }

        public float AttackInterval
        {
            get;
            set;
        }

        public float BulletSpeed
        {
            get;
            set;
        }

        public float AttackRateMin
        {
            get;
            set;
        }

        public float AttackRateMax
        {
            get;
            set;
        }

        public string BulletContentPath
        {
            get; set;
        }
    }
}
