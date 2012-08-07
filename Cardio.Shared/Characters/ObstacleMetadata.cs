﻿using Microsoft.Xna.Framework;

namespace Cardio.UI.Characters
{
    public class ObstacleMetadata : AliveGameObjectMetadata
    {
        public float StopDistance { get; set; }

        public int PlayerAttackPriority { get; set; }

        public Vector2 BiomaterialGeneratedInterval { get; set; }
    }
}
