using System;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Characters
{
    public class AliveGameObjectMetadata
    {
        public float MaxHealth { get; set; }

        public String ThinkCloudPath { get; set; }

        public String SoundPath { get; set; }

        public String ContentPath { get; set; }

        public Vector2 Size { get; set; }

        public String DeathAnimation { get; set; }

        public String DamageAnimation { get; set; }
    }
}
