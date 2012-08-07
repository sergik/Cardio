using System;

namespace Cardio.UI.Levels
{
    public class GameStoryEntry
    {
        public String Name { get; set;}

        public Func<Level> GetLevel { get; set;}
    }
}