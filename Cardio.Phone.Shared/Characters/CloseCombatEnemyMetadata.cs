﻿using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Characters
{
    public class CloseCombatEnemyMetadata: ObstacleMetadata
    {
        public float AttackDamage { get; set; }

        public float AttackRange { get; set; }

        public float MaxVelocity { get; set; }

        public float Acceleration { get; set; }
    }
}
