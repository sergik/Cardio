using System;

namespace Cardio.Phone.Shared.Levels
{
    public class GameStoryEntry
    {
        public String Name { get; set;}

        public Func<Level> GetLevel { get; set;}
    }
}