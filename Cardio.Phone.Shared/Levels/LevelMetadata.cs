using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Characters;
using Cardio.Phone.Shared.Levels;

namespace Cardio.Phone.Shared.Levels
{
    //[Serializable]
    public class LevelMetadata
    {
        public String Name { get; set; }

        public String Background { get; set; }

        public String Melody { get; set; }

        public float Distance { get; set; }

        public List<EnemyMetadata> Enemies { get; set; }

        public List<EnemyMetadata> Obstacles { get; set; }

        public List<EnemyMetadata> Bosses { get; set; }

        public List<TextMetadata> Messages { get; set; }

        public LevelMetadata()
        {
            Enemies = new List<EnemyMetadata>();
            Obstacles = new List<EnemyMetadata>();
            Bosses = new List<EnemyMetadata>();
            Messages = new List<TextMetadata>();
        }
    }
}
